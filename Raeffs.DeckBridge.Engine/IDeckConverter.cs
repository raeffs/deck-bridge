namespace Raeffs.DeckBridge.Engine;

public interface IDeckConverter
{
    Task ConvertDecksAsync(string source, string destination, CancellationToken cancellationToken = default);
}
