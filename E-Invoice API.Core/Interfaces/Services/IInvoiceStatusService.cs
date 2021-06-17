using E_Invoice_API.Core.DTO.Response;
using E_Invoice_API.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace E_Invoice_API.Core.Interfaces.Services
{
    public interface IInvoiceStatusService
    {
        Task<GetInvoiceStatusDetailsResponse> GetInvoiceStatusByIdAsync(int invoiceStatusId, CancellationToken cancellationToken);
        Task<GetInvoiceStatusDetailsResponse> GetInvoiceStatusByLoggedUserAsync(CancellationToken cancellationToken);
        Task<InvoiceStatusModel> AddInvoiceStatusAsync(CancellationToken cancellationToken);
        Task<InvoiceStatusModel> ChangeInvoiceStatusAsync(CancellationToken cancellationToken);
    }
}
