using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NineDotAssessment.Application.Common.Helpers;
using NineDotAssessment.Application.Common.Models;
using NineDotAssessment.Application.Interfaces;
using NineDotAssessment.Core.Entities;

namespace NineDotAssessment.Application.Features.Account.Commands;

public class CreatePinCommand: IRequest<BaseResponse<bool>>
{
    public string PinCode { get; set; } = string.Empty;
    public string ConfirmPinCode { get; set; } = string.Empty;
    public long UserId { get; set; }

}


public class CreatePinCommandValidator: AbstractValidator<CreatePinCommand>
{
    public CreatePinCommandValidator()
    {
     RuleFor(p => p.PinCode).Matches(@"^\d{4}$").WithMessage("Please use a 4-digit PIN.");
     RuleFor(p => p.UserId).GreaterThan(0).WithMessage("Please enter a valid User Id");
     RuleFor(x => x).Must(request => request.PinCode == request.ConfirmPinCode).WithMessage("Unmatched PIN");
     RuleFor(x => x.ConfirmPinCode).NotEmpty().WithMessage("Confirm PIN is required.");
    }
}


public class CreatePinCommandHandler : IRequestHandler<CreatePinCommand, BaseResponse<bool>>
{
    private readonly IApplicationDbContext _dbContext;

    public CreatePinCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<BaseResponse<bool>> Handle(CreatePinCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == request.UserId);
        if (user == null) return new BaseResponse<bool>(false, 404, "no user found.");
        var exisitingPin = await _dbContext.AuthPins.FirstOrDefaultAsync(ap => ap.NineDotApplicationUserId == request.UserId);
        if (exisitingPin != null) return new BaseResponse<bool>(false, 400, "PIN has already been setup.");

        var (Hash, Salt) = Utilities.CreateCredential(request.PinCode);
         AuthPin newPin = new AuthPin(request.UserId, Hash, Salt);
        await _dbContext.AuthPins.AddAsync(newPin, cancellationToken);
        int addPinResult = await _dbContext.SaveChangesAsync(cancellationToken);
        return new BaseResponse<bool>((addPinResult > 0));
    }
}
