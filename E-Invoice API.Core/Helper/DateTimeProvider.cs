using System;

namespace E_Invoice_API.Core.Helper
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetDateTimeNow()
        {
            return DateTime.Now;
        }
    }
}
