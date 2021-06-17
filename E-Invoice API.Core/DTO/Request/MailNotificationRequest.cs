using E_Invoice_API.Common.Enums;

namespace E_Invoice_API.Core.DTO.Request
{
    public class MailNotificationRequest
    {
        public string ToEmail { get; set; }
        public string UserName { get; set; }
        public EnumInvoiceStatus CurrentStatus { get; set; }
    }
}
