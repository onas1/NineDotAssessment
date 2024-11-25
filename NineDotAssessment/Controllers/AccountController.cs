using Microsoft.AspNetCore.Mvc;
using NineDotAssessment.Application.Common.Helpers;
using NineDotAssessment.Application.Features.Account.Commands;

namespace NineDotAssessment.Controllers
{
    [Route(ApiActions.Account)]
    public class AccountController: ApiControllerBase
    {




        [HttpPost(ApiActions.RegisterUser, Name = nameof(ApiActions.RegisterUser))]
        public async Task<IActionResult> RegisterUser(UserRegistrationCommand command) =>
            Ok(await Mediator.Send(command));



        [HttpPost(ApiActions.VerifyPhoneNumber, Name = nameof(ApiActions.VerifyPhoneNumber))]
        public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberCommand command) => Ok(await Mediator.Send(command));



        [HttpPost(ApiActions.VerifyEmail, Name = nameof(ApiActions.VerifyEmail))]
        public async Task<IActionResult> VerifyEmail(VerifyEmailCommand command) => Ok(await Mediator.Send(command));



        [HttpPost(ApiActions.Login, Name = nameof(ApiActions.Login))]
        public async Task<IActionResult> Login(LoginCommand command) => Ok(await Mediator.Send(command));


        [HttpPost(ApiActions.CreatePin, Name = nameof(ApiActions.CreatePin))]
        public async Task<IActionResult> CreatePin(CreatePinCommand command) => Ok(await Mediator.Send(command));



        [HttpPost(ApiActions.AcceptPrivacyPolicy, Name = nameof(ApiActions.AcceptPrivacyPolicy))]
        public async Task<IActionResult> AcceptPrivacyPolicy(AcceptPrivacyPolicyCommand command) => Ok(await Mediator.Send(command));



        [HttpPost(ApiActions.RequestOTP, Name = nameof(ApiActions.RequestOTP))]
        public async Task<IActionResult> RequestOTP(RequestOtpCommand command) => Ok(await Mediator.Send(command));






        //public IActionResult OTPVerification()
        //{
        //    return Ok();
        //}


        //public IActionResult Login()
        //{
        //    return Ok();
        //}

    }
}
