using E_Invoice_API.Common.Enums;
using E_Invoice_API.Core.DTO.Request;
using E_Invoice_API.Core.Exceptions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Invoice_API.Core.Validators
{
    public class MailNotificationRequestValidator : AbstractValidator<MailNotificationRequest>
    {
        public MailNotificationRequestValidator()
        {
            RuleFor(x => x.ToEmail)
                .NotEmpty()
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.ToEmail)} is invalid.");
                });

            RuleFor(x => x.UserName)
                .NotEmpty()
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.UserName)} is invalid.");
                });

            RuleFor(x => x.CurrentStatus.ToString())
                .IsEnumName(typeof(EnumInvoiceStatus))
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.CurrentStatus)} is invalid.");
                });
        }
    }
}
