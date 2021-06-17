using AutoMapper;
using E_Invoice_API.Common.Enums;
using E_Invoice_API.Core.DTO.Response;
using E_Invoice_API.Core.Exceptions;
using E_Invoice_API.Core.Helper;
using E_Invoice_API.Core.Interfaces.Repositories;
using E_Invoice_API.Core.Interfaces.Services;
using E_Invoice_API.Core.Models;
using E_Invoice_API.Core.Validators;
using E_Invoice_API.Data.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace E_Invoice_API.Core.Services
{
    public class InvoiceStatusService : IInvoiceStatusService
    {
        private readonly IInvoiceStatusRepository _invoiceStatusRepository;
        private readonly IMapper _mapper;
        private readonly IUserContextProvider _userContextProvider;
        private readonly IChangeInvoiceStatusHelper _changeInvoiceStatusHelper;

        public InvoiceStatusService(IInvoiceStatusRepository invoiceStatusRepository, IMapper mapper, IUserContextProvider userContextProvider, IChangeInvoiceStatusHelper changeInvoiceStatusHelper)
        {
            _invoiceStatusRepository = invoiceStatusRepository;
            _mapper = mapper;
            _userContextProvider = userContextProvider;
            _changeInvoiceStatusHelper = changeInvoiceStatusHelper;
        }

        public async Task<GetInvoiceStatusDetailsResponse> GetInvoiceStatusByIdAsync(int invoiceStatusId, CancellationToken cancellationToken)
        {
            var validator = new IdValidator();
            await validator.ValidateAndThrowAsync(invoiceStatusId, cancellationToken);

            var result = await _invoiceStatusRepository.GetByIdAsync(invoiceStatusId, cancellationToken,
                include: x => x
                .Include(x => x.User)
                .Include(x => x.UserLogs));

            if (result == null)
            {
                throw new ServiceException(ErrorCodes.InvoiceStatusWithGivenIdNotFound, $"Invoice status with provided id {invoiceStatusId} doesn't exist");
            }

            return _mapper.Map<GetInvoiceStatusDetailsResponse>(result);
        }

        public async Task<GetInvoiceStatusDetailsResponse> GetInvoiceStatusByLoggedUserAsync(CancellationToken cancellationToken)
        {
            var userId = (int)_userContextProvider.UserId;
            var validator = new IdValidator();
            await validator.ValidateAndThrowAsync(userId, cancellationToken);

            var result = await _invoiceStatusRepository.GetSingleAsync(x => x.UserId == userId, cancellationToken,
                include: x => x
                .Include(x => x.User)
                .Include(x => x.UserLogs));

            if (result == null)
            {
                throw new ServiceException(ErrorCodes.InvoiceStatusWithGivenUserIdNoFound, $"Invoice status with provided used id {userId} doesn't exists");
            }

            return _mapper.Map<GetInvoiceStatusDetailsResponse>(result);
        }

        public async Task<InvoiceStatusModel> AddInvoiceStatusAsync(CancellationToken cancellationToken)
        {
            var userId = (int)_userContextProvider.UserId;
            var validator = new IdValidator();
            await validator.ValidateAndThrowAsync(userId, cancellationToken);

            if (await _invoiceStatusRepository.AnyAsync(x => x.UserId == userId, cancellationToken))
            {
                throw new ServiceException(ErrorCodes.InvoiceStatusAlreadyExists, $"Invoice status with provided used id {userId} already exists");
            }

            var newClient = new InvoiceStatus()
            {
                UserId = userId,
                Status = EnumInvoiceStatus.FirstActivation
            };

            var newInvoiceStatus = await _invoiceStatusRepository.AddAsync(newClient, cancellationToken);

            var result = await _invoiceStatusRepository.GetByIdAsync(newInvoiceStatus.Id, cancellationToken,
                include: x => x
                .Include(x => x.User));

            return _mapper.Map<InvoiceStatusModel>(result);
        }

        public async Task<InvoiceStatusModel> ChangeInvoiceStatusAsync(CancellationToken cancellationToken)
        {
            var userId = (int)_userContextProvider.UserId;
            var validator = new IdValidator();
            await validator.ValidateAndThrowAsync(userId, cancellationToken);

            var result = await _invoiceStatusRepository.GetSingleAsync(x => x.UserId == userId, cancellationToken,
                include: x => x
                .Include(x => x.User)
                .Include(x => x.UserLogs));

            if (result == null)
            {
                throw new ServiceException(ErrorCodes.InvoiceStatusWithGivenUserIdNoFound, $"Invoice status with provided used id {userId} doesn't exists");
            }

            result.Status = _changeInvoiceStatusHelper.ChangeInvoiceStatus(result.Status);

            await _invoiceStatusRepository.UpdateAsync(result, cancellationToken);

            return _mapper.Map<InvoiceStatusModel>(result);
        }
    }
}
