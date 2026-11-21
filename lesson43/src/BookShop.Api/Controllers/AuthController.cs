using BookShop.Data;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly TokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IUserService userService, TokenService tokenService,
        ILogger<AuthController> logger)
    {
        _userService = userService;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost("login")]
    [ProducesResponseType<LoginResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var user = await _userService.FindUserAsync(request.UserName, request.Password);
        if (user == null)
        {
            _logger.LogWarning("Failed login for {UserName}", request.UserName);
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Wrong user name or password.");
        }

        _logger.LogInformation("{UserName} logged in as {Role}", user.UserName, user.Role);
        var token = _tokenService.CreateToken(user);
        return new LoginResponse(token, user.Role);
    }
}
