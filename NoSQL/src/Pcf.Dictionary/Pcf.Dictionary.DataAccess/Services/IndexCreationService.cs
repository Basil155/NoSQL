using Microsoft.Extensions.Hosting;
using Pcf.Dictionary.DataAccess.Models;
using Redis.OM;
using Redis.OM.Searching;

namespace Pcf.Dictionary.DataAccess.Services;

public class IndexCreationService(RedisConnectionProvider provider) : IHostedService
{
    /// <summary>
    /// Checks redis to see if the index already exists, if it doesn't create a new index
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var info = (await provider.Connection.ExecuteAsync("FT._LIST")).ToArray().Select(x => x.ToString());
        if (info.All(x => x != "dictionaryelement-idx"))
        {
            await provider.Connection.CreateIndexAsync(typeof(DictionaryElement));
            var collection =
                (RedisCollection<DictionaryElement>)provider.RedisCollection<DictionaryElement>();
            foreach (var p in Preferences)
            {
                await collection.InsertAsync(new DictionaryElement
                {
                    DictionaryCode = "Preference",
                    Id = p.Key.ToString(),
                    Name = p.Value
                });
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public static Dictionary<Guid, string> Preferences => new Dictionary<Guid, string>()
    {

        {
            Guid.Parse("ef7f299f-92d7-459f-896e-078ed53ef99c"),
            "Театр"
        },
        {
            Guid.Parse("c4bda62e-fc74-4256-a956-4760b3858cbd"),
            "Семья"
        },
        {
            Guid.Parse("76324c47-68d2-472d-abb8-33cfa8cc0c84"),
            "Дети"
        },
        {
            Guid.Parse("c33f832a-eeec-4890-87c0-c826710ec393"),
            "Преференция"
        }
    };
}