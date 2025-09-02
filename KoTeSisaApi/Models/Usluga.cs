using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourNamespace.Models
{
    // Mapira na Postgres tabelu "usluge"
    [Table("usluge")]
    public class Usluga
    {
        [Key]
        [Column("usluga_id")]
        public int UslugaId { get; set; }

        [Required]
        [Column("naziv")]
        [MaxLength(255)] // po potrebi prilagodi
        public string Naziv { get; set; } = string.Empty;

        // Postgres interval -> .NET TimeSpan
        [Required]
        [Column("trajanje", TypeName = "interval")]
        public TimeSpan Trajanje { get; set; }

        // Postgres interval -> .NET TimeSpan
        [Required]
        [Column("buffer", TypeName = "interval")]
        public TimeSpan Buffer { get; set; }

        [Required]
        [Column("aktivno")]
        public bool Aktivno { get; set; }

        // timestamptz -> preporučeno .NET DateTimeOffset
        [Required]
        [Column("kreirano")]
        public DateTimeOffset Kreirano { get; set; }

        [Required]
        [Column("azurirano")]
        public DateTimeOffset Azurirano { get; set; }
    }
}
