using KoTeSisaApi.Data;
using KoTeSisaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoTeSisaApi.Controllers;

[ApiController]
[Route("rezervacije")]
public class RezervacijaController : ControllerBase
{
    private readonly AppDb _db;
    public RezervacijaController(AppDb db) => _db = db;

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] RezervacijaDto dto, CancellationToken ct)
    {
        var salonExists = await _db.Saloons.AnyAsync(s => s.SaloonId == dto.SaloonId, ct);
        if (!salonExists) return NotFound(new { message = "Salon ne postoji." });

        var conflict = await _db.Rezervacije.AnyAsync(x =>
            x.SaloonId == dto.SaloonId &&
            x.DatumRezervacije == dto.DatumRezervacije &&
            x.VrijemeRezervacije == dto.VrijemeRezervacije, ct);
        if (conflict) return Conflict(new { message = "Termin je zauzet." });

        var ent = new Rezervacija
        {
            SaloonId = dto.SaloonId,
            DatumRezervacije = dto.DatumRezervacije,
            VrijemeRezervacije = dto.VrijemeRezervacije,
            UserIme = dto.UserIme.Trim(),
            UserPrezime = dto.UserPrezime.Trim(),
            KontaktTel = dto.KontaktTel.Trim(),
            Usluga = dto.Usluga,
            UslugaId = dto.UslugaId,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        _db.Rezervacije.Add(ent);

        try
        {
            await _db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            return Conflict(new { message = "Termin je zauzet." });
        }

        return CreatedAtAction(nameof(GetById), new { id = ent.RezervacijaId },
            new { rezervacija_id = ent.RezervacijaId });
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var r = await _db.Rezervacije.FirstOrDefaultAsync(x => x.RezervacijaId == id, ct);
        if (r is null) return NotFound();

        return Ok(new
        {
            rezervacija_id = r.RezervacijaId,
            saloon_id = r.SaloonId,
            datum_rezervacije = r.DatumRezervacije,
            vrijeme_rezervacije = r.VrijemeRezervacije,
            user_ime = r.UserIme,
            user_prezime = r.UserPrezime,
            kontakt_tel = r.KontaktTel,
            usluga = r.Usluga,
            usluga_id = r.UslugaId,
            created_at = r.CreatedAt,
            updated_at = r.UpdatedAt
        });
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(long id, [FromBody] RezervacijaDto dto, CancellationToken ct)
    {
        var r = await _db.Rezervacije.FirstOrDefaultAsync(x => x.RezervacijaId == id, ct);
        if (r is null) return NotFound(new { message = "Rezervacija ne postoji." });

        if (r.SaloonId != dto.SaloonId)
        {
            var salonExists = await _db.Saloons.AnyAsync(s => s.SaloonId == dto.SaloonId, ct);
            if (!salonExists) return NotFound(new { message = "Salon ne postoji." });
        }

        var conflict = await _db.Rezervacije.AnyAsync(x =>
            x.RezervacijaId != id &&
            x.SaloonId == dto.SaloonId &&
            x.DatumRezervacije == dto.DatumRezervacije &&
            x.VrijemeRezervacije == dto.VrijemeRezervacije, ct);
        if (conflict) return Conflict(new { message = "Termin je zauzet." });

        r.SaloonId = dto.SaloonId;
        r.DatumRezervacije = dto.DatumRezervacije;
        r.VrijemeRezervacije = dto.VrijemeRezervacije;
        r.UserIme = dto.UserIme.Trim();
        r.UserPrezime = dto.UserPrezime.Trim();
        r.KontaktTel = dto.KontaktTel.Trim();
        if (!string.IsNullOrWhiteSpace(dto.Usluga)) r.Usluga = dto.Usluga;
        r.UslugaId = dto.UslugaId;
        r.UpdatedAt = DateTimeOffset.UtcNow;

        try
        {
            await _db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            return Conflict(new { message = "Termin je zauzet." });
        }

        return Ok(new
        {
            rezervacija_id = r.RezervacijaId,
            saloon_id = r.SaloonId,
            datum_rezervacije = r.DatumRezervacije,
            vrijeme_rezervacije = r.VrijemeRezervacije,
            user_ime = r.UserIme,
            user_prezime = r.UserPrezime,
            kontakt_tel = r.KontaktTel,
            usluga = r.Usluga,
            usluga_id = r.UslugaId,
            created_at = r.CreatedAt,
            updated_at = r.UpdatedAt
        });
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var r = await _db.Rezervacije.FirstOrDefaultAsync(x => x.RezervacijaId == id, ct);
        if (r is null) return NotFound(new { message = "Rezervacija ne postoji." });

        _db.Rezervacije.Remove(r);
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }
}
