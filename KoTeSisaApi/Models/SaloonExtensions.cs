namespace KoTeSisaApi.Models
{
    public static class SaloonExtensions
    {
        public static SaloonDto ToDto(this Saloon s)
        {
            return new SaloonDto
            {
                SaloonId = s.SaloonId,
                NazivSalona = s.NazivSalona,
                AdresaUlica = s.AdresaUlica,
                AdresaBroj = s.AdresaBroj,
                Grad = s.Grad,
                PostanskiBroj = s.PostanskiBroj,
                Lokacija = s.Lokacija,
                BrojTelefona = s.BrojTelefona,
                Email = s.Email,
                AdminIme = s.AdminIme,
                RadnoVrijemeOd = s.RadnoVrijemeOd,
                RadnoVrijemeDo = s.RadnoVrijemeDo,
                Logo = s.Logo,
                Kreirano = s.Kreirano,
                Azurirano = s.Azurirano
            };
        }
    }
}
