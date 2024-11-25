
using MediatR;
using NineDotAssessment.Application.Common.Helpers;
using NineDotAssessment.Application.Interfaces;
using NineDotAssessment.Core.Entities;
using NineDotAssessment.Core.Enums;
using NineDotAssessment.Core.Events;

namespace NineDotAssessment.Application.Features.Account.EventHandlers;

public class PhoneNumberVerificationEventHandler : INotificationHandler<PhoneNumberVerificationEvent>
{
    private readonly ILogger<PhoneNumberVerificationEventHandler> _emailService;
    private readonly IApplicationDbContext _dbContext;

    public PhoneNumberVerificationEventHandler(ILogger<PhoneNumberVerificationEventHandler> emailService, IApplicationDbContext dbContext)
    {
        _emailService = emailService;
        _dbContext = dbContext;
    }
    public async Task Handle(PhoneNumberVerificationEvent notification, CancellationToken cancellationToken)
    {
        notification.OneTimePassword.InValidateOTP();
        notification.ApplicationUser.ValidatePhoneNumber();
        await _dbContext.SaveChangesAsync(cancellationToken);

    }
}
  

