using E_Invoice_API.Core.Helper;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Xunit;

namespace E_Invoice_API.Tests.Core.Helpers
{
    public class TokenHelperTests
    {
        private readonly Mock<RsaSecurityKey> _rsaKeyMock;
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

        public TokenHelperTests()
        {
            _rsaKeyMock = CreateRsaSecurityKeyMock();
            _dateTimeProviderMock = CreateDateTimeProviderMock();
        }

        #region BuildRsaSigningKey
        /// <summary>
        /// Method: BuildRsaSigningKey
        /// Data: Correct xml
        /// Return: Correct key
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void BuildRsaSigningKey_CorrectXml_CorrectKey()
        {
            // Arrange
            var xml =
                "<RSAKeyValue>" +
                "<Modulus>yrjwRCgJl/QFxEz3/5JP4mGGtcIoCd2wRUowSNDJQtbXZhlTkWRr42eZCBIvBQFnBCaOc1Nbn7xqyD3D5Y6iTiF2zUjsiL2ig2orMoV9x4qpoWWKzlV06lwRL8haX0JE7bHJ1lKAv+xtQBBvvKRIu6LS87izURwpvOUZdb2heMceRNJFabOtPjDfaGwrZL7hZ/ZH2YrxssV5CK8PgKTkoZ7O/4jltyULSvC/Y7H9pQy+XXWdTNkR1SOQp01mucadVdim9Y0sLVWXtu1Kw6hCz/jr352YY3G+26kAu/uOOyX/poh5wAoYz8c+nK8F7mixdvt6FQxUcrOrqshoWYH/0Q==</Modulus>" +
                "<Exponent>AQAB</Exponent>" +
                "<P>20EqVdJ0E3uGC9RrSyrYVGRreAQYq9ifkDoaDKA9YLxCIlaXf+f7YwuND+osKbrSrYvZnO2hr6OuthVZ+8rDV+kNUfDf2UwgsoxEKFSHrj6A+50I5yF/EURuJAMBpmtTCm3GhpnbVlA1jIHT8JY3mrk7F1WQryXCrftl5z1jd3s=</P>" +
                "<Q>7LJ7rlA8k4ErObpKL97UqXf+z9G0gCZc0QCH8dnK3C9nrUXSc4uLW6QASRG1B8khLYB0MOsdVUqkzDn36aksL7VhgFMIqI4huCw7DpLFPvMVsLTr2HINURyBd0DDgIAiLfr6QctBKbAwIXzJv5yhKyILv2zHxlUq+n7M/dIk3iM=</Q>" +
                "<DP>FUEG6pThHbZesyzfTcUUfXHSnSrAfYAdT6ziM5EhAgyd2JhOTV7elqZbAUzxBQaQP7SL3tOGVfFnEU2WkHsCXrY/zU6tVHI1xTklrkcrc++pLzr0zvsHR1Q73Q/RjCFhRmSH1yp5Aa/60OkQ84SMVVpZaRSpVuExMw5ovXbM+ps=</DP>" +
                "<DQ>r1iRs1KRbZyVDxDogIoM9PaF+CKcGwtQWyyPiSUU2QTtQzmkbCCGPn9CDt9lQr1HLNQqP0sN0e+YIgsXkyvWJgmyj0Pz+BpC9JMftO4Z3UZrXRrKVPA42UvKxTNfIUTLDVEOL8uJHH7/SO+O7bn002VHWqoVXOIfEq744VfaJA0=</DQ>" +
                "<InverseQ>KnUBukb1NTahctwIqCzbwTHi2lxlgZmJUk0nD8enote0dOxQGAOdMv0fVWuSMCYQlq4U+unC/XwOY+sdm/81w/ivYNUKz8AQGvm/iwm6S+ENVrXinQOlpizLoWLVyWzgcVCJsJ1U96nq+/hiOmdP7NBCb5rNoDCCsBKhz3tjN7w=</InverseQ>" +
                "<D>ia1yWAeNBGsRI5FWeHcI0+mCUJzNDm5GEbjh9AIAPemlHk0jCUJXV3j7YJTg5Bhgu1voMQCy4FhZeSchjR0Cs+dcRO06319TKMcJEWXB16wfqmJJE/rLzYK7lWUPo1RhdcDiIDGmCTJrvC+tg0NbtjtN44JUHkjvO7+oO+OO73OuPLC+oJi4PfGILmk5epA6poJtXB0dUQ3IogMTxhbMe/IpJEtXI3817e5jFSykBfJeJ7bO2L38dyFkLHK+CA2cTRWOAxufJY6HZ0ChMm3BRzDYkr/Sh9bwqNSSykLAhUm89wNROzzUJDvRuNhzDlAfJn9W0IsiacpMTkmsgagS5Q==</D>" +
                "</RSAKeyValue>";
            // Act
            var key = TokenHelper.BuildRsaSigningKey(xml);

            // Assert
            Assert.Equal(PrivateKeyStatus.Exists, key.PrivateKeyStatus);
            Assert.True(key.CryptoProviderFactory.CacheSignatureProviders);
            Assert.Null(key.KeyId);
            Assert.Equal(2048, key.KeySize);
            Assert.Equal("http://www.w3.org/2000/09/xmldsig#rsa-sha1", key.Rsa.SignatureAlgorithm);
        }

