using E_Invoice_API.Common.Enums;
using E_Invoice_API.Core.Models;
using System.Collections.Generic;

namespace E_Invoice_API.Core.DTO.Request
{
    public class LogInvoiceStatusChangeRequest
    {
        public int InvoiceStatusId { get; set; }
        public UserModel UserModel { get; set; }
        public EnumInvoiceStatus Status { get; set; }
        public virtual ICollection<LogModel> UserLogsModel { get; set; }
    }
}
