namespace KoTeSisaApi.Models
{
    public class Rezervacija
    {
        public long RezervacijaId { get; set; }
        public long SaloonId { get; set; }

        public DateOnly DatumRezervacije { get; set; }
        public TimeOnly VrijemeRezervacije { get; set; }

        public string UserIme { get; set; } = default!;
        public string UserPrezime { get; set; } = default!;
        public string KontaktTel { get; set; } = default!;

        public int? UslugaId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Saloon? Saloon { get; set; }
    }
}
