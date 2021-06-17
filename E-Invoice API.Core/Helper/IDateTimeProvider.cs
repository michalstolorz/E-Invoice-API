using System;

namespace E_Invoice_API.Core.Helper
{
    public interface IDateTimeProvider
    {
        DateTime GetDateTimeNow();
    }
}
