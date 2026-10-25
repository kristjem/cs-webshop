using Microsoft.EntityFrameworkCore;
using Webshop.Api.Data;

// builder.Services er ASP.NET Cores innebygde Dependency Injection-container
// Her registrerer vi WebshopDbContext som en tjeneste, slik at den kan injiseres i kontrollerne våre
// Når vi senere lager en ProdukterController, kan vi få databasen levert gjennom konstruktøren via Dependency Injection
/* F.eks.:
public ProdukterController(WebshopDbContext db)
{
    _db = db;
} */
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<WebshopDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Webshop")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
