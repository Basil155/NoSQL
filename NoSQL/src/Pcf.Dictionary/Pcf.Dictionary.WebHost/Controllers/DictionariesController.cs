using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pcf.Dictionary.DataAccess.Models;
using Pcf.Dictionary.DataAccess.Repositories;
using Pcf.Dictionary.WebHost.Models;

namespace Pcf.Dictionary.WebHost.Controllers;

/// <summary>
/// Справочники
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class DictionariesController(DictionaryElementRepository repository) : ControllerBase
{
    /// <summary>
    /// Получить список элементов справочника
    /// </summary>
    /// <param name="dictionary">Наименование справочника, например <example>Preferences</example></param>
    /// <returns></returns>
    [HttpGet("{dictionary}")]
    public async Task<List<DictionaryElementGetDto>> GetElementsAsync(string dictionary)
    {
        var elements = await repository.GetAllAsync(dictionary);

        return elements.Select(x => new DictionaryElementGetDto
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code
            })
            .ToList();
    }

    /// <summary>
    /// Получить элемент справочника по id
    /// </summary>
    /// <param name="dictionary">Наименование справочника, например <example>Preferences</example></param>
    /// <param name="id">Id элемента справочника, например <example>451533d5-d8d5-4a11-9c7b-eb9f14e1a32f</example></param>
    /// <returns></returns>
    [HttpGet("{dictionary}/{id:guid}")]
    public async Task<ActionResult<DictionaryElementGetDto>> GetElementByIdAsync(string dictionary, Guid id)
    {
        var element = await repository.GetAsync(id.ToString());

        if (element is null || element.DictionaryCode != dictionary)
        {
            return NoContent();
        }

        return new DictionaryElementGetDto
        {
            Id = element.Id,
            Name = element.Name,
            Code = element.Code
        };
    }

    /// <summary>
    /// Создать элемент справочника
    /// </summary>
    /// <param name="dictionary">Наименование справочника, например <example>Preferences</example></param>
    /// <param name="elementDto">Значения нового элемента справочника</param>
    /// <returns></returns>
    [HttpPost("{dictionary}")]
    public async Task<ActionResult<DictionaryElementGetDto>> CreateElementAsync(string dictionary,
        DictionaryElementSetDto elementDto)
    {
        var element = new DictionaryElement
        {
            DictionaryCode = dictionary,
            Code = elementDto.Code,
            Name = elementDto.Name
        };

        element = await repository.AddAsync(element);

        return new DictionaryElementGetDto
        {
            Id = element.Id,
            Name = element.Name,
            Code = element.Code
        };
    }

    /// <summary>
    /// Изменить элемент справочника по id
    /// </summary>
    /// <param name="dictionary">Наименование справочника, например <example>Preferences</example></param>
    /// <param name="id">Id элемента справочника, например <example>451533d5-d8d5-4a11-9c7b-eb9f14e1a32f</example></param>
    /// <param name="elementDto">Новые значения элемента справочника</param>
    /// <returns></returns>
    [HttpPost("{dictionary}/{id:guid}")]
    public async Task<ActionResult<DictionaryElementGetDto?>> SetElementByIdAsync(string dictionary, Guid id,
        DictionaryElementSetDto elementDto)
    {
        var element = new DictionaryElement
        {
            Id = id.ToString(),
            DictionaryCode = dictionary,
            Code = elementDto.Code,
            Name = elementDto.Name
        };

        element = await repository.SetAsync(element);

        if (element == null)
        {
            return NoContent();
        }

        return new DictionaryElementGetDto
        {
            Id = element.Id,
            Name = element.Name,
            Code = element.Code
        };
    }
}