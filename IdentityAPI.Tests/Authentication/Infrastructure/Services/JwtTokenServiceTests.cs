using System.Security.Claims;
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

    private readonly TokenValidationResult successfulResult = new()
    {
        IsValid = true
    };

    private const string testToken = "token";

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

        securityTokenHandler
            .Setup(s => s.ValidateTokenAsync(It.IsAny<string>(), It.IsAny<TokenValidationParameters>()))
            .ReturnsAsync(successfulResult);

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
    public void Given_User_When_GenerateTokenPair_Then_Sets_Correct_Claims_And_Issuer(User user)
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

    [Test]
    public async Task Given_Token_When_ValidateToken_Then_Calls_SecurityTokenHandler_ValidateTokenAsync()
    {
        // when
        await jwtTokenService.ValidateToken(testToken);

        // then
        securityTokenHandler.Verify(s => s.ValidateTokenAsync(testToken, It.IsAny<TokenValidationParameters>()));
    }

    [Test]
    public async Task Given_Valid_Token_When_ValidateToken_Then_Returns_True()
    {
        // when
        bool result = await jwtTokenService.ValidateToken(testToken);

        // then
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task Given_Invalid_Token_When_ValidateToken_Then_Returns_False()
    {
        // given
        TokenValidationResult unsuccessfulResult = new()
        {
            IsValid = false
        };

        securityTokenHandler
            .Setup(s => s.ValidateTokenAsync(testToken, It.IsAny<TokenValidationParameters>()))
            .ReturnsAsync(unsuccessfulResult);

        // when
        bool result = await jwtTokenService.ValidateToken(testToken);

        // then
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task Given_Existing_Claim_When_ExtractClaim_Then_Returns_Value()
    {
        // given
        const string claimKey = "key";
        const string claimValue = "value";

        ClaimsIdentity identity = new();
        identity.AddClaim(new Claim(claimKey, claimValue));

        TokenValidationResult claimsResult = new()
        {
            ClaimsIdentity = identity
        };

        securityTokenHandler
            .Setup(s => s.ValidateTokenAsync(testToken, It.IsAny<TokenValidationParameters>()))
            .ReturnsAsync(claimsResult);

        // when
        string? value = await jwtTokenService.ExtractClaim<string>(testToken, claimKey);

        // then
        Assert.That(value, Is.EqualTo(claimValue));
    }

    [Test]
    public async Task Given_NonExisting_Claim_When_ExtractClaim_Then_Returns_Null()
    {
        // given
        const string claimKey = "key";
        const string claimValue = "value";

        ClaimsIdentity identity = new();
        identity.AddClaim(new Claim(claimKey, claimValue));

        TokenValidationResult claimsResult = new()
        {
            ClaimsIdentity = identity
        };

        securityTokenHandler
            .Setup(s => s.ValidateTokenAsync(testToken, It.IsAny<TokenValidationParameters>()))
            .ReturnsAsync(claimsResult);

        // when
        string? value = await jwtTokenService.ExtractClaim<string>(testToken, "nonExistingKey");

        // then
        Assert.That(value, Is.Null);
    }
}