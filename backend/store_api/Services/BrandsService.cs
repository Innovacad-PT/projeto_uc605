using System.Data.SqlTypes;
using store_api.Dtos.Brands;
using store_api.Entities;
using store_api.Exceptions;
using store_api.Repositories;
using store_api.Utils;

namespace store_api.Services;

public class BrandsService
{
    
    private readonly BrandsRepository _brandsRepository;

    public BrandsService(IConfiguration configuration)
    {
        _brandsRepository = new(configuration);
    }
    
    public async Task<Result<BrandEntity?>> CreateBrand(BrandAddDto<BrandEntity> dto)
    {
        try
        {
            BrandEntity? result = await _brandsRepository.Add(dto.ToEntity());

            if (result == null)
                return new Failure<BrandEntity?>(ResultCode.BRAND_NOT_CREATED, $"The brand with the following id ({dto.Id}) couldn't be created!");

            return new Success<BrandEntity?>(ResultCode.BRAND_FOUND, $"The brand with the following id ({dto.Id}) has been created!", result);
        }
        catch (Exception e)
        {
            return new Failure<BrandEntity?>(ResultCode.BRAND_NOT_CREATED, $"[MESSAGE]: The brand with the following id ({dto.Id}) couldn't be created!\n[EXCEPTION]: {e.Message}");
        }
    }

    public async Task<Result<IEnumerable<BrandEntity>?>> GetAllBrands()
    {
        try
        {
            IEnumerable<BrandEntity>? result = await _brandsRepository.GetAll();

            if (result == null)
                return new Failure<IEnumerable<BrandEntity>?>(ResultCode.BRAND_NOT_FOUND, "(0) brands were found!");

            return new Success<IEnumerable<BrandEntity>?>(ResultCode.BRAND_FOUND, $"{result.Count()} brands were found!", result);
        }
        catch (Exception e)
        {
            return new Failure<IEnumerable<BrandEntity>?>(ResultCode.BRAND_NOT_FOUND, $"[MESSAGE]: (0) brands were found!\n[EXCEPTION]: {e.Message}");
        }
    }

    public async Task<Result<BrandEntity?>> GetById(Guid id)
    {
        try
        {
            BrandEntity? result = await _brandsRepository.GetById(id);

            if (result == null)
                return new Failure<BrandEntity?>(ResultCode.BRAND_NOT_FOUND, $"The brand with the following id ({id}) couldn't be found!");

            return new Success<BrandEntity?>(ResultCode.BRAND_FOUND, $"The brand with the following id ({id}) has been found!", result);
        }
        catch (Exception e)
        {
            return new Failure<BrandEntity?>(ResultCode.BRAND_NOT_FOUND, $"[MESSAGE]: The brand with the following id ({id}) couldn't be found!\n[EXCEPTION]: {e.Message}]");
        }
    }

    public async Task<Result<BrandEntity?>> Update(Guid id, BrandUpdateDto<BrandEntity> dto)
    {
        try
        {
            BrandEntity? result = await _brandsRepository.Update(id, dto);

            if (result == null)
                return new Failure<BrandEntity?>(ResultCode.BRAND_NOT_UPDATED, $"The brand with the following id ({id}) couldn't be updated!");

            return new Success<BrandEntity?>(ResultCode.BRAND_UPDATED, $"The brand with the following id ({id}) has been updated!", result);
        }
        catch (Exception e)
        {
            return new Failure<BrandEntity?>(ResultCode.BRAND_NOT_UPDATED, $"[MESSAGE]: The brand with the following id ({id}) couldn't be updated!\n[EXCEPTION]: {e.Message}]");
        }
    }

    public async Task< Result<BrandEntity?>> Delete(Guid id)
    {
        try
        {
            BrandEntity? result = await _brandsRepository.Delete(id);

            if (result == null)
                return new Failure<BrandEntity?>(ResultCode.BRAND_NOT_DELETED, $"The brand with the following id ({id}) couldn't be deleted!");

            return new Success<BrandEntity?>(ResultCode.BRAND_DELETED, $"The brand with the following id ({id}) has been deleted!", result);
        }
        catch (Exception e)
        {
            return new Failure<BrandEntity?>(ResultCode.BRAND_NOT_DELETED, e.Message);
        }
    }
}