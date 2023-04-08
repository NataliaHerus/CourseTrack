using BusinessLayer.Services;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace CourseTrack.Tests
{
    [TestFixture]
    public class JwtServiceTests
    {
        private IJwtService _jwtService;

        [SetUp]
        public void SetUp()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                {"JWT:Issuer", "TestIssuer"},
                {"JWT:Audience", "TestAudience"},
                {"JWT:SecretKey", "ThisIsAReallyLongSecretKeyThatShouldBeKeptSecret"},
                {"JWT:AccessTokenExpirationMinutes", "30"}
                })
                .Build();

            _jwtService = new JwtService(configuration);
        }

        [Test]
        public void GenerateToken_Should_Return_NonEmpty_String()
        {
            // Arrange
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "testuser@test.com"),
            new Claim(ClaimTypes.Role, "Admin")
        };

            // Act
            var token = _jwtService.GenerateToken(claims);

            // Assert
            Assert.IsNotNull(token);
            Assert.IsNotEmpty(token);
        }

        [Test]
        public void GetPrincipalFromToken_Should_Return_Null_For_Invalid_Token()
        {
            // Arrange
            var token = "invalid-token";

            // Act
            var result = _jwtService.GetPrincipalFromToken(token);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetPrincipalFromToken_Should_Return_ClaimsPrincipal_For_Valid_Token()
        {
            // Arrange
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "testuser@test.com"),
            new Claim(ClaimTypes.Role, "Admin")
        };

            var token = _jwtService.GenerateToken(claims);

            // Act
            var result = _jwtService.GetPrincipalFromToken(token);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ClaimsPrincipal>(result);
        }
    }
}