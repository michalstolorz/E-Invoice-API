using E_Invoice_API.Common.Enums;
using E_Invoice_API.Core.Helper;
using E_Invoice_API.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace E_Invoice_API.Core.DTO.Response
{
    public class GetInvoiceStatusDetailsResponse
    {
        public int InvoiceStatusId { get; set; }
        public int UserId { get; set; }
        public UserModel UserModel { get; set; }
        [JsonConverter(typeof(EnumConverterToString))]
        public EnumInvoiceStatus Status { get; set; }
        public ICollection<InvoiceStatusLog> UserInvoiceStatusLogs { get; set; }
    }

    public class InvoiceStatusLog
    {
        public int InvoiceStatusLogId { get; set; }
        public DateTime ChangeDateTime { get; set; }
        public DateTime? ToCheckFirstActivationDateTime { get; set; }
        [JsonConverter(typeof(EnumConverterToString))]
        public EnumInvoiceStatus PreviousStatus { get; set; }
        [JsonConverter(typeof(EnumConverterToString))]
        public EnumInvoiceStatus CurrentStatus { get; set; }
    }
}
