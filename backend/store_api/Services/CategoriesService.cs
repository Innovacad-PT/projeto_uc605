using store_api.Dtos.Categories;
using store_api.Entities;
using store_api.Exceptions;
using store_api.Repositories;
using store_api.Utils;

namespace store_api.Services;

public class CategoriesService(IConfiguration configuration)
{
    
    private readonly CategoriesRepository _categoriesRepository = new(configuration);

    public async Task<Result<CategoryEntity?>> GetById(Guid id)
    {
        try
        {
            var result = await _categoriesRepository.GetById(id);

            if (result == null)
                return new Failure<CategoryEntity?>(ResultCode.CATEGORY_NOT_FOUND, $"The category with the following id ({id}) couldn't be found!");

            return new Success<CategoryEntity?>(ResultCode.CATEGORY_FOUND, $"The category with the following id ({id}) was found!", result);
        }
        catch (Exception e)
        {
            return new Failure<CategoryEntity?>(ResultCode.CATEGORY_NOT_FOUND, $"[MESSAGE]: The category with the following id ({id}) couldn't be found!\n[EXCEPTION]: {e.Message}");
        }
    }
    
    public async Task<Result<CategoryEntity?>> CreateCategory(CategoryAddDto dto)
    {
        try
        {
            var result = await _categoriesRepository.Add(dto.ToEntity());

            if (result == null)
                return new Failure<CategoryEntity?>(ResultCode.CATEGORY_NOT_CREATED, $"The category with the following id ({dto.Id}) couldn't be created!");

            return new Success<CategoryEntity?>(ResultCode.CATEGORY_CREATED, $"The category with the following id ({dto.Id}) was created!", result);
        }
        catch (Exception e)
        {
            return new Failure<CategoryEntity?>(ResultCode.CATEGORY_NOT_CREATED, $"[MESSAGE]: The category with the following id ({dto.Id}) couldn't be created!\n[EXCEPTION]: {e.Message}");
        }
    }

    public async Task<Result<IEnumerable<CategoryEntity>?>> GetAllCategories()
    {
        try
        {
            var result = await _categoriesRepository.GetAll();

            if (result == null)
                return new Failure<IEnumerable<CategoryEntity>?>(ResultCode.CATEGORY_NOT_FOUND, $"(0) categories were found!");

            return new Success<IEnumerable<CategoryEntity>?>(ResultCode.CATEGORY_FOUND, $"({result.Count()}) categories were found!", result);
        }
        catch (Exception e)
        {
            return new Failure<IEnumerable<CategoryEntity>?>(ResultCode.CATEGORY_NOT_FOUND, $"[MESSAGE]: (0) categories were found!\n[EXCEPTION]: {e.Message}");
        }
    }

    public async Task<Result<CategoryEntity?>> Update(Guid id, CategoryUpdateDto<CategoryEntity> dto)
    {
        try
        {
            CategoryEntity? result = await _categoriesRepository.Update(id, dto);

            if (result == null)
                return new Failure<CategoryEntity?>(ResultCode.CATEGORY_NOT_CREATED, $"The brand with the following id ({id}) couldn't be updated!");

            return new Success<CategoryEntity?>(ResultCode.CATEGORY_CREATED, $"The brand with the following id ({id}) has been updated!", result);
        }
        catch (Exception e)
        {
            return new Failure<CategoryEntity?>(ResultCode.CATEGORY_NOT_CREATED, $"[MESSAGE]: The brand with the following id ({id}) couldn't be updated!\n[EXCEPTION]: {e.Message}]");
        }
    }

    public async Task< Result<CategoryEntity?>> Delete(Guid id)
    {
        try
        {
            CategoryEntity? result = await _categoriesRepository.Delete(id);

            if (result == null)
                return new Failure<CategoryEntity?>(ResultCode.CATEGORY_NOT_DELETED, $"The brand with the following id ({id}) couldn't be deleted!");

            return new Success<CategoryEntity?>(ResultCode.CATEGORY_DELETED, $"The brand with the following id ({id}) has been deleted!", result);
        }
        catch (Exception e)
        {
            return new Failure<CategoryEntity?>(ResultCode.BRAND_NOT_DELETED, e.Message);
        }
    }
}