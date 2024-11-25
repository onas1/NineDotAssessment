using MediatR;
using NineDotAssessment.Application.Interfaces;
using NineDotAssessment.Core.Events;

namespace NineDotAssessment.Application.Features.Account.EventHandlers;

public class EmailVerificationEventHandler : INotificationHandler<EmailVerificationEvent>
{

    private readonly IApplicationDbContext _dbContext;
    public EmailVerificationEventHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Handle(EmailVerificationEvent notification, CancellationToken cancellationToken)
    {
        notification.OneTimePassword.InValidateOTP();
        notification.ApplicationUser.ValidateEmail();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
