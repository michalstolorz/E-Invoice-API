using System.Security.Claims;

namespace E_Invoice_API.Core.Helper
{
    public interface IUserContextProvider
    {
        ClaimsPrincipal User { get; }
        int? UserId { get; }
        string UserName { get; }
        bool Authenticated { get; }
    }
}
