using BookShop.Data;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BookShop.Api.Tests;

[TestClass]
public class TokenServiceTests
{
    private static readonly JwtSettings Settings = new()
    {
        Issuer = "BookShop.Api",
        Audience = "BookShop",
        Key = "a-test-key-that-is-long-enough-for-hs256",
        MinutesValid = 60
    };

    [TestMethod]
    public void CreateToken_User_CarriesTheirNameAndRole()
    {
        var service = new TokenService(Settings);

        var token = service.CreateToken(new AppUser { UserName = "bob", Role = "Staff" });

        var read = new JsonWebToken(token);
        Assert.AreEqual("bob", read.GetClaim("name").Value);
        Assert.AreEqual("Staff", read.GetClaim("role").Value);
        Assert.AreEqual("BookShop.Api", read.Issuer);
    }

    [TestMethod]
    public void CreateToken_User_ExpiresAfterTheConfiguredMinutes()
    {
        var service = new TokenService(Settings);

        var token = service.CreateToken(new AppUser { UserName = "bob", Role = "Staff" });

        var read = new JsonWebToken(token);
        var minutesLeft = (read.ValidTo - DateTime.UtcNow).TotalMinutes;
        Assert.IsTrue(minutesLeft > 59 && minutesLeft <= 60);
    }
}
