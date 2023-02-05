using CommandLine;
using CommandLine.Text;
using Flurl;
using Raeffs.DeckBridge.Cli;

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

    var url = options.Endpoint
        .AppendPathSegments("api", "transform")
        .SetQueryParam("outputProvider", options.OutputFormat);

    var content = new MultipartFormDataContent();
    var fileContent = new StreamContent(File.OpenRead(options.InputFile));
    content.Add(fileContent, "file", Path.GetFileName(options.InputFile));

    using var client = new HttpClient();
    using var request = new HttpRequestMessage(HttpMethod.Post, url);
    request.Content = content;

    using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
    response.EnsureSuccessStatusCode();

    if (string.IsNullOrWhiteSpace(options.OutputFile))
    {
        using var stream = await response.Content.ReadAsStreamAsync();
        var outStream = Console.OpenStandardOutput();
        await stream.CopyToAsync(outStream);
        await outStream.FlushAsync();
    }
    else
    {
        using var stream = await response.Content.ReadAsStreamAsync();
        using var outStream = new FileStream(options.OutputFile, FileMode.Create, FileAccess.Write, FileShare.None);
        await stream.CopyToAsync(outStream);
        await outStream.FlushAsync();
    }
}
catch (Exception exception)
{
    await Console.Error.WriteLineAsync("An unexpected error occurred!");
    await Console.Error.WriteLineAsync(exception.ToString());
    Environment.Exit(1);
}

