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
            usluga_id = r.UslugaId,
            created_at = r.CreatedAt,
            updated_at = r.UpdatedAt
        });
    }

    [HttpGet("dan")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetForDay([FromQuery] long saloon_id, [FromQuery] DateOnly datum, CancellationToken ct)
    {
        var times = await _db.Rezervacije
            .Where(x => x.SaloonId == saloon_id && x.DatumRezervacije == datum)
            .OrderBy(x => x.VrijemeRezervacije)
            .Select(x => x.VrijemeRezervacije) // TimeOnly
            .ToListAsync(ct);

        var result = times.Select(t => t.ToString("HH:mm")); // npr. "10:45"
        return Ok(result);
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

    // GET /rezervacije/slotovi?saloon_id=1&datum=2025-09-04&od=09:00&do=17:00&korak=15
    [HttpGet("slotovi")]
    public async Task<IActionResult> GetSlotovi(
        [FromQuery] long saloon_id,
        [FromQuery] DateOnly datum,
        [FromQuery] string od,
        [FromQuery] string @do,
        [FromQuery] int korak = 15,
        CancellationToken ct = default)
    {
        if (!TimeSpan.TryParse(od, out var tOd) ||
            !TimeSpan.TryParse(@do, out var tDo) ||
            korak <= 0)
            return BadRequest(new { poruka = "Parametri od/do/korak nisu validni." });

        var danOd = Spoji(datum, TimeOnly.FromTimeSpan(tOd));
        var danDo = Spoji(datum, TimeOnly.FromTimeSpan(tDo));
        if (danDo <= danOd) return BadRequest(new { poruka = "Vrijeme 'do' mora biti poslije 'od'." });

        var korakTs = TimeSpan.FromMinutes(korak);
        var defaultTrajanje = TimeSpan.FromMinutes(30); // ako UslugaId == null

        // LEFT JOIN: rezervacije ↔ usluge (jer je UslugaId nullable)
        var rasponi = await _db.Rezervacije
            .Where(r => r.SaloonId == saloon_id && r.DatumRezervacije == datum)
            .GroupJoin(_db.Usluge,
                       r => (int?)r.UslugaId,
                       u => (int?)u.UslugaId,
                       (r, us) => new { r, us })
            .SelectMany(x => x.us.DefaultIfEmpty(), (x, u) => new
            {
                Start = Spoji(x.r.DatumRezervacije, x.r.VrijemeRezervacije),
                End = Spoji(x.r.DatumRezervacije, x.r.VrijemeRezervacije)
                       + (u != null ? (u.Trajanje + u.Buffer) : defaultTrajanje)
            })
            .ToListAsync(ct);

        var slotovi = new List<object>();
        for (var t = danOd; t < danDo; t = t.Add(korakTs))
        {
            var start = t;
            var end = t.Add(korakTs);
            var zauzet = rasponi.Any(r => start < r.End && end > r.Start);
            slotovi.Add(new { vrijeme = start.ToString("HH\\:mm"), zauzet });
        }

        var dodatniStartovi = rasponi
            .Select(r => r.End)
            .Where(e => e >= danOd && e < danDo)
            .Select(e => e.ToString("HH\\:mm"))
            .Distinct()
            .ToList();

        foreach (var s in dodatniStartovi)
        {
            if (TimeSpan.TryParse(s, out var ts))
            {
                var start = Spoji(datum, TimeOnly.FromTimeSpan(ts));
                if (start >= danOd && start < danDo)
                {
                    var end = start.Add(korakTs);
                    var zauzet = rasponi.Any(r => start < r.End && end > r.Start);
                    slotovi.Add(new { vrijeme = s, zauzet });
                }
            }
        }


        return Ok(new
        {
            datum = datum.ToString("yyyy-MM-dd"),
            korak_minuta = korak,
            slotovi,
            dodatni_startovi = dodatniStartovi
        });
    }

    // Helper (dodaj u isti controller, npr. ispod action-a):
    private static DateTimeOffset Spoji(DateOnly d, TimeOnly t)
    {
        var dt = new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, t.Second, DateTimeKind.Unspecified);
        return new DateTimeOffset(dt, TimeSpan.Zero);
    }

}
