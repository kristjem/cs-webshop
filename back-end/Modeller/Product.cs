namespace Webshop.Api.Modeller;

public class Produkt
{
    public int Id { get; set; } // EF Core gjenkjenner 'Id' som primærnøkkel automatisk

    public string Navn { get; set; } = string.Empty;

    public string Beskrivelse { get; set; } = string.Empty;

    public decimal Pris { get; set; } // priser er av typen decimal for å unngå flyttallsavrundingsfeil

    public int Lagerbeholdning { get; set; }

    public bool ErAktiv { get; set; } = true;
}