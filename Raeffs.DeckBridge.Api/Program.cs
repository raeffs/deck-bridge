using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Deckstats;
using Raeffs.DeckBridge.DelverLens;
using Raeffs.DeckBridge.Scryfall;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDelverLens(builder.Configuration.GetSection("DelverLens"))
    .AddScryfall(builder.Configuration.GetSection("Scryfall"))
    .AddDeckstats();

builder.Services
    .AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<IAppInitializer>();
    await initializer.InitializeAsync();
}

app.MapControllers();

await app.RunAsync();