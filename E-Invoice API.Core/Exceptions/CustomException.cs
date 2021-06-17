using System;

namespace E_Invoice_API.Core.Exceptions
{
    public abstract class CustomException : Exception
    {
        public string Code { get; }

        protected CustomException()
        {
        }

        public CustomException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}
