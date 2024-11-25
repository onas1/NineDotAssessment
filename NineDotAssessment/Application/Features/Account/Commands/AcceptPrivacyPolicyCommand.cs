

using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NineDotAssessment.Application.Common.Models;
using NineDotAssessment.Application.Interfaces;

namespace NineDotAssessment.Application.Features.Account.Commands;

public class AcceptPrivacyPolicyCommand: IRequest<BaseResponse<bool>>
{
    public long UserId { get; set; }
}


public class AcceptPrivacyPolicyCommandValidator: AbstractValidator<AcceptPrivacyPolicyCommand>
{
    public AcceptPrivacyPolicyCommandValidator()
    {
        RuleFor(p => p.UserId).GreaterThan(0).WithMessage("enter a valid user Id");
    }
}


public class AcceptPrivacyPolicyCommandHandler : IRequestHandler<AcceptPrivacyPolicyCommand, BaseResponse<bool>>
{
    private readonly IApplicationDbContext _dbContext;

    public AcceptPrivacyPolicyCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<BaseResponse<bool>> Handle(AcceptPrivacyPolicyCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        if (user == null) return new BaseResponse<bool>(false, 404, "no user found.");
        user.AcceptPrivacyPolicy();
        int acceptPolicyResult = await _dbContext.SaveChangesAsync(cancellationToken);
        return new BaseResponse<bool>((acceptPolicyResult > 0));
    }
}
