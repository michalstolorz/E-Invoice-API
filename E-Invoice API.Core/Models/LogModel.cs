using E_Invoice_API.Common.Enums;
using E_Invoice_API.Core.Helper;
using System;
using System.Text.Json.Serialization;

namespace E_Invoice_API.Core.Models
{
    public class LogModel
    {
        public int Id { get; set; }
        public DateTime ChangeDateTime { get; set; }
        public DateTime? ToCheckFirstActivationDateTime { get; set; }
        [JsonConverter(typeof(EnumConverterToString))]
        public EnumInvoiceStatus PreviousStatus { get; set; }
        [JsonConverter(typeof(EnumConverterToString))]
        public EnumInvoiceStatus CurrentStatus { get; set; }
        public int InvoiceStatusId { get; set; }
        public InvoiceStatusModel InvoiceStatusModel { get; set; }
    }
}
