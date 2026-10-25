# CS Webshop

En fullstack nettbutikk utviklet med **ASP.NET Core, C#, Next.js, TypeScript og SQL Server**.

Prosjektet demonstrerer utvikling av en moderne webapplikasjon med REST API, relasjonsdatabase, autentisering, produktadministrasjon, handlekurv og ordrebehandling.

Utviklingen skjer stegvis, med vekt på ryddig arkitektur, vedlikeholdbarhet, sikkerhet og praktisk bruk av rammeverkene.

**Prosjektstatus:** Tidlig utviklingsfase. ASP.NET Core API, containerbasert utviklingsmiljø, SQL Server-integrasjon og første migrasjon med Entity Framework Core er på plass. Øvrig funksjonalitet er planlagt.

## 1. Teknologistakk

| Komponent | Teknologi |
|---|---|
| Backend | C# / ASP.NET Core Web API |
| Kjøremiljø | .NET 9 |
| Frontend | Next.js / React (planlagt) |
| Frontend-språk | TypeScript (planlagt) |
| Database | SQL Server 2022 Developer |
| ORM | Entity Framework Core 9 |
| Autentisering | ASP.NET Core Identity (planlagt) |
| API | REST / JSON |
| Utviklingsmiljø | VS Code / Dev Containers |
| Containerisering | Docker / Docker Compose |
| Versjonskontroll | Git |

Prosjektet bruker foreløpig .NET 9. Støtteperioder og behov for oppgradering må vurderes før eventuell produksjonssetting.

## 2. Arkitektur

Applikasjonen er organisert i ett Git-repository med separat frontend og backend.

```text
                   Nettleser
                       |
                       v
                Next.js / React
                   Frontend
                  (planlagt)
                       |
                    HTTP/JSON
                       |
                       v
                 ASP.NET Core
                    Web API
                       |
                       v
               Entity Framework Core
                       |
                       v
                   SQL Server
```

### Backend

Backend er en ASP.NET Core Web API-applikasjon med controllerbasert ruting.

Den skal håndtere forretningslogikk, validering, autentisering, autorisasjon og databasetilgang.

Backend inneholder foreløpig en EF Core-databasekontekst og en enkel produktmodell.

### Frontend

Frontend skal utvikles med Next.js, React og TypeScript.

Den skal kommunisere med backend gjennom API-et, uten direkte tilgang til databasen.

### Database

SQL Server brukes som relasjonsdatabase.

Entity Framework Core håndterer objekt-relasjonell mapping (ORM), modellkonfigurasjon og databasemigrasjoner.

Den første migrasjonen oppretter tabellen `Produkter`, basert på C#-klassen `Produkt`.

### Applikasjonsdesign

Backend bygges som en monolittisk applikasjon med én database. Frontend og backend kjøres som separate prosesser under utvikling.

Arkitekturen prioriterer tydelig ansvarsfordeling og enkle løsninger fremfor unødvendige abstraksjoner og mikrotjenester.

## 3. Prosjektstruktur

```text
cs-webshop/
├── .devcontainer/
│   ├── Dockerfile
│   ├── compose.yaml
│   ├── devcontainer.json
│   ├── .env.example
│   └── .env                  # Lokal, ikke versjonert
│
├── back-end/
│   ├── Controllers/
│   ├── Data/
│   │   └── WebshopDbContext.cs
│   ├── Migrations/
│   │   ├── *_OpprettProdukter.cs
│   │   ├── *_OpprettProdukter.Designer.cs
│   │   └── WebshopDbContextModelSnapshot.cs
│   ├── Modeller/
│   │   └── Produkt.cs
│   ├── Properties/
│   ├── Program.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── Webshop.Api.csproj
│
├── front-end/                # Planlagt Next.js-applikasjon
├── tests/                    # Planlagte automatiserte tester
│
├── .gitignore
├── Webshop.sln
└── README.md
```

Flere mapper i backend, blant annet for DTO-er og tjenester, opprettes etter hvert som applikasjonen utvikles.

## 4. Utviklingsmiljø

Utviklingsmiljøet bruker VS Code, Dev Containers og Docker Compose.

Docker Compose definerer to tjenester:

| Tjeneste | Beskrivelse |
|---|---|
| `app` | Utviklingscontainer med .NET SDK og Node.js |
| `sqlserver` | Databasecontainer med SQL Server 2022 Developer |

Prosjektmappen monteres i utviklingscontaineren som `/workspace`.

