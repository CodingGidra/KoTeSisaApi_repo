using KoTeSisaApi.Data;
using KoTeSisaApi.Dtos;
using KoTeSisaApi.Extensions;
using KoTeSisaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoTeSisaApi.Controllers;

[ApiController]
public class FrizerController : ControllerBase
{
    private readonly AppDb _db;
    public FrizerController(AppDb db) => _db = db;

    [HttpPost("frizer")]
    public async Task<ActionResult<FrizerDto>> Create([FromBody] FrizerCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Ime) || string.IsNullOrWhiteSpace(dto.Prezime))
            return BadRequest(new { message = "Ime i Prezime su obavezni." });

        var saloonExists = await _db.Saloons.AnyAsync(s => s.SaloonId == dto.SaloonId);
        if (!saloonExists)
            return BadRequest(new { message = $"SaloonId {dto.SaloonId} nije važeći." });

        var entity = new Frizer
        {
            SaloonId = dto.SaloonId,
            Ime = dto.Ime.Trim(),
            Prezime = dto.Prezime.Trim(),
            KontaktBroj = string.IsNullOrWhiteSpace(dto.KontaktBroj) ? null : dto.KontaktBroj.Trim(),
            Slika = string.IsNullOrWhiteSpace(dto.Slika) ? null : dto.Slika.Trim()
        };

        _db.Frizeri.Add(entity);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity.ToDto());
    }

    [HttpGet("frizer/{id:int}")]
    public async Task<ActionResult<FrizerDto>> GetById(int id)
    {
        var f = await _db.Frizeri.FindAsync(id);
        return f is null ? NotFound() : Ok(f.ToDto());
    }

    [HttpGet("frizers/{saloonId:long}")]
    public async Task<ActionResult<List<FrizerDto>>> ListBySaloon(long saloonId)
    {
        var exists = await _db.Saloons.AnyAsync(s => s.SaloonId == saloonId);
        if (!exists) return NotFound(new { message = $"Saloon {saloonId} ne postoji." });

        var list = await _db.Frizeri
            .Where(f => f.SaloonId == saloonId)
            .OrderBy(f => f.Prezime).ThenBy(f => f.Ime)
            .Select(f => f.ToDto())
            .ToListAsync();

        return Ok(list);
    }

    [HttpPut("frizer/{id:int}")]
    public async Task<ActionResult<FrizerDto>> Update(int id, [FromBody] FrizerCreateDto dto)
    {
        var f = await _db.Frizeri.FindAsync(id);
        if (f is null) return NotFound();

        if (!await _db.Saloons.AnyAsync(s => s.SaloonId == dto.SaloonId))
            return BadRequest(new { message = $"SaloonId {dto.SaloonId} nije važeći." });

        f.SaloonId = dto.SaloonId;
        f.Ime = dto.Ime?.Trim() ?? "";
        f.Prezime = dto.Prezime?.Trim() ?? "";
        f.KontaktBroj = string.IsNullOrWhiteSpace(dto.KontaktBroj) ? null : dto.KontaktBroj.Trim();
        f.Slika = string.IsNullOrWhiteSpace(dto.Slika) ? null : dto.Slika.Trim();

        await _db.SaveChangesAsync();
        return Ok(f.ToDto());
    }

    [HttpDelete("frizer/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var f = await _db.Frizeri.FindAsync(id);
        if (f is null) return NotFound();

        _db.Frizeri.Remove(f);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Frizer obrisan." });
    }
}
