using E_Invoice_API.Common.Enums;
using System.Collections.Generic;

namespace E_Invoice_API.Data.Models
{
    public class InvoiceStatus
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public EnumInvoiceStatus Status { get; set; }
        public virtual ICollection<Log> UserLogs { get; set; }
    }
}
