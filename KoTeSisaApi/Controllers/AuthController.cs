using KoTeSisaApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoTeSisaApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AppDb _db;
    public AuthController(AppDb db) => _db = db;

    public record LoginRequest(string Email, string Password);

    public record LoginResponse(
        long SaloonId,
        string NazivSalona,
        string Email,
        string AdminIme
    );

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest req)
    {
        var saloon = await _db.Saloons
            .FirstOrDefaultAsync(s => s.Email == req.Email && s.Password == req.Password);

        if (saloon is null)
            return Unauthorized("Invalid credentials.");

        return Ok(new LoginResponse(
            saloon.SaloonId,
            saloon.NazivSalona,
            saloon.Email,
            saloon.AdminIme
        ));
    }
}
