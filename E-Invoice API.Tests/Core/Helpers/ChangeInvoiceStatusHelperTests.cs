using E_Invoice_API.Common.Enums;
using E_Invoice_API.Core.Exceptions;
using E_Invoice_API.Core.Helper;
using Xunit;

namespace E_Invoice_API.Tests.Core.Helpers
{
    public class ChangeInvoiceStatusHelperTests
    {
        #region ChangeInvoiceStatus
        /// <summary>
        /// Method: ChangeInvoiceStatus
        /// Data: Correct input enum
        /// Return: Correct output enum
        /// </summary>
        [Theory]
        [InlineData(EnumInvoiceStatus.Active, EnumInvoiceStatus.Inactive)]
        [InlineData(EnumInvoiceStatus.Inactive, EnumInvoiceStatus.Active)]
        [InlineData(EnumInvoiceStatus.FirstActivation, EnumInvoiceStatus.Inactive)]
        [InlineData(EnumInvoiceStatus.Nonexist, EnumInvoiceStatus.FirstActivation)]
        public void ChangeInvoiceStatus_CorrectInput_CorrectOutput(EnumInvoiceStatus inputEnum, EnumInvoiceStatus expectedEnum)
        {
            //Arrange 
            var helper = CreateChangeInvoiceStatusHelper();
            //Act
            var changedEnum = helper.ChangeInvoiceStatus(inputEnum);
            //Assert
            Assert.Equal(changedEnum, expectedEnum);
        }

        /// <summary>
        /// Method: ChangeInvoiceStatus
        /// Data: Invalid value of enum EnumInvoiceStatus
        /// Return: Throws ServiceException
        /// </summary>
        /// <returns></returns>
        [Theory]
        [InlineData(5)]
        [InlineData(17)]
        public void ChangeInvoiceStatus_InvalidValue_ThrowsServiceException(int invalidEnumValue)
        {
            // Arrange
            var helper = CreateChangeInvoiceStatusHelper();
            // Act => Assert
            Assert.Throws<ServiceException>(() => helper.ChangeInvoiceStatus((EnumInvoiceStatus)invalidEnumValue));
        }
        #endregion

        #region PrivateHelpers
        private ChangeInvoiceStatusHelper CreateChangeInvoiceStatusHelper()
        {
            return new ChangeInvoiceStatusHelper();
        }
        #endregion
    }
}
