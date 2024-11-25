

namespace NineDotAssessment.Application.Common.Helpers;

public static class ApiActions
{
    public const string V1 = "api/v1";
    #region Account
    public const string Account = $"{V1}/account";

    public const string RegisterUser = "register-user";
    public const string VerifyPhoneNumber = "verify-phone-number";
    public const string VerifyEmail = "verify-email";
    public const string Login = "login";
    public const string AcceptPrivacyPolicy = "accept-privacy-policy";
    public const string CreatePin = "create-pin";
    public const string RequestOTP = "request-otp";


    #endregion
}
