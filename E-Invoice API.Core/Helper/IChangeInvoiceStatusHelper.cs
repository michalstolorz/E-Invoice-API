using E_Invoice_API.Common.Enums;

namespace E_Invoice_API.Core.Helper
{
    public interface IChangeInvoiceStatusHelper
    {
        EnumInvoiceStatus ChangeInvoiceStatus(EnumInvoiceStatus invoiceStatus);
    }
}
