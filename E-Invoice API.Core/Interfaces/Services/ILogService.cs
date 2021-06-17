using E_Invoice_API.Core.DTO.Request;
using E_Invoice_API.Core.DTO.Response;
using System.Threading;
using System.Threading.Tasks;

namespace E_Invoice_API.Core.Interfaces.Services
{
    public interface ILogService
    {
        Task<LogInvoiceStatusResponse> LogInvoiceStatusFirstActivationAsync(LogInvoiceStatusChangeRequest request, CancellationToken cancellationToken);
        Task<LogInvoiceStatusResponse> LogInvoiceStatusChangeAsync(LogInvoiceStatusChangeRequest request, CancellationToken cancellationToken);
    }
}
