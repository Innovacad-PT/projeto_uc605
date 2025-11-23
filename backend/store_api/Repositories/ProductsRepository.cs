using store_api.Controllers;
using store_api.Dtos;
using store_api.Entities;
using store_api.Utils;

namespace store_api.Repositories;

public class ProductsRepository : IBaseRepository<ProductEntity>
{
    
    private readonly static List<ProductEntity> _products = new ([
       /* new(Guid.NewGuid(), "Pipocas Doces", [], "",3.99, "Pipocas doces com manteiga", ""),
        new(Guid.NewGuid(), "Pipocas Salgadas", [],"",3.99, "Pipocas doces com sal", ""),
        new(Guid.NewGuid(), "Pão Caseiro", [], "",3.99, "", ""),
        new(Guid.NewGuid(), "Salsichas Frankfurt",[],"", 3.99, "", ""),*/
    ]);


    public Result<ProductEntity> Add(ProductEntity entity)
    {
        
        _products.Add(entity);
        
        return new Success<ProductEntity>(ResultCode.PRODUCT_CREATED, "Product created", entity);
    }

    
    public Result<ProductEntity> Update(Guid id, IBaseDto<ProductEntity> entity)
    {
        throw new NotImplementedException();
    }

    public Result<ProductEntity> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Result<ProductEntity> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Result<ProductEntity> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Result<IEnumerable<ProductEntity>> GetAll()
    {
        if (!_products.Any())
        {
            return new Failure<IEnumerable<ProductEntity>>(ResultCode.PRODUCT_NOT_FOUND, "Products not found");
        }
        
        return new Success<IEnumerable<ProductEntity>>(ResultCode.PRODUCT_FOUND, "Products found", _products);
    }

    public Result<IEnumerable<ProductEntity>> GetAllWithFilters(string search, Guid categoryId, Guid brandId, decimal minPrice, decimal maxPrice)
    {
        throw new NotImplementedException();
    }

    public Result<IEnumerable<ProductEntity>> GetProductsInStock()
    {
        throw new NotImplementedException();
    }

    public Result<IEnumerable<ProductEntity>> AddSpecs(Guid productId, List<TechnicalSpecsEntity> specs)
    {
        throw new NotImplementedException();
    }

    public Result<DiscountEntity> GetDiscount(Guid productId)
    {
        throw new NotImplementedException();
    }
}