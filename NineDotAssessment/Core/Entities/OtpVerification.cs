using NineDotAssessment.Core.Enums;

namespace NineDotAssessment.Core.Entities
{
    public class OtpVerification : BaseEntity
    {

        public string VerificationCodeHash { get; private set; }
        public string VerificationCodeSalt { get; private set; }

        public OtpType VerificationType { get; private set; }
        public bool IsValid { get; private set; } = true;
        public long ApplicationUserId { get; private set; }
        public DateTime Exp { get; private set; }

        public OtpVerification(string verificationCodeHash, string verificationCodeSalt , OtpType verificationType, long applicationUserId, DateTime exp)
        {
            VerificationCodeHash = verificationCodeHash;
            VerificationCodeSalt = verificationCodeSalt;
            VerificationType = verificationType;
            ApplicationUserId = applicationUserId;
            Exp = exp;
        }

        public void InValidateOTP() => IsValid = false;
    }
}
