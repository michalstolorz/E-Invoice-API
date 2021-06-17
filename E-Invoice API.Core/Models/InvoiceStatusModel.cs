using E_Invoice_API.Common.Enums;
using E_Invoice_API.Core.Helper;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace E_Invoice_API.Core.Models
{
    public class InvoiceStatusModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserModel UserModel { get; set; }
        [JsonConverter(typeof(EnumConverterToString))]
        public EnumInvoiceStatus Status { get; set; }
        public virtual ICollection<LogModel> UserLogsModel { get; set; }
    }
}
