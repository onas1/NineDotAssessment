using FluentValidation;
using MediatR;
using NineDotAssessment.Application.Common.Models;
using NineDotAssessment.Application.Interfaces;
using NineDotAssessment.Core.Entities;
using NineDotAssessment.Core.Events;

namespace NineDotAssessment.Application.Features.Account.Commands
{
    public class UserRegistrationCommand: IRequest<BaseResponse<UserRegistrationResult>>
    {
        public string CustomerName { get; set; }
        public string ICNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }



    public class UserRegistrationValidator : AbstractValidator<UserRegistrationCommand>
    {

        public UserRegistrationValidator()
        {
            RuleFor(x => x.CustomerName).NotEmpty().WithMessage("Please enter a valid Customer name");
            RuleFor(x => x.ICNumber).NotEmpty().
                WithMessage("Please enter a valid ICU Number")
                .MaximumLength(10).WithMessage("Please enter a valid ICU Number");

            RuleFor(x => x.Email).EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
                .WithMessage("Enter a valid email address");


        }

    }






    public class UserRegistrationHandler : IRequestHandler<UserRegistrationCommand, BaseResponse<UserRegistrationResult>>
    {
        #region Properties
        private readonly IApplicationDbContext _dbContext;
        private readonly IMediator _mediator;

        #endregion



        #region constructor
        public UserRegistrationHandler(IApplicationDbContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        #endregion


        #region Method
        public async Task<BaseResponse<UserRegistrationResult>> Handle(UserRegistrationCommand command, CancellationToken cancellationToken)
        {
            BaseResponse<UserRegistrationResult> result = new();

            if (_dbContext.ApplicationUsers.Any(user => 
             user.ICNumber == command.ICNumber ||
             user.PhoneNumber == command.PhoneNumber ||
             user.Email == command.Email))
                result = new BaseResponse<UserRegistrationResult>()
                {
                    Message = "This user already exist. Please sign in or contact customer support for assistance.",
                    StatusCode = 400
                };

            var registrationResultData = _dbContext.ApplicationUsers
                .Add(new ApplicationUser(command.CustomerName, command.ICNumber, command.Email, command.PhoneNumber)).Entity;
          if( await _dbContext.SaveChangesAsync(cancellationToken)> 0)
            {
                var registrationEvent = new UserRegistrationEvent(registrationResultData);
              await  _mediator.Publish(registrationEvent, cancellationToken);
                result = new BaseResponse<UserRegistrationResult>
                {
                    Message = "User registration successful",
                    StatusCode = 200,
                    Data = new UserRegistrationResult
                    {
                        ApplicationUser = registrationResultData,
                        NewOtpId = registrationEvent.NewOtpId.Value
                    }
                };
            }


            return result;
        }
        #endregion

    }
}
