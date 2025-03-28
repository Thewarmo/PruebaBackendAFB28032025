using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CargoPayWebApi.Services;
using CargoPayWebApi.Models;

[ApiController]
[Route("/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginRequest request)
    {
        if (!_authService.ValidateCredentials(request.Username, request.Password))
            return Unauthorized();

        var token = await _authService.GenerateTokenAsync(request.Username);
        return Ok(new { token });
    }
}