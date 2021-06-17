
namespace E_Invoice_API.Core.Exceptions
{
    public class RepositoryException : CustomException
    {
        protected RepositoryException()
        {
        }

        public RepositoryException(string code, string message, params object[] args)
            : base(null, code, message, args)
        {
        }
    }
}
