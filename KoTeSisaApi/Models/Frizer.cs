using System.ComponentModel.DataAnnotations.Schema;

namespace KoTeSisaApi.Models
{
    public class Frizer
    {
        public int Id { get; set; }
        public long SaloonId { get; set; }
        public string Ime { get; set; } = "";
        public string Prezime { get; set; } = "";
        public string? KontaktBroj { get; set; }
        public string? Slika { get; set; }

        [ForeignKey(nameof(SaloonId))]
        public Saloon Saloon { get; set; } = default!;
    }
}
