
namespace E_Invoice_API.Core.Exceptions
{
    public class ControllerException : CustomException
    {
        protected ControllerException()
        {
        }

        public ControllerException(string code, string message, params object[] args)
            : base(null, code, message, args)
        {
        }
    }
}
