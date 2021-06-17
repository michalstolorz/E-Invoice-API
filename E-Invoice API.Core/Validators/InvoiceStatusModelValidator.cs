using E_Invoice_API.Common.Enums;
using E_Invoice_API.Core.Exceptions;
using E_Invoice_API.Core.Models;
using FluentValidation;

namespace E_Invoice_API.Core.Validators
{
    public class InvoiceStatusModelValidator : AbstractValidator<InvoiceStatusModel>
    {
        public InvoiceStatusModelValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.Id)} is invalid.");
                });

            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.UserId)} is invalid.");
                });

            RuleFor(x => x.Status.ToString())
                .IsEnumName(typeof(EnumInvoiceStatus))
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.Status)} is invalid.");
                });
        }
    }
}
