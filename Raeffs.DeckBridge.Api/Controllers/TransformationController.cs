using Microsoft.AspNetCore.Mvc;
using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Api.Controllers;

[Route("api/transform")]
[ApiController]
public class TransformationController : ControllerBase
{
    private readonly IDeckReader<Card> _reader;
    private readonly DeckWriterSelector _writerSelector;

    public TransformationController(IDeckReader<Card> reader, DeckWriterSelector writerSelector)
    {
        _reader = reader;
        _writerSelector = writerSelector;
    }

    [HttpPost]
    public async Task PostAsync([FromForm] IFormFile file, [FromQuery] DeckWriterProvider? outputProvider, CancellationToken cancellationToken)
    {
        var writer = _writerSelector.SelectWriter(outputProvider);

        var path = Path.GetTempFileName();

        using (var stream = System.IO.File.Create(path))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        Response.ContentType = "text/csv";
        await writer.WriteDeckAsync(Response.Body, _reader.ReadDeckAsync(path, cancellationToken), cancellationToken);
    }
}
