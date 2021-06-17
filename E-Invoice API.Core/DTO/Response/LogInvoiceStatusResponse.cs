using E_Invoice_API.Common.Enums;
using E_Invoice_API.Core.Helper;
using System;
using System.Text.Json.Serialization;

namespace E_Invoice_API.Core.DTO.Response
{
    public class LogInvoiceStatusResponse
    {
        public int LogId { get; set; }
        public DateTime ChangeDateTime { get; set; }
        public DateTime? ToCheckFirstActivationDateTime { get; set; }
        [JsonConverter(typeof(EnumConverterToString))]
        public EnumInvoiceStatus PreviousStatus { get; set; }
        [JsonConverter(typeof(EnumConverterToString))]
        public EnumInvoiceStatus CurrentStatus { get; set; }
        public int InvoiceStatusId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
