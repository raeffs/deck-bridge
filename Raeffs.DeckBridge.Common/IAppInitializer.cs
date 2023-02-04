namespace Raeffs.DeckBridge.Common;

public interface IAppInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
}
