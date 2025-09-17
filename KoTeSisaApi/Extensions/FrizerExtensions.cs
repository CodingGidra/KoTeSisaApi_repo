using KoTeSisaApi.Models;
using KoTeSisaApi.Dtos;

namespace KoTeSisaApi.Extensions;

public static class FrizerExtensions
{
    public static FrizerDto ToDto(this Frizer f)
    {
        return new FrizerDto
        {
            Id = f.Id,
            SaloonId = f.SaloonId,
            Ime = f.Ime,
            Prezime = f.Prezime,
            KontaktBroj = f.KontaktBroj,
            Slika = f.Slika
        };
    }
}
