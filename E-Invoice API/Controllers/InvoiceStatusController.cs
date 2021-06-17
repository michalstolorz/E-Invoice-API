using E_Invoice_API.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using E_Invoice_API.Core.DTO.Response;
using AutoMapper;
using E_Invoice_API.Core.DTO.Request;

namespace E_Invoice_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceStatusController : ControllerBase
    {
        private readonly IInvoiceStatusService _invoiceService;
        private readonly ILogService _logService;
        private readonly IMailService _mailService;
        private readonly IMailHistoryService _mailHistoryService;
        private readonly IMapper _mapper;

        public InvoiceStatusController(IInvoiceStatusService invoiceService, ILogService logService, IMailService mailService, IMailHistoryService mailHistoryService, IMapper mapper)
        {
            _invoiceService = invoiceService;
            _logService = logService;
            _mailService = mailService;
            _mailHistoryService = mailHistoryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get InvoiceStatus by its id
        /// </summary>
        /// <param name="invoiceStatusId">Id of requested invoice status</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>InvoiceStatusModel</returns>
        [HttpGet("invoiceStatus/{invoiceStatusId}")]
        [Authorize]
        [ProducesResponseType(typeof(GetInvoiceStatusDetailsResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetInvoiceStatus(int invoiceStatusId, CancellationToken cancellationToken)
        {
            var result = await _invoiceService.GetInvoiceStatusByIdAsync(invoiceStatusId, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get InvoiceStatus by logged user id
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>InvoiceStatusModel</returns>
        [HttpGet("invoiceStatus")]
        [Authorize]
        [ProducesResponseType(typeof(GetInvoiceStatusDetailsResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetInvoiceStatus(CancellationToken cancellationToken)
        {
            var result = await _invoiceService.GetInvoiceStatusByLoggedUserAsync(cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Change InvoiceStatus, log the change, send notifing mail and add MailHistory
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>InvoiceStatusModel</returns>
        [HttpPut("invoiceStatus")]
        [Authorize]
        [ProducesResponseType(typeof(LogInvoiceStatusResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeInvoiceStatus(CancellationToken cancellationToken)
        {
            var result = await _invoiceService.ChangeInvoiceStatusAsync(cancellationToken);
            var mailResponse = await _mailService.SendEmailNotificationAsync(_mapper.Map<MailNotificationRequest>(result), cancellationToken);

            var request = _mapper.Map<AddMailHistoryRequest>(mailResponse);
            request.UserId = result.UserId;

            await _mailHistoryService.AddMailHistoryAsync(request, cancellationToken);
            var logResult = await _logService.LogInvoiceStatusChangeAsync(_mapper.Map<LogInvoiceStatusChangeRequest>(result), cancellationToken);

            return Ok(logResult);
        }

        /// <summary>
        /// Add InvoiceStatus, log the add, send notifing mail and add MailHistory
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>InvoiceStatusModel</returns>
        [HttpPost("invoiceStatus")]
        [Authorize]
        [ProducesResponseType(typeof(LogInvoiceStatusResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddInvoiceStatus(CancellationToken cancellationToken)
        {
            var result = await _invoiceService.AddInvoiceStatusAsync(cancellationToken);
            var mailResponse = await _mailService.SendEmailNotificationAsync(_mapper.Map<MailNotificationRequest>(result), cancellationToken);

            var request = _mapper.Map<AddMailHistoryRequest>(mailResponse);
            request.UserId = result.UserId;

            await _mailHistoryService.AddMailHistoryAsync(request, cancellationToken);
            var logResult = await _logService.LogInvoiceStatusFirstActivationAsync(_mapper.Map<LogInvoiceStatusChangeRequest>(result), cancellationToken);

            return Ok(logResult);
        }
    }
}
