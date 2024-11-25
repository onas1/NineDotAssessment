

using NineDotAssessment.Core.Entities;

namespace NineDotAssessment.Application.Common.Models;

public class LoginResult
{
    public ApplicationUser  ApplicationUser { get; set; }
    public string token { get; set; }
    public bool Succeeded { get; set; }

}
