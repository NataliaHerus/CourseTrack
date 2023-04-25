using BusinessLayer.Account;
using BusinessLayer.Services;
using DataLayer.Account;
using DataLayer.Enums;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Security.Claims;

namespace CourseTrack.Tests
{
    [TestFixture]
    public class AccountFacadeTests
    {
        private AccountFacade _accountFacade;
        private Mock<IJwtService> _jwtServiceMock;
        private Mock<IConfiguration> _configurationMock;
        private Mock<IAccountRepository> _accountRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _jwtServiceMock = new Mock<IJwtService>();
            _configurationMock = new Mock<IConfiguration>();
            _accountRepositoryMock = new Mock<IAccountRepository>();

            _accountFacade = new AccountFacade(
                _jwtServiceMock.Object,
                _configurationMock.Object,
                _accountRepositoryMock.Object);
        }

        [Test]
        public void Register_WhenUserDoesNotExist_ShouldCallRegisterStudent()
        {
            // Arrange
            var firstName = "John";
            var lastName = "Doe";
            var email = "johndoe@example.com";
            var password = "password";
            _configurationMock.Setup(x => x["EncryptionKey"]).Returns("some_key");
            _accountRepositoryMock.Setup(x => x.UserExists(email)).Returns(false);

            // Act
            _accountFacade.Register(firstName, lastName, email, password);
        }

        [Test]
        public void Register_WhenUserExists_ShouldThrowArgumentException()
        {
            // Arrange
            var firstName = "John";
            var lastName = "Doe";
            var email = "johndoe@example.com";
            var password = "password";
            _configurationMock.Setup(x => x["EncryptionKey"]).Returns("some_key");
            _accountRepositoryMock.Setup(x => x.UserExists(email)).Returns(true);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _accountFacade.Register(firstName, lastName, email, password));
        }

        [Test]
        public void Login_WhenPasswordIsValid_ShouldReturnTrue()
        {
            // Arrange
            var email = "johndoe@example.com";
            var password = "password";
            var encryptionKey = "some_key";
            _configurationMock.Setup(x => x["EncryptionKey"]).Returns(encryptionKey);
            _accountRepositoryMock.Setup(x => x.LecturerPasswordIsValid(email, It.IsAny<string>())).Returns(true);
            _accountRepositoryMock.Setup(x => x.StudentPasswordIsValid(email, It.IsAny<string>())).Returns(true);

            // Act
            var result = _accountFacade.Login(email, password);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Login_WhenPasswordIsInvalid_ShouldReturnFalse()
        {
            // Arrange
            var email = "johndoe@example.com";
            var password = "password";
            var encryptionKey = "some_key";
            _configurationMock.Setup(x => x["EncryptionKey"]).Returns(encryptionKey);
            _accountRepositoryMock.Setup(x => x.LecturerPasswordIsValid(email, It.IsAny<string>())).Returns(false);
            _accountRepositoryMock.Setup(x => x.StudentPasswordIsValid(email, It.IsAny<string>())).Returns(false);

            // Act
            var result = _accountFacade.Login(email, password);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
