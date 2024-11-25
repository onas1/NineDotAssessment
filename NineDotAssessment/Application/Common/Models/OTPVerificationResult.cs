

using NineDotAssessment.Core.Entities;

namespace NineDotAssessment.Application.Common.Models;

public class OtpVerificationResult
{
    public ApplicationUser ApplicationUser { get; set; }
    public bool IsVerified { get; set; }
    public string Message { get; set; }
}
