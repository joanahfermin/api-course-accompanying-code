using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Tests;

[TestClass]
public class AuthControllerTests
{
    private AuthController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        var settings = new JwtSettings
        {
            Issuer = "BookShop.Api",
            Audience = "BookShop",
            Key = "a-test-key-that-is-long-enough-for-hs256",
            MinutesValid = 60
        };
        _controller = new AuthController(new FakeUserService(), new TokenService(settings), new FakeLogger<AuthController>());
    }

    [TestMethod]
    public async Task Login_RightPassword_ReturnsATokenAndTheRole()
    {
        var result = await _controller.Login(new LoginRequest("alice", "secret"));

        Assert.IsNotNull(result.Value);
        Assert.IsFalse(string.IsNullOrEmpty(result.Value.Token));
        Assert.AreEqual("Manager", result.Value.Role);
    }

    [TestMethod]
    public async Task Login_WrongPassword_Returns401()
    {
        var result = await _controller.Login(new LoginRequest("alice", "guess"));

        var answer = result.Result as ObjectResult;
        Assert.IsNotNull(answer);
        Assert.AreEqual(401, answer.StatusCode);
    }
}
