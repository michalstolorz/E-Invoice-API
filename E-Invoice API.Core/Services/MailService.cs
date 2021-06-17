using E_Invoice_API.Common.Enums;
using E_Invoice_API.Common.Models;
using E_Invoice_API.Core.DTO.Request;
using E_Invoice_API.Core.DTO.Response;
using E_Invoice_API.Core.Exceptions;
using E_Invoice_API.Core.Interfaces.Services;
using E_Invoice_API.Core.Validators;
using FluentValidation;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace E_Invoice_API.Core.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task<MailNotificationResponse> SendEmailNotificationAsync(MailNotificationRequest request, CancellationToken cancellationToken)
        {
            var validator = new MailNotificationRequestValidator();
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var templateName = "NotificationTemplate";
            string FilePath = Directory.GetCurrentDirectory() + $"\\Templates\\{templateName}.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            string status = ChangeInvoiceStatus(request.CurrentStatus);
            MailText = MailText.Replace("[username]", request.UserName).Replace("[status]", status);

            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Mail)
            };

            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = "E-Invoice service status";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            try
            {
                await smtp.SendAsync(email);
            }
            catch(CustomException ex)
            {
                new ServiceException(ErrorCodes.MailSendingHasFailed, $"Sending mail has failed. Exception message: {ex.Message}");
            }
            smtp.Disconnect(true);

            return new MailNotificationResponse() { MailSendToEmail = request.ToEmail, EmailTemplate = templateName };
        }

        private string ChangeInvoiceStatus(EnumInvoiceStatus invoiceStatus)
        {
            switch (invoiceStatus)
            {
                case EnumInvoiceStatus.Active:
                    return "activated.";
                case EnumInvoiceStatus.Inactive:
                    return "deactivated.";
                case EnumInvoiceStatus.FirstActivation:
                    return "activated for the first time.";
                default:
                    throw new ServiceException(ErrorCodes.NoConversionForGivenStatus, $"No conversion for given status {invoiceStatus}");
            }
        }
    }
}
