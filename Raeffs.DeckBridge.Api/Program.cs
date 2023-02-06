using Raeffs.DeckBridge.Engine;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDeckBridgeEngine(builder.Configuration);

builder.Services
    .AddControllers();

var app = builder.Build();

await app.InitializeEngineAsync().ConfigureAwait(false);

app.MapControllers();

await app.RunAsync();
