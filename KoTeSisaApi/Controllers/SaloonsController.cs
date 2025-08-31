using KoTeSisaApi.Data;
using KoTeSisaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoTeSisaApi.Controllers;

[ApiController]
[Route("saloons")]
public class SaloonsController : ControllerBase
{
    private readonly AppDb _db;
    public SaloonsController(AppDb db) => _db = db;

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Saloon>> GetById(long id)
    {
        var saloon = await _db.Saloons.FindAsync(id);
        return saloon is null ? NotFound() : Ok(saloon);
    }

    [HttpPost]
    public async Task<ActionResult<Saloon>> Create([FromBody] Saloon s)
    {
        _db.Saloons.Add(s);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = s.SaloonId }, s);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] Saloon s)
    {
        if (id != s.SaloonId)
            return BadRequest("ID iz rute i body-ja se ne poklapaju.");

        var existing = await _db.Saloons.FindAsync(id);
        if (existing is null) return NotFound();

        // Ažuriramo polja (explicitan update da izbjegnemo overposting)
        existing.NazivSalona = s.NazivSalona;
        existing.AdresaUlica = s.AdresaUlica;
        existing.AdresaBroj = s.AdresaBroj;
        existing.Grad = s.Grad;
        existing.PostanskiBroj = s.PostanskiBroj;
        existing.Lokacija = s.Lokacija;
        existing.BrojTelefona = s.BrojTelefona;
        existing.Email = s.Email;
        existing.AdminIme = s.AdminIme;
        existing.Password = s.Password;      // (kasnije ćemo hash)
        existing.RadnoVrijeme = s.RadnoVrijeme;
        existing.Logo = s.Logo;
        existing.Azurirano = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(existing);
    }
}
