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
    public async Task<ActionResult<SaloonDto>> GetById(long id)
    {
        var saloon = await _db.Saloons.FindAsync(id);
        return saloon is null ? NotFound() : Ok(saloon.ToDto());
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<SaloonDto>>> Search([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q) || q.Trim().Length < 3)
            return Ok(new List<SaloonDto>());

        q = q.Trim();

        // Postgres case-insensitive: ILIKE
        var list = await _db.Saloons
            .Where(s =>
                EF.Functions.ILike(s.NazivSalona, $"%{q}%") ||
                EF.Functions.ILike(s.Grad, $"%{q}%") ||
                EF.Functions.ILike(s.AdresaUlica, $"%{q}%"))
            .OrderBy(s => s.NazivSalona)
            .Select(s => s.ToDto())     
            .ToListAsync();

        return Ok(list);
    }

    [HttpPost]
    public async Task<ActionResult<SaloonDto>> Create([FromBody] Saloon s)
    {
        _db.Saloons.Add(s);
        await _db.SaveChangesAsync();

        var dto = s.ToDto();
        return CreatedAtAction(nameof(GetById), new { id = s.SaloonId }, dto);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<SaloonDto>> Update(long id, Saloon s)
    {
        var existing = await _db.Saloons.FindAsync(id);
        if (existing is null) return NotFound();

        // ažuriraj samo polja
        existing.NazivSalona = s.NazivSalona;
        existing.AdresaUlica = s.AdresaUlica;
        existing.AdresaBroj = s.AdresaBroj;
        existing.Grad = s.Grad;
        existing.PostanskiBroj = s.PostanskiBroj;
        existing.Lokacija = s.Lokacija;
        existing.BrojTelefona = s.BrojTelefona;
        existing.Email = s.Email; // unique ali ostaje isti red
        existing.AdminIme = s.AdminIme;
        existing.Password = s.Password;
        existing.RadnoVrijemeOd = s.RadnoVrijemeOd;
        existing.RadnoVrijemeDo = s.RadnoVrijemeDo;
        existing.Logo = s.Logo;
        existing.Azurirano = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok(new SaloonDto
        {
            SaloonId = existing.SaloonId,
            NazivSalona = existing.NazivSalona,
            AdresaUlica = existing.AdresaUlica,
            AdresaBroj = existing.AdresaBroj,
            Grad = existing.Grad,
            PostanskiBroj = existing.PostanskiBroj,
            Lokacija = existing.Lokacija,
            BrojTelefona = existing.BrojTelefona,
            Email = existing.Email,
            AdminIme = existing.AdminIme,
            RadnoVrijemeOd = existing.RadnoVrijemeOd,
            RadnoVrijemeDo = existing.RadnoVrijemeDo,
            Logo = existing.Logo,
            Kreirano = existing.Kreirano,
            Azurirano = existing.Azurirano
        });
    }


    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var saloon = await _db.Saloons.FindAsync(id);
        if (saloon is null)
        {
            return NotFound(new { message = $"Saloon with id {id} not found." });
        }

        _db.Saloons.Remove(saloon);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Saloon uspješno obrisan." });
    }

}
