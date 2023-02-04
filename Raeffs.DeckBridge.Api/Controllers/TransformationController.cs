using Microsoft.AspNetCore.Mvc;
using Raeffs.DeckBridge.Common;

namespace Raeffs.DeckBridge.Api.Controllers;

[Route("api/transform")]
[ApiController]
public class TransformationController : ControllerBase
{
    private readonly IDeckReader<Card> _reader;
    private readonly IDeckWriter _writer;

    public TransformationController(IDeckReader<Card> reader, IDeckWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    [HttpPost]
    public async Task PostAsync([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        var path = Path.GetTempFileName();

        using (var stream = System.IO.File.Create(path))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        Response.ContentType = "text/csv";
        await _writer.WriteDeckAsync(Response.Body, _reader.ReadDeckAsync(path, cancellationToken), cancellationToken);
    }
}
