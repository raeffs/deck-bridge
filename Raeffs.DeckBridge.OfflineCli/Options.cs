using CommandLine;
using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.OfflineCli;

internal record Options
{
    [Option('i', "in", Required = true, HelpText = "Convert the specified file or folder.")]
    public string Input { get; init; } = string.Empty;

    [Option("from", Default = DeckReaderProvider.DelverLens, HelpText = "Specify the input format.")]
    public DeckReaderProvider InputFormat { get; init; }

    [Option('o', "out", HelpText = "Write the output to the specified file or folder. If not specified, the output will be written to standard output.")]
    public string Output { get; init; } = string.Empty;

    [Option("to", Default = DeckWriterProvider.Generic, HelpText = "Specify the output format.")]
    public DeckWriterProvider OutputFormat { get; init; }

    [Option("force", HelpText = "Overwrite the output files if they already exists.")]
    public bool Force { get; init; }

    [Option("delverLensDataFile", Default = "delverlens-mappings.json")]
    public string DelverLensDataFile { get; init; } = "dlens.db";

    [Option("scryfallBulkDataFile", Default = "scryfall.json")]
    public string ScryfallBulkDataFile { get; init; } = "scryfall.json";
}
