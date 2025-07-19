using IdentityAPI.Authentication.Domain.Models;
using IdentityAPI.Authentication.Infrastructure.Services;
using IdentityAPI.Configuration;
using IdentityAPI.Tests.Data.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace IdentityAPI.Tests.Authentication.Infrastructure.Services;

public class JwtTokenServiceTests
{
    // constants
    private readonly JwtOptions jwtOptions = new()
    {
        Issuer = "Issuer",
        Audience = "Audience",
        AccessTokenExpiration = 300,
        RefreshTokenExpiration = 3600,
        SigningKey = "MyVeryVerySecureJwtSigningKey"
    };

    // dependencies
    private Mock<SecurityTokenHandler> securityTokenHandler;

    private Mock<IOptions<JwtOptions>> options;

    // under test
    private JwtTokenService jwtTokenService;

    [SetUp]
    public void SetUp()
    {
        securityTokenHandler = new Mock<SecurityTokenHandler>();

        securityTokenHandler
            .Setup(s => s.CreateToken(It.IsAny<SecurityTokenDescriptor>()));

        securityTokenHandler
            .Setup(s => s.WriteToken(It.IsAny<SecurityToken>()));

        options = new Mock<IOptions<JwtOptions>>();

        options
            .Setup(o => o.Value)
            .Returns(jwtOptions);

        jwtTokenService = new JwtTokenService(securityTokenHandler.Object, options.Object);
    }

    [TestCaseSource(typeof(UserSource), nameof(UserSource.TestCases))]
    public void Given_User_When_GenerateTokenPair_Then_Calls_SecurityTokenHandler_CreateToken_Twice(User user)
    {
        // when
        jwtTokenService.GenerateTokenPair(user);

        // then
        securityTokenHandler.Verify(s => s.CreateToken(It.IsAny<SecurityTokenDescriptor>()), Times.Exactly(2));
    }

    [TestCaseSource(typeof(UserSource), nameof(UserSource.TestCases))]
    public void Given_User_When_GenerateTokenPair_Then_Calls_SecurityTokenHandler_WriteToken_Twice(User user)
    {
        // when
        jwtTokenService.GenerateTokenPair(user);

        // then
        securityTokenHandler.Verify(s => s.WriteToken(It.IsAny<SecurityToken>()), Times.Exactly(2));
    }

    [TestCaseSource(typeof(UserSource), nameof(UserSource.TestCases))]
    public void Given_User_When_GenerateTokenPain_Then_Sets_Correct_Claims_And_Issuer(User user)
    {
        // given
        List<SecurityTokenDescriptor> descriptors = [];

        securityTokenHandler
            .Setup(s => s.CreateToken(It.IsAny<SecurityTokenDescriptor>()))
            .Callback(descriptors.Add);

        // when
        jwtTokenService.GenerateTokenPair(user);

        // then
        Assert.That(descriptors, Has.Count.EqualTo(2));

        foreach (SecurityTokenDescriptor descriptor in descriptors)
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(descriptor.Issuer, Is.EqualTo(jwtOptions.Issuer));
                Assert.That(descriptor.Audience, Is.EqualTo(jwtOptions.Audience));

                Assert.That(descriptor.Subject.FindFirst("Sub")?.Value, Is.EqualTo(user.Id));
                Assert.That(descriptor.Subject.FindFirst("Name")?.Value, Is.EqualTo(user.Username));
            }
        }
    }
}