        /// <summary>
        /// Method: BuildRsaSigningKey
        /// Data: Wrong xml format
        /// Return: Throws CryptographicException
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void BuildRsaSigningKey_WrongXmlFormat_ThrowsCryptographicException()
        {
            // Arrange
            var xml =
                "<RSAKeyValue>" +
                "yrjwRCgJl/QFxEz3/5JP4mGGtcIoCd2wRUowSNDJQtbXZhlTkWRr42eZCBIvBQFnBCaOc1Nbn7xqyD3D5Y6iTiF2zUjsiL2ig2orMoV9x4qpoWWKzlV06lwRL8haX0JE7bHJ1lKAv+xtQBBvvKRIu6LS87izURwpvOUZdb2heMceRNJFabOtPjDfaGwrZL7hZ/ZH2YrxssV5CK8PgKTkoZ7O/4jltyULSvC/Y7H9pQy+XXWdTNkR1SOQp01mucadVdim9Y0sLVWXtu1Kw6hCz/jr352YY3G+26kAu/uOOyX/poh5wAoYz8c+nK8F7mixdvt6FQxUcrOrqshoWYH/0Q==</Modulus>" +
                "<Exponent>AQAB</Exponent>" +
                "<P>20EqVdJ0E3uGC9RrSyrYVGRreAQYq9ifkDoaDKA9YLxCIlaXf+f7YwuND+osKbrSrYvZnO2hr6OuthVZ+8rDV+kNUfDf2UwgsoxEKFSHrj6A+50I5yF/EURuJAMBpmtTCm3GhpnbVlA1jIHT8JY3mrk7F1WQryXCrftl5z1jd3s=</P>" +
                "<Q>7LJ7rlA8k4ErObpKL97UqXf+z9G0gCZc0QCH8dnK3C9nrUXSc4uLW6QASRG1B8khLYB0MOsdVUqkzDn36aksL7VhgFMIqI4huCw7DpLFPvMVsLTr2HINURyBd0DDgIAiLfr6QctBKbAwIXzJv5yhKyILv2zHxlUq+n7M/dIk3iM=</Q>" +
                "<DP>FUEG6pThHbZesyzfTcUUfXHSnSrAfYAdT6ziM5EhAgyd2JhOTV7elqZbAUzxBQaQP7SL3tOGVfFnEU2WkHsCXrY/zU6tVHI1xTklrkcrc++pLzr0zvsHR1Q73Q/RjCFhRmSH1yp5Aa/60OkQ84SMVVpZaRSpVuExMw5ovXbM+ps=</DP>" +
                "<DQ>r1iRs1KRbZyVDxDogIoM9PaF+CKcGwtQWyyPiSUU2QTtQzmkbCCGPn9CDt9lQr1HLNQqP0sN0e+YIgsXkyvWJgmyj0Pz+BpC9JMftO4Z3UZrXRrKVPA42UvKxTNfIUTLDVEOL8uJHH7/SO+O7bn002VHWqoVXOIfEq744VfaJA0=</DQ>" +
                "<InverseQ>KnUBukb1NTahctwIqCzbwTHi2lxlgZmJUk0nD8enote0dOxQGAOdMv0fVWuSMCYQlq4U+unC/XwOY+sdm/81w/ivYNUKz8AQGvm/iwm6S+ENVrXinQOlpizLoWLVyWzgcVCJsJ1U96nq+/hiOmdP7NBCb5rNoDCCsBKhz3tjN7w=</InverseQ>" +
                "<D>ia1yWAeNBGsRI5FWeHcI0+mCUJzNDm5GEbjh9AIAPemlHk0jCUJXV3j7YJTg5Bhgu1voMQCy4FhZeSchjR0Cs+dcRO06319TKMcJEWXB16wfqmJJE/rLzYK7lWUPo1RhdcDiIDGmCTJrvC+tg0NbtjtN44JUHkjvO7+oO+OO73OuPLC+oJi4PfGILmk5epA6poJtXB0dUQ3IogMTxhbMe/IpJEtXI3817e5jFSykBfJeJ7bO2L38dyFkLHK+CA2cTRWOAxufJY6HZ0ChMm3BRzDYkr/Sh9bwqNSSykLAhUm89wNROzzUJDvRuNhzDlAfJn9W0IsiacpMTkmsgagS5Q==</D>" +
                "</RSAKeyValue>";

            // Act => Assert
            Assert.Throws<CryptographicException>(() => TokenHelper.BuildRsaSigningKey(xml));
        }

