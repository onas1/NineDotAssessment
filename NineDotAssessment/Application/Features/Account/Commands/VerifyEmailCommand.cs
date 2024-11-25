

using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NineDotAssessment.Application.Common.Helpers;
using NineDotAssessment.Application.Common.Models;
using NineDotAssessment.Application.Interfaces;
using NineDotAssessment.Core.Enums;
using NineDotAssessment.Core.Events;

namespace NineDotAssessment.Application.Features.Account.Commands;

public class VerifyEmailCommand: IRequest<BaseResponse<OtpVerificationResult>>
{
    public long UserId { get; set; }
    public string VerificationCode { get; set; }
    public long VerificationId { get; set; }
}





public class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
{
    public VerifyEmailCommandValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0).WithMessage("Please enter a valid user Id");
        RuleFor(x => x.VerificationCode).NotEmpty().WithMessage("Verification code cannot be empty.");
        RuleFor(x => x.VerificationId).GreaterThan(0).WithMessage("verificationId cannot be empty.");

    }
}




public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, BaseResponse<OtpVerificationResult>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMediator _mediator;

    public VerifyEmailCommandHandler(IApplicationDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }





    public async Task<BaseResponse<OtpVerificationResult>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<OtpVerificationResult>();

        var user = await _dbContext.ApplicationUsers
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            response.Message = "No user found.";
            return response;
        }

        var oneTimePassword = await _dbContext.OtpVerifications
            .FirstOrDefaultAsync(o => o.Id == request.VerificationId, cancellationToken);

        if (oneTimePassword == null || !oneTimePassword.IsValid || oneTimePassword.VerificationType != OtpType.EmailVerification)
        {
            response.Message = "Invalid OTP credentials.";
            return response;
        }

        if (oneTimePassword.Exp <= DateTime.Now)
        {
            response.Message = "This OTP has expired. Please request a new OTP.";
            return response;
        }

        bool isValidCredential = Utilities.VerifyCredential(
            request.VerificationCode,
            oneTimePassword.VerificationCodeHash,
            oneTimePassword.VerificationCodeSalt);

        if (isValidCredential)
        {
            var successfulEmailVerificationEvent = new EmailVerificationEvent(user, oneTimePassword);
            await _mediator.Publish(successfulEmailVerificationEvent, cancellationToken);
            response = new BaseResponse<OtpVerificationResult>(new OtpVerificationResult
            {
                ApplicationUser = user,
                IsVerified = true,
                Message = "Email verification was successful."
            });
            return response;
        }
        else
        {
            response.Message = "Incorrect OTP. Please verify and try again.";
        }

        return response;
    }






}

