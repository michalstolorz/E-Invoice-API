using E_Invoice_API.Common.Enums;
using System;

namespace E_Invoice_API.Data.Models
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime ChangeDateTime { get; set; }
        public DateTime? ToCheckFirstActivationDateTime { get; set; }
        public EnumInvoiceStatus PreviousStatus { get; set; }
        public EnumInvoiceStatus CurrentStatus { get; set; }
        public int InvoiceStatusId { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; }
    }
}
