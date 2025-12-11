using store_api.Dtos.Discounts;
using store_api.Entities;
using store_api.Repositories;
using store_api.Utils;

namespace store_api.Services;

public class DiscountService
{

    private DiscountsRepository _repository;
    private ProductsRepository _productsRepository;

    public DiscountService(IConfiguration configuration)
    {
        _repository = new();
        _productsRepository = new(configuration);
    }

    public Result<IEnumerable<DiscountEntity>?> GetAll()
    {
        IEnumerable<DiscountEntity>? result = _repository.GetAll();

        if (result == null || result.Count() <= 0)
        {
            return new Success<IEnumerable<DiscountEntity>?>(ResultCode.DISCOUNT_NOT_FOUND, "Discounts list is empty", result);
        }

        return new Success<IEnumerable<DiscountEntity>?>(ResultCode.DISCOUNT_FOUND, "Discounts found", result);
    }

    public Result<DiscountEntity?> GetById(Guid id)
    {
        DiscountEntity? discount = _repository.GetById(id);

        if (discount == null)
        {
            return new Failure<DiscountEntity?>(ResultCode.DISCOUNT_NOT_FOUND, "Discount not found");
        }
        
        return new Success<DiscountEntity?>(ResultCode.DISCOUNT_FOUND, "Discount found", discount);
    }

    public Result<DiscountEntity?> Update(Guid id, DiscountUpdateDto dto)
    {
        try
        {
            DiscountEntity? discount = _repository.Update(id, dto);

            if (discount == null)
            {
                return new Failure<DiscountEntity?>(ResultCode.DISCOUNT_NOT_FOUND, "Discount not found");
            }

            return new Success<DiscountEntity?>(ResultCode.DISCOUNT_UPDATED, "Discount updated", discount);
        }
        catch (Exception e)
        {
            return new Failure<DiscountEntity?>(ResultCode.DISCOUNT_NOT_UPDATED, e.Message);
        }
    }

    public Result<DiscountEntity?> Delete(Guid id)
    {
        DiscountEntity? discount = _repository.Delete(id);

        if (discount == null)
        {
            return new Failure<DiscountEntity?>(ResultCode.DISCOUNT_NOT_DELETED, "Discount not found");
        }

        return new Success<DiscountEntity?>(ResultCode.DISCOUNT_DELETED, "Discount deleted", discount);
    }

    public Result<DiscountEntity?> AddDiscount(DiscountAddDto dto)
    {
        try
        {
            ProductEntity? product = _productsRepository.GetById(dto.ProductId);

            if (product == null)
            {
                return new Failure<DiscountEntity?>(ResultCode.DISCOUNT_NOT_CREATED, "Product not found");
            }

            DiscountEntity? discount = _repository.Add(dto.ToEntity());
        
            return new Success<DiscountEntity?>(ResultCode.DISCOUNT_CREATED, "Discount was created", discount);
        }
        catch (Exception ex)
        {
            return new Failure<DiscountEntity?>(ResultCode.DISCOUNT_NOT_CREATED, ex.Message);
        }
    }

    public Result<DiscountEntity?> GetActiveDiscount(Guid productId)
    {
        DiscountEntity? discount = _repository.GetActiveDiscount(productId);

        if (discount == null)
        {
            return new Failure<DiscountEntity?>(ResultCode.DISCOUNT_NOT_FOUND, "Discount not found");
        }
        
        return new Success<DiscountEntity?>(ResultCode.DISCOUNT_FOUND, "Discount was found", discount);
    }
}