SQL Server bruker et navngitt Docker-volum, slik at databasedata beholdes ved omstart og gjenskaping av containere, så lenge volumet ikke slettes.

### Forutsetninger

- Git
- Visual Studio Code
- Dev Containers-utvidelsen
- Docker med Docker Compose
- WSL2 med en Linux-distribusjon ved utvikling på Windows

.NET SDK og Node.js er inkludert i utviklingscontaineren.

### Kom i gang

1. Klon repositoryet.
2. Opprett `.devcontainer/.env` fra eksempelfilen.
3. Angi et sterkt passord for SQL Server.
4. Åpne prosjektet i VS Code.
5. Bygg Docker-imaget for utviklingscontaineren.
6. Velg **Dev Containers: Reopen in Container**.
7. Kontroller at både `app` og `sqlserver` kjører.
8. Kjør databasemigrasjonene som beskrevet i kapittel 6.

Opprett lokal miljøfil:

```bash
cp .devcontainer/.env.example .devcontainer/.env
```

Rediger deretter `.devcontainer/.env`:

```dotenv
MSSQL_SA_PASSWORD=ReplaceWithStrongPassword123!
```

Erstatt eksempelpassordet med ditt eget.

**Viktig:** `.env` skal aldri committes til Git. `.env.example` skal versjoneres.

### Bygging av utviklingsimage

Den nåværende Docker Compose-konfigurasjonen bruker et lokalt bygget image:

```yaml
services:
  app:
    image: devcontainer-app:latest
```

Bygg imaget fra rotmappen i prosjektet:

```bash
docker build \
  -t devcontainer-app:latest \
  -f .devcontainer/Dockerfile \
  .devcontainer
```

Dette er foreløpig en løsning på et kompatibilitetsproblem med byggeprosessen i Dev Containers. Oppsettet kan forenkles senere.

### Nettverk mellom containere

Begge tjenestene skal kjøre i samme Docker Compose-prosjekt.

Innenfor Docker-nettverket kobler backend seg til SQL Server via vertsnavnet `sqlserver` og port `1433`.

SQL Server-porten er også eksponert på vertsmaskinens loopback-adresse, slik at databasen kan administreres med lokale verktøy.

## 5. Kjøre backend

ASP.NET Core-prosjektet ligger i `back-end/` og er registrert i `Webshop.sln`.

### Kontroller .NET SDK

```bash
dotnet --version
```

### Bygg løsningen

Fra prosjektets rotmappe:

```bash
dotnet build Webshop.sln
```

### Start API-et

```bash
cd back-end
dotnet run --urls http://0.0.0.0:5000
```

API-et blir tilgjengelig på:

`http://localhost:5000`

### Eksempelendepunkt

Den opprinnelige ASP.NET Core-malen inneholder:

`GET http://localhost:5000/weatherforecast`

Dette endepunktet returnerer eksempeldata som JSON og kan brukes til å kontrollere at API-et fungerer.

Det skal senere erstattes av endepunkter for nettbutikken.

## 6. Database og Entity Framework Core

SQL Server 2022 Developer kjører i en separat Docker-container.

Ved lokal utvikling er databasen tilgjengelig på port `1433`, begrenset til vertsmaskinens loopback-adresse.

### Koble til med SQL Server Management Studio

| Innstilling | Verdi |
|---|---|
| Server | `localhost,1433` |
| Autentisering | SQL Server Authentication |
| Brukernavn | `sa` |
| Passord | Verdien i `.devcontainer/.env` |
| Database | `Webshop`, etter migrering |

I utviklingsmiljøet kan det være nødvendig å aktivere tillit til serversertifikatet.

### Tilkobling fra backend

Backend får tilkoblingsstrengen gjennom miljøvariabelen `ConnectionStrings__Webshop`, definert i Docker Compose.

```yaml
environment:
  ConnectionStrings__Webshop: "Server=sqlserver,1433;Database=Webshop;User Id=sa;Password=${MSSQL_SA_PASSWORD};TrustServerCertificate=True"
```

ASP.NET Core tolker doble understreker (`__`) i miljøvariabelnavn som skilletegn mellom konfigurasjonsseksjoner.

Tilkoblingsstrengen hentes i `Program.cs` med:

```csharp
builder.Configuration.GetConnectionString("Webshop")
```

`WebshopDbContext` registreres gjennom dependency injection og konfigureres til å bruke SQL Server.

Passord og andre hemmeligheter skal ikke hardkodes eller lagres i Git.

