using store_api.Dtos.Brands;
using store_api.Entities;
using store_api.Repositories;
using store_api.Utils;

namespace store_api.Services;

public class BrandsService
{
    
    private readonly BrandsRepository _brandsRepository = new();
    
    public Result<BrandEntity?> CreateBrand(BrandAddDto<BrandEntity> dto)
    {
        
        Result<BrandEntity> result = _brandsRepository.Add(dto.ToEntity());

        return result;
    }

    public Result<IEnumerable<BrandEntity>?> GetAllBrands()
    {
        return _brandsRepository.GetAll();
    }

    public Result<BrandEntity?> GetByName(String name)
    {
        return _brandsRepository.GetByName(name);
    }

    public Result<BrandEntity?> GetById(Guid id)
    {
        return _brandsRepository.GetById(id);
    }

    public Result<BrandEntity?> Update(Guid id, BrandUpdateDto<BrandEntity> dto)
    {
        return _brandsRepository.Update(id, dto);
    }

    public Result<BrandEntity?> Delete(Guid id)
    {
        return _brandsRepository.Delete(id);
    }
}