        /// <summary>
        /// Method: BuildRsaSigningKey
        /// Data: Wrong input string length
        /// Return: Throws FormatException
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void BuildRsaSigningKey_WrongInputStringLength_ThrowsFormatException()
        {
            // Arrange
            var xml = "<RSAKeyValue>" +
                      "<Modulus>yrjwRCgJl/QFxEz3/5JP4mGGtIoCd2wRUowSNDJQtbXZhlTkWRr42eZCBIvBQFnBCaOc1Nbn7xqyD3D5Y6iTiF2zUjsiL2ig2orMoV9x4qpoWWKzlV06lwRL8haX0JE7bHJ1lKAv+xtQBBvvKRIu6LS87izURwpvOUZdb2heMceRNJFabOtPjDfaGwrZL7hZ/ZH2YrxssV5CK8PgKTkoZ7O/4jltyULSvC/Y7H9pQy+XXWdTNkR1SOQp01mucadVdim9Y0sLVWXtu1Kw6hCz/jr352YY3G+26kAu/uOOyX/poh5wAoYz8c+nK8F7mixdvt6FQxUcrOrqshoWYH/0Q==</Modulus>" +
                      "<Exponent>AQAB</Exponent>" +
                      "<P>20EqVdJ0E3uGC9RrSyrYVGRreAQYq9ifkDoaDKA9YLxCIlaXf+f7YwuND+osKbrSrYvZnO2hr6OuthVZ+8rDV+kNUfDf2UwgsoxEKFSHrj6A+50I5yF/EURuJAMBpmtTCm3GhpnbVlA1jIHT8JY3mrk7F1WQryXCrftl5z1jd3s=</P>" +
                      "<Q>7LJ7rlA8k4ErObpKL97UqXf+z9G0gCZc0QCH8dnK3C9nrUXSc4uLW6QASRG1B8khLYB0MOsdVUqkzDn36aksL7VhgFMIqI4huCw7DpLFPvMVsLTr2HINURyBd0DDgIAiLfr6QctBKbAwIXzJv5yhKyILv2zHxlUq+n7M/dIk3iM=</Q>" +
                      "<DP>FUEG6pThHbZesyzfTcUUfXHSnSrAfYAdT6ziM5EhAgyd2JhOTV7elqZbAUzxBQaQP7SL3tOGVfFnEU2WkHsCXrY/zU6tVHI1xTklrkcrc++pLzr0zvsHR1Q73Q/RjCFhRmSH1yp5Aa/60OkQ84SMVVpZaRSpVuExMw5ovXbM+ps=</DP>" +
                      "<DQ>r1iRs1KRbZyVDxDogIoM9PaF+CKcGwtQWyyPiSUU2QTtQzmkbCCGPn9CDt9lQr1HLNQqP0sN0e+YIgsXkyvWJgmyj0Pz+BpC9JMftO4Z3UZrXRrKVPA42UvKxTNfIUTLDVEOL8uJHH7/SO+O7bn002VHWqoVXOIfEq744VfaJA0=</DQ>" +
                      "<InverseQ>KnUBukb1NTahctwIqCzbwTHi2lxlgZmJUk0nD8enote0dOxQGAOdMv0fVWuSMCYQlq4U+unC/XwOY+sdm/81w/ivYNUKz8AQGvm/iwm6S+ENVrXinQOlpizLoWLVyWzgcVCJsJ1U96nq+/hiOmdP7NBCb5rNoDCCsBKhz3tjN7w=</InverseQ>" +
                      "<D>ia1yWAeNBGsRI5FWeHcI0+mCUJzNDm5GEbjh9AIAPemlHk0jCUJXV3j7YJTg5Bhgu1voMQCy4FhZeSchjR0Cs+dcRO06319TKMcJEWXB16wfqmJJE/rLzYK7lWUPo1RhdcDiIDGmCTJrvC+tg0NbtjtN44JUHkjvO7+oO+OO73OuPLC+oJi4PfGILmk5epA6poJtXB0dUQ3IogMTxhbMe/IpJEtXI3817e5jFSykBfJeJ7bO2L38dyFkLHK+CA2cTRWOAxufJY6HZ0ChMm3BRzDYkr/Sh9bwqNSSykLAhUm89wNROzzUJDvRuNhzDlAfJn9W0IsiacpMTkmsgagS5Q==</D>" +
                      "</RSAKeyValue>";

            // Act => Assert
            Assert.Throws<FormatException>(() => TokenHelper.BuildRsaSigningKey(xml));
        }