Bruk av `sa` og et sertifikat som er klarert uten validering, er begrenset til lokal utvikling. Ved produksjonssetting skal tilgangen begrenses og tilkoblingen sikres.

### Entity Framework Core

Den første databaseintegrasjonen består av:

- **`Produkt`** – C#-klasse som representerer et produkt.
- **`WebshopDbContext`** – databasekontekst med `DbSet<Produkt> Produkter`.
- **SQL Server-provider** – lar EF Core kommunisere med SQL Server.
- **Fluent API-konfigurasjon** – angir at `Pris` skal lagres som `decimal(18,2)`.
- **`OpprettProdukter`** – migrasjon som oppretter det første databaseskjemaet.

Produktmodellen har følgende egenskaper:

| Egenskap | SQL Server-datatype |
|---|---|
| `Id` | `int`, automatisk generert primærnøkkel |
| `Navn` | `nvarchar(max)` |
| `Beskrivelse` | `nvarchar(max)` |
| `Pris` | `decimal(18,2)` |
| `Lagerbeholdning` | `int` |
| `ErAktiv` | `bit` |

Flere begrensninger, indekser, relasjoner og modeller legges til etter hvert.

### Databasemigrasjoner

Entity Framework Core bruker migrasjoner for å holde oversikt over endringer i databaseskjemaet.

Den første migrasjonen er opprettet og kjørt mot SQL Server.

For et nytt utviklingsmiljø kjøres eksisterende migrasjoner fra utviklingscontaineren:

```bash
cd /workspace/back-end
dotnet ef database update
```

EF Core CLI-verktøyet (`dotnet-ef`, versjon 9) må være installert.

Installer ved behov:

```bash
dotnet tool install --global dotnet-ef --version "9.*"
```

Etter endringer i EF Core-modellene kan en ny migrasjon opprettes med:

```bash
dotnet ef migrations add Migrasjonsnavn
```

Kjør deretter migrasjonen mot databasen:

```bash
dotnet ef database update
```

Tabellen `__EFMigrationsHistory` holder oversikt over hvilke migrasjoner som er utført.

Migrasjoner kjøres foreløpig manuelt under utvikling.

### Kontroller databasen

Koble til SQL Server med SSMS og kjør:

```sql
USE Webshop;

SELECT * FROM dbo.Produkter;

SELECT * FROM dbo.__EFMigrationsHistory;
```

Tabellen `Produkter` er tom frem til produkter legges inn.

### Offisiell Microsoft-dokumentasjon

