using E_Invoice_API.Core.Exceptions;
using FluentValidation;

namespace E_Invoice_API.Core.Validators
{
    public class IdValidator : AbstractValidator<int>
    {
        public IdValidator()
        {
            RuleFor(x => x)
                .GreaterThan(0)
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x)} is invalid.");
                });
        }
    }
}
