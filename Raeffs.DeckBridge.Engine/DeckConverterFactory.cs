using Microsoft.Extensions.DependencyInjection;
using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Engine;

internal class DeckConverterFactory : IDeckConverterFactory
{
    private readonly IServiceProvider _services;

    public DeckConverterFactory(IServiceProvider services)
    {
        _services = services;
    }

    public IDeckConverter CreateConverter(DeckReaderProvider from, DeckWriterProvider to)
    {
        return new DeckConverter(
            GetCollectionReader(from),
            GetReader(from),
            GetWriter(to)
        );
    }

    private IDeckCollectionReader GetCollectionReader(DeckReaderProvider provider)
    {
        return _services.GetRequiredService<IEnumerable<IDeckCollectionReader>>()
            .SingleOrDefault(x => x.ProviderName == provider)
            ?? new NullDeckCollectionReader(provider);
    }

    private IDeckReader<Card> GetReader(DeckReaderProvider provider)
    {
        return _services.GetRequiredService<IEnumerable<IDeckReader<Card>>>()
            .SingleOrDefault(x => x.ProviderName == provider)
            ?? new NullDeckReader(provider);
    }

    private IDeckWriter GetWriter(DeckWriterProvider provider)
    {
        return _services.GetRequiredService<IEnumerable<IDeckWriter>>()
            .SingleOrDefault(x => x.ProviderName == provider)
            ?? new NullDeckWriter(provider);
    }
}
