using Redis.OM.Modeling;

namespace Pcf.Dictionary.DataAccess.Models;

[Document(StorageType = StorageType.Json, Prefixes = new []{"DictionaryElement"})]
public class DictionaryElement
{
    // Id Field, also indexed, marked as nullable to pass validation
    [RedisIdField] [Indexed]public string? Id { get; set; } = Guid.NewGuid().ToString();

    // Indexed for exact text matching
    [Indexed] public string? DictionaryCode { get; set; }
    [Indexed] public string? Code { get; set; }

    //Indexed for Full Text matches
    [Searchable] public string? Name { get; set; }
}