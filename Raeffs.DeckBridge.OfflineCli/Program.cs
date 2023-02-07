using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Raeffs.DeckBridge.Engine;
using Raeffs.DeckBridge.OfflineCli;

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

    builder.Services
        .AddDeckBridgeEngine(new EngineOptions
        {
            ScryfallBulkDataFile = options.ScryfallBulkDataFile,
            DelverLensDataFile = options.DelverLensDataFile
        });

    var host = builder.Build();

    await host.InitializeEngineAsync().ConfigureAwait(false);

    var readers = host.Services.GetRequiredService<IDeckReaderCollection>();
    var writers = host.Services.GetRequiredService<IDeckWriterCollection>();

    var reader = readers.Find(options.InputFormat);
    var writer = writers.Find(options.OutputFormat);

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
