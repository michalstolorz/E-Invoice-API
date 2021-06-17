using E_Invoice_API.Core.DTO.Request;
using E_Invoice_API.Core.Exceptions;
using FluentValidation;

namespace E_Invoice_API.Core.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.Email)} is invalid.");
                });

            RuleFor(x => x.Password)
                .NotEmpty()
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.Password)} is invalid.");
                });
        }
    }
}
