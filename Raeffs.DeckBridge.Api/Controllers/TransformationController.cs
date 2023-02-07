using Microsoft.AspNetCore.Mvc;
using Raeffs.DeckBridge.Common;
using Raeffs.DeckBridge.Engine;

namespace Raeffs.DeckBridge.Api.Controllers;

[Route("api/transform")]
[ApiController]
public class TransformationController : ControllerBase
{
    private readonly IDeckReaderCollection _readers;
    private readonly IDeckWriterCollection _writers;

    public TransformationController(IDeckReaderCollection readers, IDeckWriterCollection writers)
    {
        _readers = readers;
        _writers = writers;
    }

    [HttpPost]
    public async Task PostAsync([FromForm] IFormFile file, [FromQuery] DeckReaderProvider? inputProvider, [FromQuery] DeckWriterProvider? outputProvider, CancellationToken cancellationToken)
    {
        var reader = _readers.Find(inputProvider);
        var writer = _writers.Find(outputProvider);

        var path = Path.GetTempFileName();

        using (var stream = System.IO.File.Create(path))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        Response.ContentType = "text/csv";
        await writer.WriteDeckAsync(Response.Body, reader.ReadDeckAsync(path, cancellationToken), cancellationToken);
    }
}
