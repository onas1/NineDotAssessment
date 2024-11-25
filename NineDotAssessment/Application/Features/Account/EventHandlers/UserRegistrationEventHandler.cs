

using MediatR;
using NineDotAssessment.Application.Common.Helpers;
using NineDotAssessment.Application.Interfaces;
using NineDotAssessment.Core.Entities;
using NineDotAssessment.Core.Enums;
using NineDotAssessment.Core.Events;

namespace NineDotAssessment.Application.Features.Account.EventHandlers;

public class UserRegistrationEventHandler : INotificationHandler<UserRegistrationEvent>
{
    #region properties
    private readonly ILogger<UserRegistrationEventHandler> _smsService;
    private readonly IApplicationDbContext _dbContext;
    #endregion




    #region Constructor
    public UserRegistrationEventHandler(ILogger<UserRegistrationEventHandler> smsService, IApplicationDbContext dbContext )
    {
        _smsService = smsService;
        _dbContext = dbContext;
    }

    #endregion



    #region Method
    public async Task Handle(UserRegistrationEvent notification, CancellationToken cancellationToken)
    {

        string verificationCode = Utilities.GenerateOTP();
        var (Hash, Salt) = Utilities.CreateCredential(verificationCode);
        OtpVerification oneTimePassword = new(Hash,Salt, OtpType.PhoneNumberVerification, notification.NewApplicationUser.Id, DateTime.Now.AddMinutes(10));
        await _dbContext.OtpVerifications.AddAsync(oneTimePassword, cancellationToken);
        await _dbContext.SaveChangesAsync();
        // Assign the generated OTP ID to the event
        notification.NewOtpId = oneTimePassword.Id;
        string Message = $"This is your one-time verification code: {verificationCode}";
        _smsService.LogInformation(message: Message);

    }
    #endregion
}
