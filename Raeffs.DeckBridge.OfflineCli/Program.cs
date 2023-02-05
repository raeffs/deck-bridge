using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Deckstats;
using Raeffs.DeckBridge.DelverLens;
using Raeffs.DeckBridge.Generic;
using Raeffs.DeckBridge.OfflineCli;
using Raeffs.DeckBridge.Scryfall;

var parser = new Parser();
var result = parser.ParseArguments<Options>(args);

if (result.Errors.Any())
{
    var helpText = HelpText.AutoBuild(result);
    helpText.AddEnumValuesToHelpText = true;
    helpText.AddOptions(result);
    await Console.Error.WriteAsync(helpText);
    Environment.Exit(1);
}

try
{
    var options = result.Value;

    if (!File.Exists(options.ScryfallBulkDataFile))
    {
        await Console.Error.WriteLineAsync($"The scryfall bulk data file '{options.ScryfallBulkDataFile}' does not exist or cannot be accessed!");
        Environment.Exit(1);
    }

    if (!File.Exists(options.DelverLensDataFile))
    {
        await Console.Error.WriteLineAsync($"The delver lens data file '{options.DelverLensDataFile}' does not exist or cannot be accessed!");
        Environment.Exit(1);
    }

    if (!File.Exists(options.InputFile))
    {
        await Console.Error.WriteLineAsync($"The file '{options.InputFile}' does not exist or cannot be accessed!");
        Environment.Exit(1);
    }

    var outputToFile = !string.IsNullOrWhiteSpace(options.OutputFile);
    if (outputToFile && File.Exists(options.OutputFile) && !options.Force)
    {
        await Console.Error.WriteLineAsync($"The file '{options.OutputFile}' does already exist!");
        Environment.Exit(1);
    }

    var builder = Host.CreateApplicationBuilder();

    var config = new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            { "Scryfall:BulkDataFile", options.ScryfallBulkDataFile },
            { "DelverLens:DataFile", options.DelverLensDataFile }
        })
        .Build();

    builder.Services
        .AddGeneric()
        .AddDelverLens(config.GetSection("DelverLens"))
        .AddScryfall(config.GetSection("Scryfall"))
        .AddDeckstats();

    builder.Services
        .AddTransient<DeckWriterSelector>();

    var host = builder.Build();

    var initializer = host.Services.GetRequiredService<IAppInitializer>();
    await initializer.InitializeAsync();

    var reader = host.Services.GetRequiredService<IDeckReader<Card>>();
    var writerSelector = host.Services.GetRequiredService<DeckWriterSelector>();
    var writer = writerSelector.SelectWriter(options.OutputFormat);

    if (outputToFile)
    {
        await using var outStream = new FileStream(options.OutputFile, FileMode.Create, FileAccess.Write, FileShare.None);
        await writer.WriteDeckAsync(outStream, reader.ReadDeckAsync(options.InputFile));
    }
    else
    {
        await using var outStream = Console.OpenStandardOutput();
        await writer.WriteDeckAsync(outStream, reader.ReadDeckAsync(options.InputFile));
    }
}
catch (Exception exception)
{
    await Console.Error.WriteLineAsync("An unexpected error occurred!");
    await Console.Error.WriteLineAsync(exception.ToString());
    Environment.Exit(1);
}
