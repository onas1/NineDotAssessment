
using NineDotAssessment.Core.Entities;

namespace NineDotAssessment.Application.Common.Models;

public class UserRegistrationResult
{
    public long NewOtpId { get; set; }
    public ApplicationUser  ApplicationUser { get; set; }
}
