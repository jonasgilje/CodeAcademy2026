using Kaffebar.Models;
using Kaffebar.Storage;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Registrer OpenAPI-tjenestene. Disse genererer spesifikasjonen
// automatisk basert på endepunktene og typene i prosjektet (code-first).
builder.Services.AddOpenApi();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddProblemDetails();
builder.Services.AddSingleton<IOrderStore, InMemoryOrderStore>();

var app = builder.Build();

app.UseStatusCodePages();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    // Eksponerer den genererte spesifikasjonen på /openapi/v1.json
    app.MapOpenApi();

    // Scalar gir et moderne, interaktivt UI på /scalar/v1
    // for å utforske og teste API-et.
    app.MapScalarApiReference();
}

// --- Hello Coffee --------------------------------------------------------
// Et minimal API-endepunkt som returnerer en hardkodet kaffemeny.
// Dette er utgangspunktet for workshopen. Bygg videre herfra!
app.MapGet("/menu", () => new[]
{
    new Coffee(Guid.NewGuid(), "Kaffe Latte", 48.50m),
    new Coffee(Guid.NewGuid(), "Cappuccino", 45.00m),
    new Coffee(Guid.NewGuid(), "Espresso", 35.00m)
})
.WithName("GetMenu")
.WithSummary("Hent kaffemeny")
.WithDescription("Returnerer en liste over alle tilgjengelige kaffedrikker i kaffebaren.");


// app.MapPost("/orders", (OrderRequest orderRequest) =>
// {
//     return Results.Created($"/orders/{orderRequest.CoffeeId}", new OrderResponse(Guid.NewGuid(), orderRequest.CoffeeId));
// })
// .WithName("PlaceOrder")
// .WithSummary("Legg inn en bestilling")
// .WithDescription("Legg inn en bestilling for en kaffe ved å sende en POST-forespørsel med ID-en til kaffen du ønsker å bestille.");

app.Run();

// DTO-er kan ligge i Program.cs når prosjektet er lite.
// Etter hvert er det ryddig å flytte dem til egne filer i en Models-mappe.



