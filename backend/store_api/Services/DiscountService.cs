using store_api.Dtos.Discounts;
using store_api.Entities;
using store_api.Repositories;
using store_api.Utils;

namespace store_api.Services;

public class DiscountService(IConfiguration configuration)
{

    private readonly DiscountsRepository _discountsRepository = new(configuration);
    private readonly ProductsRepository _productsRepository = new(configuration);

    public async Task<Result<IEnumerable<DiscountEntity>?>> GetAll()
    {
        IEnumerable<DiscountEntity>? result = await _discountsRepository.GetAll();

        if (result == null || result.Count() <= 0)
            return new Success<IEnumerable<DiscountEntity>?>(ResultCode.DISCOUNT_NOT_FOUND, "Discounts list is empty", result);

        return new Success<IEnumerable<DiscountEntity>?>(ResultCode.DISCOUNT_FOUND, "Discounts found", result);
    }

    public async Task<Result<DiscountEntity?>> GetById(Guid id)
    {
        DiscountEntity? discount = await _discountsRepository.GetById(id);

        if (discount == null)
            return new Failure<DiscountEntity?>(ResultCode.DISCOUNT_NOT_FOUND, "Discount not found");

        return new Success<DiscountEntity?>(ResultCode.DISCOUNT_FOUND, "Discount found", discount);
    }

    public async Task<Result<DiscountEntity?>> Update(Guid id, DiscountUpdateDto<DiscountEntity> dto)
    {
        try
        {
            DiscountEntity? discount = await _discountsRepository.Update(id, dto);

            if (discount == null)
                return new Failure<DiscountEntity?>(ResultCode.DISCOUNT_NOT_FOUND, "Discount not found");

            return new Success<DiscountEntity?>(ResultCode.DISCOUNT_UPDATED, "Discount updated", discount);
        }
        catch (Exception e)
        {
            return new Failure<DiscountEntity?>(ResultCode.DISCOUNT_NOT_UPDATED, e.Message);
        }
    }

    public async Task<Result<DiscountEntity?>> Delete(Guid id)
    {
        DiscountEntity? discount = await _discountsRepository.Delete(id);

        if (discount == null)
            return new Failure<DiscountEntity?>(ResultCode.DISCOUNT_NOT_DELETED, "Discount not found");

        return new Success<DiscountEntity?>(ResultCode.DISCOUNT_DELETED, "Discount deleted", discount);
    }

    public async Task<Result<DiscountEntity?>> AddDiscount(DiscountAddDto<DiscountEntity> dto)
    {
        try
        {
            ProductEntity? product = await _productsRepository.GetById(dto.ProductId);

            if (product == null)
                return new Failure<DiscountEntity?>(ResultCode.DISCOUNT_NOT_CREATED, "Product not found");

            DiscountEntity? prod = await _discountsRepository.GetActiveDiscount(product.Id);

            if (prod != null)
            {
                return new Failure<DiscountEntity?>(ResultCode.DISCOUNT_NOT_CREATED, "A discount to this product already exists.");
            }
            
            DiscountEntity? discount = await _discountsRepository.Add(dto.ToEntity());
        
            return new Success<DiscountEntity?>(ResultCode.DISCOUNT_CREATED, "Discount was created", discount);
        }
        catch (Exception ex)
        {
            return new Failure<DiscountEntity?>(ResultCode.DISCOUNT_NOT_CREATED, ex.Message);
        }
    }

    public async Task<Result<DiscountEntity?>> GetActiveDiscount(Guid productId)
    {
        DiscountEntity? discount = await _discountsRepository.GetActiveDiscount(productId);

        if (discount == null)
            return new Failure<DiscountEntity?>(ResultCode.DISCOUNT_NOT_FOUND, "Discount not found");
        
        return new Success<DiscountEntity?>(ResultCode.DISCOUNT_FOUND, "Discount was found", discount);
    }
}