using store_api.Controllers;
using store_api.Entities;
using store_api.Utils;

namespace store_api.Repositories;

public class ProductsRepository : IBaseRepository<ProductEntity>
{
    
    static List<ProductEntity> products = new ([
       /* new(Guid.NewGuid(), "Pipocas Doces", [], "",3.99, "Pipocas doces com manteiga", ""),
        new(Guid.NewGuid(), "Pipocas Salgadas", [],"",3.99, "Pipocas doces com sal", ""),
        new(Guid.NewGuid(), "Pão Caseiro", [], "",3.99, "", ""),
        new(Guid.NewGuid(), "Salsichas Frankfurt",[],"", 3.99, "", ""),*/
    ]);


    public Result<ProductEntity> Add(ProductEntity entity)
    {
        throw new NotImplementedException();
    }

    public Result<ProductEntity> Update(ProductEntity entity)
    {
        throw new NotImplementedException();
    }

    public Result<ProductEntity> Delete(ProductEntity entity)
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
        throw new NotImplementedException();
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