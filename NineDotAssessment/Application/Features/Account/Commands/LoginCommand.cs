using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NineDotAssessment.Application.Common.Helpers;
using NineDotAssessment.Application.Common.Models;
using NineDotAssessment.Application.Interfaces;

namespace NineDotAssessment.Application.Features.Account.Commands;

public class LoginCommand: IRequest<BaseResponse<LoginResult>>
{
    public string ICNumber { get; set; }
    public string Pin { get; set; }
}



public class LoginCommandValidator: AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
            RuleFor(l => l.ICNumber).NotEmpty();
    }
}





public class LoginCommandHandler : IRequestHandler<LoginCommand, BaseResponse<LoginResult>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IJWTService _jWTService;

    public LoginCommandHandler(IApplicationDbContext dbContext, IJWTService jWTService)
    {
        _dbContext = dbContext;
        _jWTService = jWTService;
    }
    public async Task<BaseResponse<LoginResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<LoginResult>();

        // Fetch user by ICNumber
        var user = await _dbContext.ApplicationUsers
            .FirstOrDefaultAsync(u => u.ICNumber == request.ICNumber, cancellationToken);

        if (user == null)
        {
            response.Message = "Invalid IC Number. User not found.";
            return response;
        }
        response.Data = new LoginResult
        {
            ApplicationUser = user,
            Succeeded = false
        };
        if (!user.IsPhoneNumberVerified || !user.IsEmailVerified)
        {
            response.Message = "Complete registration process to login";
            return response;
        }
       

        var authPin = await _dbContext.AuthPins.FirstOrDefaultAsync(p => p.NineDotApplicationUserId == user.Id);

        if (authPin == null)
        {
            response.Message = "Complete pin setup to login";
            return response;
        }

        bool isValidPIN = Utilities.VerifyCredential(request.Pin, authPin.PINHash, authPin.PINSalt);
        if (string.IsNullOrWhiteSpace(request.Pin) || !isValidPIN)
        {
            response.Message = "Invalid login details";
            response.Data = new LoginResult { Succeeded = false};
            return response;
        }

        response.Data.token = _jWTService.GenerateToken(user);
        response.Data.Succeeded = true;
        response.StatusCode = 200;
        response.Message = "User successfully logged in.";

        return response;
    }


}









