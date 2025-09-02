using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KoTeSisaApi.Models;

public sealed class RezervacijaDto
{
    [JsonPropertyName("saloon_id")]
    [Required]
    public long SaloonId { get; set; }

    [JsonPropertyName("datum_rezervacije")]
    [Required]
    public DateOnly DatumRezervacije { get; set; }

    [JsonPropertyName("vrijeme_rezervacije")]
    [Required]
    public TimeOnly VrijemeRezervacije { get; set; }

    [JsonPropertyName("user_ime")]
    [Required, StringLength(100)]
    public string UserIme { get; set; } = default!;

    [JsonPropertyName("user_prezime")]
    [Required, StringLength(100)]
    public string UserPrezime { get; set; } = default!;

    [JsonPropertyName("kontakt_tel")]
    [Required, StringLength(50)]
    public string KontaktTel { get; set; } = default!;

    [JsonPropertyName("usluga_id")]
    public int? UslugaId { get; set; }

    [JsonPropertyName("usluga")]
    [StringLength(100)]
    public string? Usluga { get; set; }
}