        /// <summary>
        /// Method: BuildRsaSigningKey
        /// Data: Empty xml
        /// Return: Throws CryptographicException
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void BuildRsaSigningKey_EmptyXml_ThrowsCryptographicException()
        {
            // Arrange
            var xml = "";

            // Act => Assert
            Assert.Throws<CryptographicException>(() => TokenHelper.BuildRsaSigningKey(xml));
        }

        /// <summary>
        /// Method: BuildRsaSigningKey
        /// Data: Null xml
        /// Return: Throws ArgumentNullException
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void BuildRsaSigningKey_NullXml_ThrowsArgumentNullException()
        {
            // Arrange => Act => Assert
            Assert.Throws<ArgumentNullException>(() => TokenHelper.BuildRsaSigningKey(null));
        }
        #endregion

        #region GenerateToken
        /// <summary>
        /// Method: GenerateToken
        /// Data: Null rsa key
        /// Return: Throws ArgumentNullException
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void GenerateToken_NullRsaKey_ThrowsArgumentNullException()
        {
            // Arrange => Act => Assert
            Assert.Throws<ArgumentNullException>(() => TokenHelper.GenerateToken(3851, null, _dateTimeProviderMock.Object));
        }

        /// <summary>
        /// Method: GenerateToken
        /// Data: Null DateTimeProvider
        /// Return: Throws NullReferenceException
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void GenerateToken_NullDateTimeProvider_ThrowsNullReferenceException()
        {
            // Arrange => Act => Assert
            Assert.Throws<NullReferenceException>(() => TokenHelper.GenerateToken(3851, _rsaKeyMock.Object, null));
        }
        #endregion

        #region Private Helpers
        private Mock<IDateTimeProvider> CreateDateTimeProviderMock()
        {
            var mock = new Mock<IDateTimeProvider>();
            mock.Setup(x => x.GetDateTimeNow()).Returns(DateTime.Parse("01/01/2020 15:00"));

            return mock;
        }

        private Mock<RsaSecurityKey> CreateRsaSecurityKeyMock()
        {
            var mock = new Mock<RsaSecurityKey>(new RSACryptoServiceProvider());
            mock.Setup(x => x.KeySize).Returns(2048);
            return mock;
        }
        #endregion
    }
}
