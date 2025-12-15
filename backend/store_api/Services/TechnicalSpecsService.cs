using store_api.Dtos;
using store_api.Dtos.TechnicalSpecs;
using store_api.Entities;
using store_api.Repositories;
using store_api.Utils;

namespace store_api.Services;

public class TechnicalSpecsService(IConfiguration configuration)
{
    
    private readonly TechnicalSpecsRepository _repository = new(configuration);
    
    public async Task<Result<TechnicalSpecsEntity?>> Create(TechnicalSpecsAddDto dto)
    {
        try
        {
            TechnicalSpecsEntity? result = await _repository.Add(dto.ToEntity());

            if (result == null)
                return new Failure<TechnicalSpecsEntity?>(ResultCode.TECHNICAL_SPEC_NOT_CREATED,
                    "The Technical Spec couldn't be created!");

            return new Success<TechnicalSpecsEntity?>(ResultCode.TECHNICAL_SPEC_FOUND,
                $"The Technical Spec with key ({dto.Key}) has been created!", result);
        }
        catch (Exception e)
        {
            return new Failure<TechnicalSpecsEntity?>(ResultCode.TECHNICAL_SPEC_NOT_CREATED,
                $"[MESSAGE]: Couldn't create Technical Spec.\n[EXCEPTION]: {e.Message}");
        }
    }

    public async Task<Result<IEnumerable<TechnicalSpecsEntity>?>> GetAll()
    {
        try
        {
            IEnumerable<TechnicalSpecsEntity>? result = await _repository.GetAll();

            if (result == null)
                return new Failure<IEnumerable<TechnicalSpecsEntity>>(ResultCode.TECHNICAL_SPEC_NOT_FOUND,
                    "(0) Technical Specs were found!")!;

            return new Success<IEnumerable<TechnicalSpecsEntity>?>(ResultCode.TECHNICAL_SPEC_FOUND,
                $"{result.Count()} Technical Specs were found!", result);
        }
        catch (Exception e)
        {
            return new Failure<IEnumerable<TechnicalSpecsEntity>?>(ResultCode.TECHNICAL_SPEC_NOT_FOUND,
                $"[MESSAGE]: (0) Technical Specs were found!\n[EXCEPTION]: {e.Message}");
        }
    }

    public async Task<Result<TechnicalSpecsEntity?>> GetById(Guid id)
    {
        try
        {
            TechnicalSpecsEntity? result = await _repository.GetById(id);

            if (result == null)
                return new Failure<TechnicalSpecsEntity?>(ResultCode.TECHNICAL_SPEC_NOT_FOUND,
                    $"The Technical Spec with id ({id}) couldn't be found!");

            return new Success<TechnicalSpecsEntity?>(ResultCode.TECHNICAL_SPEC_FOUND,
                $"The Technical Spec with id ({id}) has been found!", result);
        }
        catch (Exception e)
        {
            return new Failure<TechnicalSpecsEntity?>(ResultCode.TECHNICAL_SPEC_NOT_FOUND,
                $"[MESSAGE]: The Technical Spec with id ({id}) couldn't be found!\n[EXCEPTION]: {e.Message}]");
        }
    }
    
    public async Task<Result<TechnicalSpecsEntity?>> GetByKey(String key)
    {
        try
        {
            TechnicalSpecsEntity? result = await _repository.GetByKey(key);

            if (result == null)
                return new Failure<TechnicalSpecsEntity?>(ResultCode.TECHNICAL_SPEC_NOT_FOUND,
                    $"The Technical Spec with key ({key}) couldn't be found!");

            return new Success<TechnicalSpecsEntity?>(ResultCode.TECHNICAL_SPEC_FOUND,
                $"The Technical Spec with key ({key}) has been found!", result);
        }
        catch (Exception e)
        {
            return new Failure<TechnicalSpecsEntity?>(ResultCode.TECHNICAL_SPEC_NOT_FOUND,
                $"[MESSAGE]: The Technical Spec with key ({key}) couldn't be found!\n[EXCEPTION]: {e.Message}]");
        }
    }

    // --- UPDATE ---
    public async Task<Result<TechnicalSpecsEntity?>> Update(Guid id, TechnicalSpecsUpdateDto<TechnicalSpecsEntity> dto)
    {
        try
        {
            TechnicalSpecsEntity? result = await _repository.Update(id, dto);

            if (result == null)
                return new Failure<TechnicalSpecsEntity?>(ResultCode.TECHNICAL_SPEC_NOT_UPDATED,
                    $"The Technical Spec with id ({id}) couldn't be updated!");

            return new Success<TechnicalSpecsEntity?>(ResultCode.TECHNICAL_SPEC_UPDATED,
                $"The Technical Spec with id ({id}) has been updated!", result);
        }
        catch (Exception e)
        {
            return new Failure<TechnicalSpecsEntity?>(ResultCode.TECHNICAL_SPEC_NOT_UPDATED,
                $"[MESSAGE]: The Technical Spec with id ({id}) couldn't be updated!\n[EXCEPTION]: {e.Message}]");
        }
    }

    // --- DELETE ---
    public async Task<Result<TechnicalSpecsEntity?>> Delete(Guid id)
    {
        try
        {
            TechnicalSpecsEntity? result = await _repository.Delete(id);

            if (result == null)
                return new Failure<TechnicalSpecsEntity?>(ResultCode.TECHNICAL_SPEC_NOT_DELETED,
                    $"The Technical Spec with id ({id}) couldn't be deleted!");

            return new Success<TechnicalSpecsEntity?>(ResultCode.TECHNICAL_SPEC_DELETED,
                $"The Technical Spec with id ({id}) has been deleted!", result);
        }
        catch (Exception e)
        {
            return new Failure<TechnicalSpecsEntity?>(ResultCode.TECHNICAL_SPEC_NOT_DELETED, e.Message);
        }
    }
}