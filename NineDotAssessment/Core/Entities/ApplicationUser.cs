namespace NineDotAssessment.Core.Entities
{
    public class ApplicationUser : BaseEntity
    {

        #region Constructor
        public ApplicationUser(string customerName, string iCNumber, string email, string phoneNumber)
        {
            CustomerName = customerName;
            ICNumber = iCNumber;
            Email = email;
            PhoneNumber = phoneNumber;
            InValidateEmail();
            InValidatePhoneNumber();
            RejectPrivacyPolicy();
        }
        #endregion



        #region Properties
        public string CustomerName { get; private set; }
        public string ICNumber { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public bool IsPhoneNumberVerified { get; private set; }
        public bool IsEmailVerified { get; private set; }
        public bool HasAcceptedPrivacyPolicy { get; set; }

        #endregion

        #region Methods
        public void ValidatePhoneNumber() => IsPhoneNumberVerified = true;
        public void InValidatePhoneNumber() => IsPhoneNumberVerified = false;
        public void ValidateEmail() => IsEmailVerified = true;
        public void InValidateEmail() => IsEmailVerified = false;
        public void AcceptPrivacyPolicy() => HasAcceptedPrivacyPolicy = true;
        public void RejectPrivacyPolicy() => HasAcceptedPrivacyPolicy = false;

        #endregion

    }



}
