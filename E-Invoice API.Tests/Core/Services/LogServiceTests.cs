using AutoMapper;
using E_Invoice_API.Common.Enums;
using E_Invoice_API.Core.DTO.Request;
using E_Invoice_API.Core.Exceptions;
using E_Invoice_API.Core.Helper;
using E_Invoice_API.Core.Interfaces.Repositories;
using E_Invoice_API.Core.Models;
using E_Invoice_API.Core.Services;
using E_Invoice_API.Data.Models;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace E_Invoice_API.Tests.Core.Services
{
    public class LogServiceTests : BaseServiceTests
    {
        private readonly Mock<ILogRepository> _logRepositoryMock;
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
        private readonly IMapper _mapper;
        public LogServiceTests()
        {
            _mapper = CreateMapper();
            _logRepositoryMock = CreateLogRepositoryMock();
            _dateTimeProviderMock = CreateDateTimeProvider();
        }

        #region LogInvoiceStatusFirstActivationAsync
        /// <summary>
        /// Method: LogInvoiceStatusFirstActivationAsync
        /// Data: Valid params
        /// Return: AddAsync() invoke
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task LogInvoiceStatusFirstActivationAsync_ValidParams_Invoke_AddAsync()
        {
            //Arrange
            var departmentService = CreateLogService();
            var request = new LogInvoiceStatusChangeRequest() { InvoiceStatusId = 1, Status = EnumInvoiceStatus.Active };
            var ct = new CancellationToken();

            //Act
            await departmentService.LogInvoiceStatusFirstActivationAsync(request, ct);

            //Assert
            _logRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Log>(), ct), Times.Once);
        }

        /// <summary>
        /// Method: LogInvoiceStatusFirstActivationAsync
        /// Data: Invalid params
        /// Return: Throws ValidatorException
        /// </summary>
        /// <returns></returns>
        [Theory]
        [InlineData(1, (EnumInvoiceStatus)5)]
        [InlineData(0, (EnumInvoiceStatus)1)]
        public async Task LogInvoiceStatusFirstActivationAsync_InvalidParams_ThrowsValidatorException(int invoiceStatusId, EnumInvoiceStatus enumStatus)
        {
            //Arrange
            var service = CreateLogService();
            var request = new LogInvoiceStatusChangeRequest() { InvoiceStatusId = invoiceStatusId, Status = enumStatus };
            var ct = new CancellationToken();

            //Act => Assert
            await Assert.ThrowsAsync<ValidatorException>(async () => await service.LogInvoiceStatusFirstActivationAsync(request, ct));
        }
        #endregion

        #region LogInvoiceStatusChangeAsync
        /// <summary>
        /// Method: LogInvoiceStatusChangeAsync
        /// Data: Valid params
        /// Return: AddAsync() invoke
        /// </summary>
        /// <returns></returns>
        [Theory]
        [ClassData(typeof(LogInvoiceStatusChangeValidParamsData))]
        public async Task LogInvoiceStatusChangeAsync_ValidParams_Invoke_AddAsync(int invoiceStatusId, EnumInvoiceStatus enumInvoiceStatus, List<LogModel> userLogsModel)
        {
            //Arrange
            var departmentService = CreateLogService();
            var request = new LogInvoiceStatusChangeRequest()
            {
                InvoiceStatusId = invoiceStatusId,
                Status = enumInvoiceStatus,
                UserLogsModel = userLogsModel
            };
            var ct = new CancellationToken();

            //Act
            await departmentService.LogInvoiceStatusChangeAsync(request, ct);

            //Assert
            _logRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Log>(), ct), Times.Once);
        }

        /// <summary>
        /// Method: LogInvoiceStatusChangeAsync
        /// Data: Invalid params
        /// Return: Throws ValidatorException
        /// </summary>
        /// <returns></returns>
        [Theory]
        [InlineData(1, (EnumInvoiceStatus)5)]
        [InlineData(0, (EnumInvoiceStatus)1)]
        public async Task LogInvoiceStatusChangeAsync_InvalidParams_ThrowsValidatorException(int invoiceStatusId, EnumInvoiceStatus enumStatus)
        {
            //Arrange
            var service = CreateLogService();
            var request = new LogInvoiceStatusChangeRequest() { InvoiceStatusId = invoiceStatusId, Status = enumStatus };
            var ct = new CancellationToken();

            //Act => Assert
            await Assert.ThrowsAsync<ValidatorException>(async () => await service.LogInvoiceStatusChangeAsync(request, ct));
        }
        #endregion

        #region Private Helpers
        private Mock<ILogRepository> CreateLogRepositoryMock()
        {
            var list = new List<Log>
            {
                new Log { Id = 1, ChangeDateTime = DateTime.Parse("01/01/2020 15:00"), ToCheckFirstActivationDateTime = DateTime.Parse("01/01/2020 15:00"), PreviousStatus = EnumInvoiceStatus.Nonexist, CurrentStatus = EnumInvoiceStatus.FirstActivation, InvoiceStatusId = 1},
                new Log { Id = 2, ChangeDateTime = DateTime.Parse("02/01/2020 13:00"), ToCheckFirstActivationDateTime = null, PreviousStatus = EnumInvoiceStatus.Active, CurrentStatus = EnumInvoiceStatus.Inactive, InvoiceStatusId = 1},
                new Log { Id = 3, ChangeDateTime = DateTime.Parse("03/01/2020 11:00"), ToCheckFirstActivationDateTime = null, PreviousStatus = EnumInvoiceStatus.Inactive, CurrentStatus = EnumInvoiceStatus.Active, InvoiceStatusId = 1}
            };

            var mock = CreateMockedGenericRepositoryFromList<ILogRepository, Log>(list);

            return mock;
        }

        private Mock<IDateTimeProvider> CreateDateTimeProvider()
        {
            var mock = new Mock<IDateTimeProvider>();
            mock.Setup(x => x.GetDateTimeNow()).Returns(DateTime.Parse("01/01/2020 15:00"));

            return mock;
        }

        private LogService CreateLogService()
        {
            return new LogService(_logRepositoryMock.Object, _mapper, _dateTimeProviderMock.Object, new ChangeInvoiceStatusHelper());
        }
        #endregion

        #region Test data
        public class LogInvoiceStatusChangeValidParamsData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { 1, EnumInvoiceStatus.Active, new List<LogModel>() { new LogModel { Id = 1, ChangeDateTime = DateTime.Parse("01/01/2020 15:00"), ToCheckFirstActivationDateTime = DateTime.Parse("01/01/2020 15:00"), PreviousStatus = EnumInvoiceStatus.Nonexist, CurrentStatus = EnumInvoiceStatus.FirstActivation, InvoiceStatusId = 1 } } };
                yield return new object[] { 2, EnumInvoiceStatus.Inactive, new List<LogModel>() {
                  new LogModel { Id = 1, ChangeDateTime = DateTime.Parse("01/01/2020 15:00"), ToCheckFirstActivationDateTime = DateTime.Parse("01/01/2020 15:00"), PreviousStatus = EnumInvoiceStatus.Nonexist, CurrentStatus = EnumInvoiceStatus.FirstActivation, InvoiceStatusId = 1 },
                  new LogModel { Id = 2, ChangeDateTime = DateTime.Parse("02/01/2020 17:00"), ToCheckFirstActivationDateTime = null, PreviousStatus = EnumInvoiceStatus.FirstActivation, CurrentStatus = EnumInvoiceStatus.Inactive, InvoiceStatusId = 1 }} };

            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #endregion
    }
}
