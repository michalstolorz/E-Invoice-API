
namespace E_Invoice_API.Core.Exceptions
{
    public class ValidatorException : CustomException
    {
        protected ValidatorException()
        {
        }

        public ValidatorException(string code, string message, params object[] args)
            : base(null, code, message, args)
        {
        }
    }
}
