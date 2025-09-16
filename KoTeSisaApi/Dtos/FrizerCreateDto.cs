namespace KoTeSisaApi.Dtos
{
    public class FrizerCreateDto
    {
        public int SaloonId { get; set; }
        public string Ime { get; set; } = "";
        public string Prezime { get; set; } = "";
        public string? KontaktBroj { get; set; }
        public string? Slika { get; set; }
    }
}
