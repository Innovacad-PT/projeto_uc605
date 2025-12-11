using store_api.Controllers;
using store_api.Dtos;
using store_api.Dtos.TechnicalSpecs;
using store_api.Entities;
using store_api.Exceptions;

namespace store_api.Repositories;

public class TechnicalSpecsRepository : IBaseRepository<TechnicalSpecsEntity>
{
    private readonly static List<TechnicalSpecsEntity> _specs = [
        new() { TechnicalSpecsId = Guid.Parse("b1f7d9c8-4a5e-4c7b-9f0e-2d1a3f6e8c0d"), Key = "RAM" },
        new() { TechnicalSpecsId = Guid.Parse("f9e6a0d2-1b3c-4e5f-8a7b-0c9d2e4f6a8b"), Key = "Screen Size" },
        new() { TechnicalSpecsId = Guid.Parse("1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"), Key = "Processor" }
    ];
    
    public TechnicalSpecsEntity? Add(TechnicalSpecsEntity entity)
    {
        
        if (_specs.Any((s) => s.TechnicalSpecsId == entity.TechnicalSpecsId))
        {
            throw new SameIdException("Technical Spec with the same id already exists.");
        }

        if (_specs.Any((s) => s.Key.ToLower() == entity.Key.ToLower()))
        {
            throw new SameNameException("Technical Spec with the same key/name already exists.");
        }
        
        _specs.Add(entity);

        return entity;
    }
    
    public IEnumerable<TechnicalSpecsEntity>? GetAll()
    {
        return _specs;
    }

    public TechnicalSpecsEntity? GetById(Guid id)
    {
        TechnicalSpecsEntity? entity = _specs.FirstOrDefault((s) => s.TechnicalSpecsId == id);
        return entity;
    }

    public TechnicalSpecsEntity? GetByKey(String key)
    {
        TechnicalSpecsEntity? spec = _specs.FirstOrDefault((s) => s.Key.ToLower() == key.ToLower());
        return spec;
    }

    public TechnicalSpecsEntity? Update(Guid id, IBaseDto<TechnicalSpecsEntity> dto)
    {
        
        var updateDto = dto as TechnicalSpecsUpdateDto;  

        if (updateDto == null)
        {
            throw new InvalidDtoType("Invalid data transfer object type.");
        }
        
        TechnicalSpecsEntity? entity = _specs.FirstOrDefault((s) => s.TechnicalSpecsId == id);

        if (entity == null)
        {
            return null;
        }
        
        if (updateDto.Key != null && _specs.Any(s => s.TechnicalSpecsId != id && s.Key.ToLower() == updateDto.Key.ToLower()))
        {
             throw new SameNameException($"Technical Spec with the key '{updateDto.Key}' already exists.");
        }


        entity.Key = updateDto.Key ?? entity.Key;
        
        return entity;
    }

    public TechnicalSpecsEntity? Delete(Guid id)
    {
        TechnicalSpecsEntity? entity = _specs.FirstOrDefault((s) => s.TechnicalSpecsId == id);

        if (entity == null)
        {
            return null;
        }

        _specs.Remove(entity);

        return entity;
    }
    
    public TechnicalSpecsEntity? GetByName(string name) => GetByKey(name);
}