- [Entity Framework Core – oversikt](https://learn.microsoft.com/en-us/ef/core/)
- [Konfigurering av DbContext](https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/)
- [Databasemigrasjoner](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [EF Core CLI-verktøy](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)
- [SQL Server-provider](https://learn.microsoft.com/en-us/ef/core/providers/sql-server/)

## 7. Domenemodell

Nettbutikken skal etter planen støtte følgende sentrale entiteter:

| Entitet | Ansvar | Status |
|---|---|---|
| Bruker | Brukerkonto, profil og autentisering | Planlagt |
| Rolle | Tilgangsstyring | Planlagt |
| Medlemskap | Medlemsnivåer og rabatter | Planlagt |
| Produkt | Produktinformasjon, pris og lagerbeholdning | Første modell implementert |
| Merke | Produktmerker | Planlagt |
| Kategori | Produktkategorisering | Planlagt |
| Ordre | Handlekurv og bestillinger | Planlagt |
| Ordrelinje | Produkter, antall og priser i en ordre | Planlagt |

### Vurderinger rundt domenemodellen

**Handlekurv:** En handlekurv kan representeres som en ordre med status for utkast eller handlekurv. Dette kan redusere duplisering mellom handlekurv og ordre.

**Historiske priser:** Ordrelinjer bør lagre pris og rabatt på bestillingstidspunktet, slik at gamle ordrer forblir korrekte selv om produktprisene endres.

**Lagerbeholdning:** Ved fullføring av ordre må lagerbeholdningen kontrolleres, også når flere kunder bestiller samtidig.

**Sletting av produkter:** Myk sletting (soft delete) kan brukes for å skjule produkter uten å ødelegge historiske ordrereferanser.

**Dataintegritet:** Unike begrensninger, fremmednøkler og andre databasebegrensninger skal bidra til konsistente data.

Endelig domenemodell og relasjoner fastsettes under implementeringen.

## 8. Autentisering og autorisasjon

ASP.NET Core Identity er planlagt brukt til bruker- og rolleadministrasjon.

Løsningen skal støtte:

- Registrering med unik e-postadresse.
- Sikker hashing av passord.
- Innlogging og utlogging.
- Rollebasert tilgangsstyring.
- Beskyttede administrative funksjoner.

Autentiseringen mellom Next.js og ASP.NET Core skal utformes slik at sensitive opplysninger og tokens ikke eksponeres unødvendig.

Autentisering er foreløpig ikke implementert.

## 9. Planlagt API-funksjonalitet

API-et skal etter planen håndtere:

- Brukerregistrering og autentisering.
- Administrasjon av brukere og roller.
- Produktadministrasjon.
- Merker og kategorier.
- Produktsøk og filtrering.
- Medlemsnivåer og rabatter.
- Handlekurv.
- Ordrebehandling.
- Ordrehistorikk.
- Lagerstyring.
- Initialisering av testdata.

Ruter, DTO-er, validering og responsformater defineres etter hvert som funksjonaliteten utvikles.

OpenAPI-dokumentasjon er også planlagt.

## 10. Initialisering av eksempeldata

Det skal utvikles en repeterbar mekanisme for å fylle databasen med eksempeldata.

Den skal kunne:

- Opprette forhåndsdefinerte roller og medlemsnivåer.
- Opprette eksempelkategorier, merker og produkter.
- Unngå duplikater ved gjentatt kjøring.
- Begrenses til utviklingsmiljøet eller sikres på annen egnet måte.

Denne funksjonaliteten er foreløpig ikke implementert.

## 11. Testing

Automatiserte tester skal legges i `tests/`.

Planlagt testdekning omfatter:

- Validering av API-forespørsler.
- Produkt- og kategorihåndtering.
- Autentisering og autorisasjon.
- Handlekurv.
- Ordrebehandling.
- Lagerstyring.
- Rabattberegninger.
- Databaseintegrasjon.

Testrammeverk og prosjektstruktur velges under implementeringen.

## 12. Utviklingsprinsipper

Prosjektet bygger på følgende prinsipper:

**Stegvis utvikling:** Funksjonalitet implementeres og testes i små, oversiktlige steg.

**Bruk av etablerte rammeverk:** Foretrekk funksjonalitet som allerede finnes i ASP.NET Core og Entity Framework Core, fremfor unødvendige egenutviklede løsninger.

**Tydelig ansvarsfordeling:** HTTP-håndtering, forretningsregler og databasetilgang skal ha tydelige ansvarsområder.

**Vedlikeholdbarhet:** Lesbar kode og forståelige abstraksjoner prioriteres.

**Sikkerhet:** Validering, autorisasjon, sikker håndtering av hemmeligheter og passord skal ivaretas.

**Dataintegritet:** Databasebegrensninger og transaksjoner brukes der det er nødvendig.

**Navngiving:** Norske domenebegreper brukes der det er naturlig. Etablerte C#/.NET-konvensjoner, rammeverksnavn og standardiserte tekniske betegnelser beholdes.

**Testbarhet:** Viktige forretningsregler skal kunne verifiseres med automatiserte tester.

## 13. Utviklingsplan

| Fase | Beskrivelse | Status |
|---|---|---|
| 1 | Git, Docker og Dev Container | Ferdig |
| 2 | Oppsett av ASP.NET Core Web API | Ferdig |
| 3 | Oppsett av Next.js og TypeScript | Planlagt |
| 4 | SQL Server-integrasjon og EF Core | Ferdig |
| 5 | Første produktmodell og migrasjon | Ferdig |
| 6 | Eksempeldata og produkt-API | Planlagt |
| 7 | Autentisering og autorisasjon | Planlagt |
| 8 | Handlekurv, rabatter og ordrer | Planlagt |
| 9 | Frontend-integrasjon og brukergrensesnitt | Planlagt |
| 10 | Automatisert testing og dokumentasjon | Planlagt |

## 14. Nåværende status

Det containerbaserte utviklingsmiljøet fungerer, og ASP.NET Core API-et kan bygges og kjøres.

SQL Server 2022 og Entity Framework Core 9 er konfigurert. Den første produktmodellen og migrasjonen `OpprettProdukter` er implementert, og databasen `Webshop` er opprettet.

Produkt-API, frontend, autentisering, handlekurv og ordrebehandling gjenstår.

---

**CS Webshop** – Fullstack nettbutikk under utvikling.