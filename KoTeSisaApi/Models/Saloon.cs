namespace KoTeSisaApi.Models
{
    public class Saloon
    {
        public long SaloonId { get; set; }            
        public string NazivSalona { get; set; } = "";
        public string AdresaUlica { get; set; } = "";
        public string? AdresaBroj { get; set; }
        public string Grad { get; set; } = "";
        public string? PostanskiBroj { get; set; }
        public string? Lokacija { get; set; }
        public string BrojTelefona { get; set; } = "";
        public string Email { get; set; } = "";
        public string AdminIme { get; set; } = "";
        public string Password { get; set; } = "";
        public string? RadnoVrijeme { get; set; }
        public string? Logo { get; set; }
        public DateTime Kreirano { get; set; }
        public DateTime Azurirano { get; set; }
    }
}
