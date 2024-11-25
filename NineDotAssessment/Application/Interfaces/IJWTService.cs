
using NineDotAssessment.Core.Entities;

namespace NineDotAssessment.Application.Interfaces;

public interface IJWTService
{
    string GenerateToken(ApplicationUser user);

}
