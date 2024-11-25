

using NineDotAssessment.Core.Entities;

namespace NineDotAssessment.Core.Events
{
    public class UserRegistrationEvent(ApplicationUser applicationUser) : BaseEvent
    {
        public ApplicationUser NewApplicationUser { get; private set; } = applicationUser;
        public long? NewOtpId { get; set; }
    }
}
