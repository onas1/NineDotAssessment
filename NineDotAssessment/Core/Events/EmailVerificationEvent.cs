using NineDotAssessment.Core.Entities;

namespace NineDotAssessment.Core.Events;


public class EmailVerificationEvent(ApplicationUser applicationUser, OtpVerification oneTimePassword) : BaseEvent
{
    public ApplicationUser ApplicationUser { get; private set; } = applicationUser;
    public long? NewOtpId { get; set; }
    public OtpVerification OneTimePassword { get; private set; } = oneTimePassword;
}
