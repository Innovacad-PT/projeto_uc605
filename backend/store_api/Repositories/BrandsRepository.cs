using store_api.Controllers;
using store_api.Dtos;
using store_api.Dtos.Brands;
using store_api.Entities;
using store_api.Utils;

namespace store_api.Repositories;

public class BrandsRepository : IBaseRepository<BrandEntity>
{
    
    private readonly static List<BrandEntity> _brands = [
        new(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), "Lenovo")
    ];
    
    public Result<BrandEntity> Add(BrandEntity dto)
    {
        
        if (_brands.Any((b) => b.Id == dto.Id))
        {
            return new Failure<BrandEntity>(ResultCode.BRAND_EXISTING_GUID, $"Brand with id ({dto.Id}) already exists");
        }

        if (_brands.Any((b) => b.Name.ToLower() == dto.Name.ToLower()))
        {
            return new Failure<BrandEntity>(ResultCode.BRAND_EXISTING_NAME, $"Brand with name ({dto.Name}) already exists");
        }
        
        _brands.Add(dto);

        return new Success<BrandEntity>(ResultCode.BRAND_CREATED, "Brand created", dto);
    }
    
    public Result<BrandEntity> Update(Guid id, IBaseDto<BrandEntity> dto)
    {
        
        var updateDto = dto as BrandUpdateDto<BrandEntity>;  

        if (updateDto == null)
        {
            return new Failure<BrandEntity>(ResultCode.INVALID_DTO, "Invalid dto type");
        }
        
        BrandEntity? entity = _brands.FirstOrDefault((b) => b.Id == id);

        if (entity == null)
        {
            return new Failure<BrandEntity>(ResultCode.BRAND_NOT_FOUND, $"Brand with id ({id}) not found");
        }

        entity.Name = updateDto.Name ?? entity.Name;
        
        return new Success<BrandEntity>(ResultCode.BRAND_UPDATED, $"Brand with id ({id}) updated", entity);
    }

    public Result<BrandEntity> Delete(Guid id)
    {
        BrandEntity? entity = _brands.FirstOrDefault((b) => b.Id == id);

        if (entity == null)
        {
            return new Failure<BrandEntity>(ResultCode.BRAND_NOT_FOUND, $"Brand with id ({id}) not found");
        }

        _brands.Remove(entity);
        
        return new Success<BrandEntity>(ResultCode.BRAND_DELETED, $"Brand with id ({id}) deleted", entity);
    }

    public Result<BrandEntity> GetById(Guid id)
    {
        BrandEntity? entity = _brands.FirstOrDefault((b) => b.Id == id);

        if (entity == null)
        {
            return new Failure<BrandEntity>(ResultCode.BRAND_NOT_FOUND, $"Brand with id ({id}) not found");
        }
        
        return new Success<BrandEntity>(ResultCode.BRAND_FOUND, $"Brand with id ({id}) found", entity);
    }

    public Result<IEnumerable<BrandEntity>> GetAll()
    {
        if (!_brands.Any())
        {
            return new Success<IEnumerable<BrandEntity>>(ResultCode.BRAND_NOT_FOUND, "Brands not found", []);
        }

        return new Success<IEnumerable<BrandEntity>>(ResultCode.BRAND_FOUND, "Brands found", _brands);
    }

    public Result<BrandEntity> GetByName(String name)
    {
        BrandEntity? brand = _brands.FirstOrDefault((b) => b.Name.ToLower() == name.ToLower());

        if (brand == null)
        {
            return new Failure<BrandEntity>(ResultCode.PRODUCT_NOT_FOUND, $"Brand with name ({name}) not found");
        }
        
        return new Success<BrandEntity>(ResultCode.BRAND_FOUND, "Brand found", brand);
    }
}