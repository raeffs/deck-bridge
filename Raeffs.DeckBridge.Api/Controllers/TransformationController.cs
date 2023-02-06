using Microsoft.AspNetCore.Mvc;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Engine;

namespace Raeffs.DeckBridge.Api.Controllers;

[Route("api/transform")]
[ApiController]
public class TransformationController : ControllerBase
{
    private readonly IDeckReader<Card> _reader;
    private readonly IDeckWriterCollection _writers;

    public TransformationController(IDeckReader<Card> reader, IDeckWriterCollection writers)
    {
        _reader = reader;
        _writers = writers;
    }

    [HttpPost]
    public async Task PostAsync([FromForm] IFormFile file, [FromQuery] DeckWriterProvider? outputProvider, CancellationToken cancellationToken)
    {
        var writer = _writers.Find(outputProvider);

        var path = Path.GetTempFileName();

        using (var stream = System.IO.File.Create(path))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        Response.ContentType = "text/csv";
        await writer.WriteDeckAsync(Response.Body, _reader.ReadDeckAsync(path, cancellationToken), cancellationToken);
    }
}
