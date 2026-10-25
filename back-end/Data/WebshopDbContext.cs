using Microsoft.EntityFrameworkCore; // Importerer Entity Framework Core for å kunne definere DbContext og DbSet
using Webshop.Api.Modeller; // Importerer modellen for Produkt, som vi har definert i Modeller-mappen

namespace Webshop.Api.Data; // Definerer navnerommet for databasen, som gjør det enklere å organisere koden vår

// Vi arver fra EF Cores DbContext. Dermed får klassen funksjonalitet for å hente, legge til, oppdatere og slette data
// DbContextOptions<WebshopDbContext> brukes for å konfigurere databasen, for eksempel hvilken databaseleverandør som skal brukes og tilkoblingsstrenger
public class WebshopDbContext : DbContext
{
    public WebshopDbContext(DbContextOptions<WebshopDbContext> options) // Konstruktør som tar inn konfigurasjonsalternativer for databasen
        : base(options) // Kaller basiskonstruktøren i DbContext med de gitte alternativene
    {
    }

    public DbSet<Produkt> Produkter { get; set; } // Representerer tabellen for produkter i databasen
        = null!;

    // Fluent API brukes for å konfigurere modellene våre i databasen, for eksempel presisjon på desimaler,
    // det holder databasekonfigurasjonen samlet i DbContext
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Produkt>()
            .Property(p => p.Pris)
            .HasPrecision(18, 2);
    }
}