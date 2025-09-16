namespace KoTeSisaApi.Dtos
{
    public class FrizerDto
    {
        public int Id { get; set; }
        public long SaloonId { get; set; }
        public string Ime { get; set; } = "";
        public string Prezime { get; set; } = "";
        public string? KontaktBroj { get; set; }
        public string? Slika { get; set; }
    }
}
