
namespace E_Invoice_API.Core.Exceptions
{
    public class ServiceException : CustomException
    {
        protected ServiceException()
        {
        }

        public ServiceException(string code, string message, params object[] args)
            : base(null, code, message, args)
        {
        }
    }
}
