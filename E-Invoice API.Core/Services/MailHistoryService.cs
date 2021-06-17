using E_Invoice_API.Core.Interfaces.Services;
using E_Invoice_API.Core.Interfaces.Repositories;
using E_Invoice_API.Core.DTO.Request;
using System.Threading;
using System.Threading.Tasks;
using E_Invoice_API.Core.Validators;
using FluentValidation;
using E_Invoice_API.Data.Models;
using E_Invoice_API.Core.Helper;

namespace E_Invoice_API.Core.Services
{
    public class MailHistoryService : IMailHistoryService
    {
        private readonly IMailHistoryRepository _mailHistoryRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public MailHistoryService(IMailHistoryRepository mailHistoryRepository, IDateTimeProvider dateTimeProvider)
        {
            _mailHistoryRepository = mailHistoryRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task AddMailHistoryAsync(AddMailHistoryRequest request, CancellationToken cancellationToken)
        {
            var validator = new AddMailHistoryRequestValidator();
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var newMailHistory = new MailHistory()
            {
                UserId = request.UserId,
                MailSendToEmail = request.MailSendToEmail,
                SendMailDateTime = _dateTimeProvider.GetDateTimeNow(),
                EmailTemplate = request.EmailTemplate
            };

            await _mailHistoryRepository.AddAsync(newMailHistory, cancellationToken);
        }
    }
}
