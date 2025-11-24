using store_api.Controllers;
using store_api.Dtos;
using store_api.Dtos.Discounts;
using store_api.Entities;
using store_api.Exceptions;
using store_api.Utils;

namespace store_api.Repositories;

public class DiscountsRepository : IBaseRepository<DiscountEntity>
{
    
    private readonly static List<DiscountEntity> _discounts = new();
    
    public DiscountEntity? Add(DiscountEntity entity)
    {
        if (_discounts.Any((d) => d.Id == entity.Id))
        {
            throw new Exception("Discount already exists");
        }

        if (_discounts.Any((d) => d.ProductId == entity.ProductId && DateTime.Now.CompareTo(d.EndDate) == -1))
        {
            throw new Exception("Discount already exists for this product");
        }
        
        _discounts.Add(entity);
        return entity;
    }

    public DiscountEntity? Update(Guid id, IBaseDto<DiscountEntity> entity)
    {

        DiscountUpdateDto updateDto = entity as DiscountUpdateDto;

        if (updateDto == null)
        {
            throw new InvalidDtoType("Invalid data transfer object type");
        }
        
        if (!_discounts.Any((d) => d.Id == id))
        {
            return null;
        }
        
        DiscountEntity discount = _discounts.First((d) => d.Id == id);
        
        discount.ProductId = updateDto.ProductId ?? discount.ProductId;
        discount.Percentage = updateDto.Percentage ?? discount.Percentage;
        discount.EndDate = updateDto.EndDate ?? discount.EndDate;
        discount.StartDate = updateDto.StartDate ?? discount.StartDate;

        return discount;
    }

    public DiscountEntity? Delete(Guid id)
    {
        if (!_discounts.Any((d) => d.Id == id))
        {
            return null;
        }
        
        DiscountEntity discount = _discounts.First((d) => d.Id == id);
        _discounts.Remove(discount);
        return discount;
    }

    public DiscountEntity? GetById(Guid id)
    {
        DiscountEntity? discount = _discounts.FirstOrDefault(d => d.Id == id);
        
        return discount;
    }
    
    public IEnumerable<DiscountEntity>? GetAll()
    {
        return _discounts;
    }

    public DiscountEntity? GetActiveDiscount(Guid productId)
    {
        DiscountEntity? discount = _discounts.FirstOrDefault(d => d.ProductId == productId && DateTime.Now.CompareTo(d.EndDate) == 1);
        
        return discount;
    }
}