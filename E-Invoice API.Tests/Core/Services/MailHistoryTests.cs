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
    public class MailHistoryTests : BaseServiceTests
    {
        private readonly Mock<IMailHistoryRepository> _mailHistoryRepositoryMock;
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
        public MailHistoryTests()
        {
            _mailHistoryRepositoryMock = CreateMailHistoryRepositoryMock();
            _dateTimeProviderMock = CreateDateTimeProvider();
        }

        #region AddMailHistoryAsync
        /// <summary>
        /// Method: AddMailHistoryAsync
        /// Data: Valid params
        /// Return: AddAsync() invoke
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddMailHistoryAsync_ValidParams_Invoke_AddAsync()
        {
            //Arrange
            var departmentService = CreateMailHistoryService();
            var request = new AddMailHistoryRequest() { UserId = 1, MailSendToEmail = "jtracz@gmail.com", EmailTemplate = "NotificationTemplate" };
            var ct = new CancellationToken();

            //Act
            await departmentService.AddMailHistoryAsync(request, ct);

            //Assert
            _mailHistoryRepositoryMock.Verify(x => x.AddAsync(It.IsAny<MailHistory>(), ct), Times.Once);
        }

        /// <summary>
        /// Method: AddMailHistoryAsync
        /// Data: Invalid params
        /// Return: Throws ValidatorException
        /// </summary>
        /// <returns></returns>
        [Theory]
        [ClassData(typeof(AddMailHistoryValidParamsData))]
        public async Task AddMailHistoryAsync_InvalidParams_ThrowsValidatorException(int userId, string mailSendToEmail, string emailTemplate)
        {
            //Arrange
            var service = CreateMailHistoryService();
            var request = new AddMailHistoryRequest() { UserId = userId, MailSendToEmail = mailSendToEmail, EmailTemplate = emailTemplate };
            var ct = new CancellationToken();

            //Act => Assert
            await Assert.ThrowsAsync<ValidatorException>(async () => await service.AddMailHistoryAsync(request, ct));
        }
        #endregion

        #region Private Helpers
        private Mock<IMailHistoryRepository> CreateMailHistoryRepositoryMock()
        {
            var list = new List<MailHistory>
            {
                new MailHistory { Id = 1, UserId = 1, MailSendToEmail = "jtracz@gmail.com", SendMailDateTime = DateTime.Parse("01/01/2020 15:00"), EmailTemplate = "NotificationTemplate"},
                new MailHistory { Id = 2, UserId = 2, MailSendToEmail = "rlewandowski@gmail.com", SendMailDateTime = DateTime.Parse("02/01/2020 17:00"), EmailTemplate = "NotificationTemplate"},
                new MailHistory { Id = 3, UserId = 3, MailSendToEmail = "ddario@gmail.com", SendMailDateTime = DateTime.Parse("03/01/2020 13:00"), EmailTemplate = "NotificationTemplate"},
            };

            var mock = CreateMockedGenericRepositoryFromList<IMailHistoryRepository, MailHistory>(list);

            return mock;
        }

        private Mock<IDateTimeProvider> CreateDateTimeProvider()
        {
            var mock = new Mock<IDateTimeProvider>();
            mock.Setup(x => x.GetDateTimeNow()).Returns(DateTime.Parse("01/01/2020 15:00"));

            return mock;
        }

        private MailHistoryService CreateMailHistoryService()
        {
            return new MailHistoryService(_mailHistoryRepositoryMock.Object, _dateTimeProviderMock.Object);
        }
        #endregion

        #region Test data
        public class AddMailHistoryValidParamsData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { -2, "jtracz@gmail.com", "NotificationTemplate" };
                yield return new object[] { 1, "", "NotificationTemplate" };
                yield return new object[] { 2, null, "NotificationTemplate" };
                yield return new object[] { 3, "jtracz", "NotificationTemplate" };
                yield return new object[] { 4, "jtraczgmail.com", "NotificationTemplate" };
                yield return new object[] { 5, "jtracz@gmail.com", "" };
                yield return new object[] { 6, "jtracz@gmail.com", null };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #endregion
    }
}
