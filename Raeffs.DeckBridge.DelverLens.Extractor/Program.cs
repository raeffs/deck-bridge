using Raeffs.DeckBridge.DelverLens;
using System.Data.Common;
using System.Data.SQLite;
using System.Text.Json;

var source = args.First();
var destination = args.Skip(1).First();

var connection = new SQLiteConnection($"URI=file:{source};mode=ReadOnly");
connection.Open();

var command = new SQLiteCommand("SELECT * FROM cards", connection);
var reader = command.ExecuteReader();

var schema = reader.GetColumnSchema();
var idIndex = schema.GetColumnIndex(new("_id", 0));
var scryfallIdIndex = schema.GetColumnIndex(new("scryfall_id", 12));

var mappings = new List<Mapping>();

while (reader.Read())
{
    mappings.Add(new Mapping
    {
        Id = reader.GetInt32(idIndex),
        ScryfallId = reader.GetGuid(scryfallIdIndex)
    });
}

var file = File.Create(destination);

JsonSerializer.Serialize(file, mappings, new JsonSerializerOptions
{
    WriteIndented = true
});

file.Flush();

Console.WriteLine("Finished");
Console.ReadLine();
