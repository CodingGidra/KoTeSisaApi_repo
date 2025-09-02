using KoTeSisaApi.Data;
using KoTeSisaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Models;

namespace KoTeSisaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UslugeController : ControllerBase
    {
        private readonly AppDb _db;
        public UslugeController(AppDb db)
        {
            _db = db;
        }

        // GET: api/usluge
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usluga>>> GetAll()
        {
            var list = await _db.Usluge
                .Where(u => u.Aktivno)
                .OrderBy(u => u.Naziv)
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/usluge/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Usluga>> GetOne(int id)
        {
            var usluga = await _db.Usluge.FindAsync(id);
            if (usluga == null) return NotFound();
            return Ok(usluga);
        }
    }
}
