using store_api.Controllers;
using store_api.Dtos;
using store_api.Dtos.Brands;
using store_api.Entities;
using store_api.Exceptions;
using store_api.Utils;

namespace store_api.Repositories;

public class BrandsRepository : IBaseRepository<BrandEntity>
{
    
    private readonly static List<BrandEntity> _brands = [
        new(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), "Lenovo")
    ];
    
    public BrandEntity? Add(BrandEntity entity)
    {
        
        if (_brands.Any((b) => b.Id == entity.Id))
        {
            throw new SameIdException("Brand with the same id already exists.");
        }

        if (_brands.Any((b) => b.Name.ToLower() == entity.Name.ToLower()))
        {
            throw new SameNameException("Brand with the same name already exists.");
        }
        
        _brands.Add(entity);

        return entity;
    }
    
    public BrandEntity? Update(Guid id, IBaseDto<BrandEntity> dto)
    {
        
        var updateDto = dto as BrandUpdateDto<BrandEntity>;  

        if (updateDto == null)
        {
            throw new InvalidDtoType("Invalid data transfer object type.");
        }
        
        BrandEntity? entity = _brands.FirstOrDefault((b) => b.Id == id);

        if (entity == null)
        {
            return null;
        }

        entity.Name = updateDto.Name ?? entity.Name;
        
        return entity;
    }

    public BrandEntity? Delete(Guid id)
    {
        BrandEntity? entity = _brands.FirstOrDefault((b) => b.Id == id);

        if (entity == null)
        {
            return null;
        }

        _brands.Remove(entity);

        return entity;
    }

    public BrandEntity? GetById(Guid id)
    {
        BrandEntity? entity = _brands.FirstOrDefault((b) => b.Id == id);
        
        return entity;
    }

    public IEnumerable<BrandEntity>? GetAll()
    {
        return _brands;
    }

    public BrandEntity? GetByName(String name)
    {
        BrandEntity? brand = _brands.FirstOrDefault((b) => b.Name.ToLower() == name.ToLower());

        return brand;
    }
}