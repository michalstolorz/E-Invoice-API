using E_Invoice_API.Common.Enums;
using E_Invoice_API.Core.DTO.Request;
using E_Invoice_API.Core.Exceptions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Invoice_API.Core.Validators
{
    public class LogInvoiceStatusChangeRequestValidator : AbstractValidator<LogInvoiceStatusChangeRequest>
    {
        public LogInvoiceStatusChangeRequestValidator()
        {
            RuleFor(x => x.InvoiceStatusId)
                .GreaterThan(0)
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.InvoiceStatusId)} is invalid.");
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
