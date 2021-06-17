using AutoMapper;
using E_Invoice_API.Common.Enums;
using E_Invoice_API.Core.Exceptions;
using E_Invoice_API.Core.Helper;
using E_Invoice_API.Core.Interfaces.Repositories;
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
    public class InvoiceStatusServiceTests : BaseServiceTests
    {
        private readonly Mock<IInvoiceStatusRepository> _invoiceStatusRepositoryMock;
        private readonly Mock<IUserContextProvider> _correctUserContextProviderMock;
        private readonly Mock<IUserContextProvider> _invalidUserContextProviderMock;
        private readonly Mock<IUserContextProvider> _noExistingUserContextProviderMock;
        private readonly IMapper _mapper;
        public InvoiceStatusServiceTests()
        {
            _mapper = CreateMapper();
            _invoiceStatusRepositoryMock = CreateInvoiceStatusRepositoryMock();
            _correctUserContextProviderMock = CreateUserContextProviderMock(1);
            _invalidUserContextProviderMock = CreateUserContextProviderMock(0);
            _noExistingUserContextProviderMock = CreateUserContextProviderMock(10);
        }

        #region GetInvoiceStatusByIdAsync
        /// <summary>
        /// Method: GetInvoiceStatusByIdAsync
        /// Data: Valid Invoice status id
        /// Return: GetByIdAsync() invoke
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetInvoiceStatusByIdAsync_ValidParams_Invoke_GetByIdAsync()
        {
            //Arrange
            var service = CreateInvoiceStatusService();
            var ct = new CancellationToken();

            //Act
            await service.GetInvoiceStatusByIdAsync(1, ct);

            //Assert
            _invoiceStatusRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>(), It.IsAny<Func<IQueryable<InvoiceStatus>, IIncludableQueryable<InvoiceStatus, object>>>()), Times.Once);
        }

        /// <summary>
        /// Method: GetInvoiceStatusByIdAsync
        /// Data: Invoice status id of no existing invoice status
        /// Return: Throws ServiceException
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetInvoiceStatusByIdAsync_NoExistingInvoiceStatusId_ThrowsServiceException()
        {
            //Arrange
            var service = CreateInvoiceStatusService();
            var ct = new CancellationToken();

            //Act => Assert
            await Assert.ThrowsAsync<ServiceException>(async () => await service.GetInvoiceStatusByIdAsync(9, ct));
        }

        /// <summary>
        /// Method: GetInvoiceStatusByIdAsync
        /// Data: Invalid invoice status id
        /// Return: Throws ValidatorException
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetInvoiceStatusByIdAsync_InvalidId_ThrowsValidatorException()
        {
            //Arrange
            var service = CreateInvoiceStatusService();
            var ct = new CancellationToken();

            //Act => Assert
            await Assert.ThrowsAsync<ValidatorException>(async () => await service.GetInvoiceStatusByIdAsync(-1, ct));
        }
        #endregion

        #region GetInvoiceStatusByLoggedUserAsync
        /// <summary>
        /// Method: GetInvoiceStatusByLoggedUserAsync
        /// Data: Valid params
        /// Return: GetSingleAsync() invoke
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetInvoiceStatusByLoggedUserAsync_ValidParams_Invoke_GetSingleAsync()
        {
            //Arrange
            var service = CreateInvoiceStatusService();
            var ct = new CancellationToken();

            //Act
            await service.GetInvoiceStatusByLoggedUserAsync(ct);

            //Assert
            _invoiceStatusRepositoryMock.Verify(x => x.GetSingleAsync(It.IsAny<Expression<Func<InvoiceStatus, bool>>>(), It.IsAny<CancellationToken>(), It.IsAny<Func<IQueryable<InvoiceStatus>, IIncludableQueryable<InvoiceStatus, object>>>()), Times.Once);
        }

        /// <summary>
        /// Method: GetInvoiceStatusByLoggedUserAsync
        /// Data: User id of no existing user
        /// Return: Throws ServiceException
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetInvoiceStatusByLoggedUserAsync_NoExistUserId_ThrowsServiceException()
        {
            //Arrange
            var service = CreateInvoiceStatusServiceNoExistingUserId();
            var ct = new CancellationToken();

            //Act => Assert
            await Assert.ThrowsAsync<ServiceException>(async () => await service.GetInvoiceStatusByLoggedUserAsync(ct));
        }

        /// <summary>
        /// Method: GetInvoiceStatusByLoggedUserAsync
        /// Data: Invalid user id
        /// Return: Throws ValidatorException
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetInvoiceStatusByLoggedUserAsync_InvalidUserId_ThrowsValidatorException()
        {
            //Arrange
            var service = CreateInvoiceStatusServiceInvalidUserId();
            var ct = new CancellationToken();

            //Act => Assert
            await Assert.ThrowsAsync<ValidatorException>(async () => await service.GetInvoiceStatusByLoggedUserAsync(ct));
        }
        #endregion

        #region AddInvoiceStatusAsync
        /// <summary>
        /// Method: AddInvoiceStatusAsync
        /// Data:
        /// Return: AddAsync() AnyAsync() invoke
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddInvoiceStatusAsync_Invoke_AddAsync_AnyAsync()
        {
            //Arrange
            var service = CreateInvoiceStatusServiceNoExistingUserId();
            var ct = new CancellationToken();

            //Act
            await service.AddInvoiceStatusAsync(ct);

            //Assert
            _invoiceStatusRepositoryMock.Verify(x => x.AddAsync(It.IsAny<InvoiceStatus>(), It.IsAny<CancellationToken>()), Times.Once);
            _invoiceStatusRepositoryMock.Verify(x => x.AnyAsync(It.IsAny<Expression<Func<InvoiceStatus, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Method: AddInvoiceStatusAsync
        /// Data: User id of already existing user
        /// Return: Throws ServiceException
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddInvoiceStatusAsync_AlreadyExistUserId_ThrowsServiceException()
        {
            //Arrange
            var service = CreateInvoiceStatusService();
            var ct = new CancellationToken();

            //Act => Assert
            await Assert.ThrowsAsync<ServiceException>(async () => await service.AddInvoiceStatusAsync(ct));
        }

        /// <summary>
        /// Method: AddInvoiceStatusAsync
        /// Data: Invalid user id
        /// Return: Throws ValidatorException
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddInvoiceStatusAsync_InvalidUserId_ThrowsValidatorException()
        {
            //Arrange
            var service = CreateInvoiceStatusServiceInvalidUserId();
            var ct = new CancellationToken();

            //Act => Assert
            await Assert.ThrowsAsync<ValidatorException>(async () => await service.AddInvoiceStatusAsync(ct));
        }
        #endregion

        #region ChangeInvoiceStatusAsync
        /// <summary>
        /// Method: ChangeInvoiceStatusAsync
        /// Data:
        /// Return: UpdateAsync() GetSingleAsync() invoke
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ChangeInvoiceStatusAsync_Invoke_UpdateAsync_GetSingleAsync()
        {
            //Arrange
            var service = CreateInvoiceStatusService();
            var ct = new CancellationToken();

            //Act
            await service.ChangeInvoiceStatusAsync(ct);

            //Assert
            _invoiceStatusRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<InvoiceStatus>(), It.IsAny<CancellationToken>(), false), Times.Once);
            _invoiceStatusRepositoryMock.Verify(x => x.GetSingleAsync(It.IsAny<Expression<Func<InvoiceStatus, bool>>>(), It.IsAny<CancellationToken>(), It.IsAny<Func<IQueryable<InvoiceStatus>, IIncludableQueryable<InvoiceStatus, object>>>()), Times.Once);
        }

        /// <summary>
        /// Method: ChangeInvoiceStatusAsync
        /// Data: User id of no existing user
        /// Return: Throws ServiceException
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ChangeInvoiceStatusAsync_NoExistUserId_ThrowsServiceException()
        {
            //Arrange
            var service = CreateInvoiceStatusServiceNoExistingUserId();
            var ct = new CancellationToken();

            //Act => Assert
            await Assert.ThrowsAsync<ServiceException>(async () => await service.ChangeInvoiceStatusAsync(ct));
        }

        /// <summary>
        /// Method: ChangeInvoiceStatusAsync
        /// Data: Invalid user id
        /// Return: Throws ValidatorException
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ChangeInvoiceStatusAsync_InvalidUserId_ThrowsValidatorException()
        {
            //Arrange
            var service = CreateInvoiceStatusServiceInvalidUserId();
            var ct = new CancellationToken();

            //Act => Assert
            await Assert.ThrowsAsync<ValidatorException>(async () => await service.ChangeInvoiceStatusAsync(ct));
        }
        #endregion

        #region Private Helpers
        private Mock<IInvoiceStatusRepository> CreateInvoiceStatusRepositoryMock()
        {
            var list = new List<InvoiceStatus>
            {
                new InvoiceStatus { Id = 1, Status = (EnumInvoiceStatus)1, UserId = 1},
                new InvoiceStatus { Id = 2, Status = (EnumInvoiceStatus)2, UserId = 2},
                new InvoiceStatus { Id = 3, Status = (EnumInvoiceStatus)3, UserId = 3},
                new InvoiceStatus { Id = 4, Status = (EnumInvoiceStatus)4, UserId = 4},
            };

            var mock = CreateMockedGenericRepositoryFromList<IInvoiceStatusRepository, InvoiceStatus>(list);

            mock
            .Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>(),
                It.IsAny<Func<IQueryable<InvoiceStatus>, IIncludableQueryable<InvoiceStatus, object>>>()))
            .Returns(new Func<int, CancellationToken, Func<IQueryable<InvoiceStatus>, IIncludableQueryable<InvoiceStatus, object>>, Task<InvoiceStatus>>
                    (async (k, c, i) => await Task.FromResult(list.SingleOrDefault(x => x.Id == k))));

            mock
            .Setup(x => x.AddAsync(It.IsAny<InvoiceStatus>(), It.IsAny<CancellationToken>()))
            .Returns(new Func<InvoiceStatus, CancellationToken, Task<InvoiceStatus>>
                    (async (i, c) => await Task.FromResult(i)));

            return mock;
        }

        private Mock<IUserContextProvider> CreateUserContextProviderMock(int fakeUserId)
        {
            var mock = new Mock<IUserContextProvider>();

            mock.Setup(x => x.UserId).Returns(fakeUserId);

            return mock;
        }

        private InvoiceStatusService CreateInvoiceStatusService()
        {
            return new InvoiceStatusService(_invoiceStatusRepositoryMock.Object, _mapper, _correctUserContextProviderMock.Object, new ChangeInvoiceStatusHelper());
        }

        private InvoiceStatusService CreateInvoiceStatusServiceInvalidUserId()
        {
            return new InvoiceStatusService(_invoiceStatusRepositoryMock.Object, _mapper, _invalidUserContextProviderMock.Object, new ChangeInvoiceStatusHelper());
        }

        private InvoiceStatusService CreateInvoiceStatusServiceNoExistingUserId()
        {
            return new InvoiceStatusService(_invoiceStatusRepositoryMock.Object, _mapper, _noExistingUserContextProviderMock.Object, new ChangeInvoiceStatusHelper());
        }
        #endregion
    }
}
