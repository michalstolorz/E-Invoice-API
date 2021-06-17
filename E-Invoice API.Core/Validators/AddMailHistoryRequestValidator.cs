using E_Invoice_API.Core.DTO.Request;
using E_Invoice_API.Core.Exceptions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Invoice_API.Core.Validators
{
    public class AddMailHistoryRequestValidator : AbstractValidator<AddMailHistoryRequest>
    {
        public AddMailHistoryRequestValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.UserId)} is invalid.");
                });

            RuleFor(x => x.MailSendToEmail)
                .EmailAddress()
                .NotEmpty()
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.MailSendToEmail)} is invalid.");
                });

            RuleFor(x => x.EmailTemplate)
                .NotEmpty()
                .OnAnyFailure(x =>
                {
                    throw new ValidatorException(ErrorCodes.InvalidParameter, $"Parameter {nameof(x.EmailTemplate)} is invalid.");
                });
        }
    }
}
