using E_Invoice_API.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace E_Invoice_API.Core.Helper
{
    public class EnumConverterToString : JsonConverter<EnumInvoiceStatus>
    {
        public override EnumInvoiceStatus Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
                (EnumInvoiceStatus)Enum.Parse(typeToConvert, reader.GetString());

        public override void Write(
            Utf8JsonWriter writer,
            EnumInvoiceStatus enumValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(enumValue.ToString());
    }
}
