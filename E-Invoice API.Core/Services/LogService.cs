using AutoMapper;
using E_Invoice_API.Common.Enums;
using E_Invoice_API.Core.DTO.Request;
using E_Invoice_API.Core.DTO.Response;
using E_Invoice_API.Core.Helper;
using E_Invoice_API.Core.Interfaces.Repositories;
using E_Invoice_API.Core.Interfaces.Services;
using E_Invoice_API.Core.Models;
using E_Invoice_API.Core.Validators;
using E_Invoice_API.Data.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace E_Invoice_API.Core.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IChangeInvoiceStatusHelper _changeInvoiceStatusHelper;

        public LogService(ILogRepository logRepository, IMapper mapper, IDateTimeProvider dateTimeProvider, IChangeInvoiceStatusHelper changeInvoiceStatusHelper)
        {
            _logRepository = logRepository;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
            _changeInvoiceStatusHelper = changeInvoiceStatusHelper;
        }

        public async Task<LogInvoiceStatusResponse> LogInvoiceStatusFirstActivationAsync(LogInvoiceStatusChangeRequest request, CancellationToken cancellationToken)
        {
            var validator = new LogInvoiceStatusChangeRequestValidator();
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var log = new Log()
            {
                ChangeDateTime = _dateTimeProvider.GetDateTimeNow(),
                ToCheckFirstActivationDateTime = _dateTimeProvider.GetDateTimeNow(),
                PreviousStatus = EnumInvoiceStatus.Nonexist,
                CurrentStatus = EnumInvoiceStatus.FirstActivation,
                InvoiceStatusId = request.InvoiceStatusId
            };

            var result = await _logRepository.AddAsync(log, cancellationToken);

            var mapped = _mapper.Map<LogInvoiceStatusResponse>(request);

            return _mapper.Map<Log, LogInvoiceStatusResponse>(result, mapped);
        }

        public async Task<LogInvoiceStatusResponse> LogInvoiceStatusChangeAsync(LogInvoiceStatusChangeRequest request, CancellationToken cancellationToken)
        {
            var validator = new LogInvoiceStatusChangeRequestValidator();
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var log = new Log()
            {
                ChangeDateTime = _dateTimeProvider.GetDateTimeNow(),
                CurrentStatus = request.Status,
                InvoiceStatusId = request.InvoiceStatusId
            };
            
            if(request.UserLogsModel.Count() == 1)
            {
                log.PreviousStatus = EnumInvoiceStatus.FirstActivation;
            }
            else
            {
                log.PreviousStatus = _changeInvoiceStatusHelper.ChangeInvoiceStatus(log.CurrentStatus);
            }

            var result = await _logRepository.AddAsync(log, cancellationToken);

            var mapped = _mapper.Map<LogInvoiceStatusResponse>(request);

            return _mapper.Map<Log, LogInvoiceStatusResponse>(result, mapped);
        }
    }
}
