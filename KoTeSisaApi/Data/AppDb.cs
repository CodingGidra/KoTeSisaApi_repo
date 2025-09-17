using KoTeSisaApi.Models;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Models;

namespace KoTeSisaApi.Data
{
    public class AppDb : DbContext
    {
        public AppDb(DbContextOptions<AppDb> opt) : base(opt) { }

        public DbSet<Saloon> Saloons => Set<Saloon>();
        public DbSet<Rezervacija> Rezervacije => Set<Rezervacija>();
        public DbSet<Usluga> Usluge => Set<Usluga>();
        public DbSet<Frizer> Frizeri => Set<Frizer>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
            var e = mb.Entity<Saloon>();
            e.ToTable("saloon");
            e.HasKey(x => x.SaloonId);
            e.Property(x => x.SaloonId).HasColumnName("saloon_id");
            e.Property(x => x.NazivSalona).HasColumnName("naziv_salona");
            e.Property(x => x.AdresaUlica).HasColumnName("adresa_ulica");
            e.Property(x => x.AdresaBroj).HasColumnName("adresa_broj");
            e.Property(x => x.Grad).HasColumnName("grad");
            e.Property(x => x.PostanskiBroj).HasColumnName("postanski_broj");
            e.Property(x => x.Lokacija).HasColumnName("lokacija");
            e.Property(x => x.BrojTelefona).HasColumnName("broj_telefona");
            e.Property(x => x.Email).HasColumnName("email");
            e.Property(x => x.AdminIme).HasColumnName("admin_ime");
            e.Property(x => x.Password).HasColumnName("password");
            e.Property(x => x.RadnoVrijemeOd).HasColumnName("radno_vrijeme_od");
            e.Property(x => x.RadnoVrijemeDo).HasColumnName("radno_vrijeme_do");
            e.Property(x => x.Logo).HasColumnName("logo");
            e.Property(x => x.Kreirano).HasColumnName("kreirano");
            e.Property(x => x.Azurirano).HasColumnName("azurirano");

            var r = mb.Entity<Rezervacija>();
            r.ToTable("rezervacije");
            r.HasKey(x => x.RezervacijaId);
            r.Property(x => x.RezervacijaId).HasColumnName("rezervacija_id");
            r.Property(x => x.SaloonId).HasColumnName("saloon_id");
            r.Property(x => x.DatumRezervacije).HasColumnName("datum_rezervacije");
            r.Property(x => x.VrijemeRezervacije).HasColumnName("vrijeme_rezervacije");
            r.Property(x => x.UserIme).HasColumnName("user_ime");
            r.Property(x => x.UserPrezime).HasColumnName("user_prezime");
            r.Property(x => x.KontaktTel).HasColumnName("kontakt_tel");
            r.Property(x => x.UslugaId).HasColumnName("usluga_id");
            r.Property(x => x.CreatedAt).HasColumnName("created_at");
            r.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            r.HasOne(x => x.Saloon)
             .WithMany()
             .HasForeignKey(x => x.SaloonId)
             .HasPrincipalKey(s => s.SaloonId)
             .OnDelete(DeleteBehavior.Restrict);

            var u = mb.Entity<Usluga>();
            u.ToTable("usluge");
            u.HasKey(x => x.UslugaId);
            u.Property(x => x.UslugaId).HasColumnName("usluga_id");
            u.Property(x => x.Naziv).HasColumnName("naziv");
            u.Property(x => x.Trajanje).HasColumnName("trajanje").HasColumnType("interval");
            u.Property(x => x.Buffer).HasColumnName("buffer").HasColumnType("interval");
            u.Property(x => x.Aktivno).HasColumnName("aktivno");
            u.Property(x => x.Kreirano).HasColumnName("kreirano").HasColumnType("timestamptz");
            u.Property(x => x.Azurirano).HasColumnName("azurirano").HasColumnType("timestamptz");

            var f = mb.Entity<Frizer>();
            f.ToTable("frizeri");
            f.HasKey(x => x.Id);
            f.Property(x => x.Id).HasColumnName("id");
            f.Property(x => x.SaloonId).HasColumnName("saloon_id");
            f.Property(x => x.Ime).HasColumnName("ime");
            f.Property(x => x.Prezime).HasColumnName("prezime");
            f.Property(x => x.KontaktBroj).HasColumnName("kontakt_broj");
            f.Property(x => x.Slika).HasColumnName("slika");
            f.HasOne(x => x.Saloon)
             .WithMany()
             .HasForeignKey(x => x.SaloonId)
             .HasPrincipalKey(s => s.SaloonId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
