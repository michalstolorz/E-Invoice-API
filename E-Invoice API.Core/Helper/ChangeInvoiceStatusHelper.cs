using E_Invoice_API.Common.Enums;
using E_Invoice_API.Core.Exceptions;

namespace E_Invoice_API.Core.Helper
{
    public class ChangeInvoiceStatusHelper : IChangeInvoiceStatusHelper
    {
        public EnumInvoiceStatus ChangeInvoiceStatus(EnumInvoiceStatus invoiceStatus)
        {
            switch (invoiceStatus)
            {
                case EnumInvoiceStatus.Active:
                    return EnumInvoiceStatus.Inactive;
                case EnumInvoiceStatus.Inactive:
                    return EnumInvoiceStatus.Active;
                case EnumInvoiceStatus.FirstActivation:
                    return EnumInvoiceStatus.Inactive;
                case EnumInvoiceStatus.Nonexist:
                    return EnumInvoiceStatus.FirstActivation;
                default:
                    throw new ServiceException(ErrorCodes.NoConversionForGivenStatus, $"No conversion for given status {invoiceStatus}");
            }
        }
    }
}
