using CommandLine;
using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Cli;

internal record Options
{
    [Option('i', "inFile", Required = true, HelpText = "Convert the specified file.")]
    public string InputFile { get; init; } = string.Empty;

    [Option('o', "outFile", HelpText = "Write the output to the specified file. If not specified, the output will be written to standard output.")]
    public string OutputFile { get; init; } = string.Empty;

    [Option("force", HelpText = "Overwrite the output file if it already exists.")]
    public bool Force { get; init; }

    [Option('f', "outFormat", Default = DeckWriterProvider.Generic, HelpText = "Specify the output format.")]
    public DeckWriterProvider OutputFormat { get; init; }

    [Option("endpoint", Hidden = true)]
    public string Endpoint { get; init; } = "https://localhost:5050";

    /*
    [Usage]
    public static IEnumerable<Example> Examples => new List<Example>
    {
        new("Convert a file to a generic deck list", new Options { InputFile = "mydeck.dlens" })
    };
    */
}
