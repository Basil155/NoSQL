using Redis.OM;
using Pcf.Dictionary.DataAccess.Models;
using Redis.OM.Searching;

namespace Pcf.Dictionary.DataAccess.Repositories;

public class DictionaryElementRepository(RedisConnectionProvider provider)
{
    private readonly RedisCollection<DictionaryElement> _collection =
        (RedisCollection<DictionaryElement>)provider.RedisCollection<DictionaryElement>();


    public async Task<IList<DictionaryElement>> GetAllAsync(string dictionaryCode)
    {
        return await _collection.Where(x => x.DictionaryCode == dictionaryCode).ToListAsync();
    }

    public async Task<DictionaryElement?> GetAsync(string id)
    {
        return await _collection.FindByIdAsync(id);
    }

    public async Task<DictionaryElement> AddAsync(DictionaryElement element)
    {
        await _collection.InsertAsync(element);
        return element;
    }

    public async Task<DictionaryElement?> SetAsync(DictionaryElement element)
    {
        if (element.Id == null) return null;

        var existingElement = await _collection.FindByIdAsync(element.Id);

        if (existingElement == null)
        {
            await _collection.InsertAsync(element);
            return element;
        }

        await _collection.UpdateAsync(element);
        return element;

    }

}