using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NineDotAssessment.Application.Common.Helpers;
using NineDotAssessment.Application.Common.Models;
using NineDotAssessment.Application.Interfaces;
using NineDotAssessment.Core.Entities;
using NineDotAssessment.Core.Enums;

namespace NineDotAssessment.Application.Features.Account.Commands;

public class RequestOtpCommand: IRequest<BaseResponse<RequestOtpResult>>
{
    public long UserId { get; set; } 
    public OtpType OtpType { get; set; }
}


public class RequestOtpCommandValidator : AbstractValidator<RequestOtpCommand>
{
    public RequestOtpCommandValidator()
    {
        RuleFor(x => x.UserId)
        .NotEmpty()
        .WithMessage("UserId is required.");

        RuleFor(x => x.OtpType)
            .IsInEnum()
            .WithMessage("OtpType must be a valid type.");
    }
}



public class RequestOtpCommandHandler : IRequestHandler<RequestOtpCommand, BaseResponse<RequestOtpResult>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ILogger<RequestOtpCommandHandler> _notificationService;

    public RequestOtpCommandHandler(IApplicationDbContext dbContext, ILogger<RequestOtpCommandHandler> notificationService)
    {
        _dbContext = dbContext;
        _notificationService = notificationService;
    }
    public async Task<BaseResponse<RequestOtpResult>> Handle(RequestOtpCommand request, CancellationToken cancellationToken)
    {
        // Find the user
        var user = await _dbContext.ApplicationUsers
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null) return new BaseResponse<RequestOtpResult>() { Message = "User not found" };



        // Generate OTP
        string otp = Utilities.GenerateOTP();
        var (hash, salt) = Utilities.CreateCredential(otp);

        // Save OTP to the database
        var otpVerification = new OtpVerification(hash, salt, request.OtpType, request.UserId, DateTime.Now.AddMinutes(10));
        await _dbContext.OtpVerifications.AddAsync(otpVerification, cancellationToken);
         await _dbContext.SaveChangesAsync(cancellationToken);
        string Message = $"Your one-time verification code is: {otp}";
        _notificationService.LogInformation(message: Message);
        return new BaseResponse<RequestOtpResult>()
        {
            Data = new RequestOtpResult
            {
                VerificationId = otpVerification.Id,
                Successful = true
            },
            Message = "One time password has been sent.",
            StatusCode = 200
        };


    }
}
