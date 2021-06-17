using E_Invoice_API.Core.DTO.Request;
using E_Invoice_API.Core.DTO.Response;
using System.Threading;
using System.Threading.Tasks;

namespace E_Invoice_API.Core.Interfaces.Services
{
    public interface IMailHistoryService
    {
        Task AddMailHistoryAsync(AddMailHistoryRequest request, CancellationToken cancellationToken);
    }
}
