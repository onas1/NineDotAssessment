

namespace NineDotAssessment.Core.Entities
{
    public class AuthPin: BaseEntity
    {
        public AuthPin(){ }

        public AuthPin(long userId, string pinHash, string pinSalt)
        {
            PINSalt = pinSalt;
            PINHash = pinHash;
            NineDotApplicationUserId = userId;
            NineDotApplicationUser = default!;
        }
        public string PINHash { get;  private set; }
        public string PINSalt { get; private set; }
        public long NineDotApplicationUserId { get; private set; }
        public ApplicationUser NineDotApplicationUser { get; private set; }
    }





}
