using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Raeffs.DeckBridge.Engine;
using Raeffs.DeckBridge.OfflineCli;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

var parser = new Parser(settings => settings.CaseSensitive = false);
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
    var builder = Host.CreateApplicationBuilder();

    builder.Logging
        .ClearProviders()
        .AddSerilog();

    builder.Services
        .AddDeckBridgeEngine(new EngineOptions
        {
            Force = options.Force,
            Combine = options.Combine,
            DefaultLanguage = options.DefaultLanguage,
            ScryfallBulkDataFile = options.ScryfallBulkDataFile,
            DelverLensDataFile = options.DelverLensDataFile
        });

    var host = builder.Build();

    await host.InitializeEngineAsync().ConfigureAwait(false);

    var factory = host.Services.GetRequiredService<IDeckConverterFactory>();
    var converter = factory.CreateConverter(options.InputFormat, options.OutputFormat);
    await converter.ConvertDecksAsync(options.Input, options.Output).ConfigureAwait(false);
}
catch (Exception exception)
{
    await Console.Error.WriteLineAsync("An unexpected error occurred!");
    await Console.Error.WriteLineAsync(exception.ToString());
    Environment.Exit(1);
}
