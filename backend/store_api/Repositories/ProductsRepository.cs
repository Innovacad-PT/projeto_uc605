using store_api.Controllers;
using store_api.Dtos;
using store_api.Entities;
using store_api.Exceptions;
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


    public ProductEntity Add(ProductEntity entity)
    {

        if (_products.Any(p => p.Id == entity.Id))
        {
            throw new SameIdException("Product with same id already exists");
        }
        
        _products.Add(entity);
        
        return entity;
    }

    
    public ProductEntity? Update(Guid id, IBaseDto<ProductEntity> entity)
    {
        
        ProductUpdateDto updateDto = entity as ProductUpdateDto;

        if (updateDto == null)
        {
            throw new InvalidDtoType("Invalid data transfer object type");
        }
        
        if (!_products.Any(p => p.Id == id))
        {
            return null;
        }
        
        ProductEntity product = _products.First(p => p.Id == id);
        
        product.Name = updateDto.Name ?? product.Name;
        product.Price = updateDto.Price ?? product.Price;
        product.Description = updateDto.Description ?? product.Description;
        product.TechnicalSpecs = updateDto.TechnicalSpecs ?? product.TechnicalSpecs;
        product.Reviews = updateDto.Reviews ?? product.Reviews;
        
        if (updateDto.ImageFile != null)
        {
            string folder = Path.Combine("wwwroot", "images");
            
            string filename = $"{id}{Path.GetExtension(updateDto.ImageFile.FileName)}";
            string filePath = Path.Combine(folder, filename);
            
            String imageUrl = $"/images/{filename}";
            product.ImageUrl = imageUrl;
        }
        
        return product;
    }

    public ProductEntity? Delete(Guid id)
    {
        if (!_products.Any(p => p.Id == id))
        {
            return null;
        }
        
        ProductEntity product = _products.First(p => p.Id == id);
        _products.Remove(product);
        return product;
    }

    public ProductEntity? GetById(Guid id)
    {
        ProductEntity? product = _products.FirstOrDefault(p => p.Id == id);
        
        return product;
    }

    public IEnumerable<ProductEntity>? GetAll()
    {
        return _products;
    }

    public IEnumerable<ProductEntity>? GetAllWithFilters(string search, Guid categoryId, Guid brandId, double minPrice, double maxPrice)
    {
        IEnumerable<ProductEntity>? products = _products.Where(p =>
            p.Name.ToLower().ContainsAny(search.ToLower().AsSpan()) && p.Category.Id == categoryId && p.Brand.Id == brandId && (p.Price >= minPrice && p.Price <= maxPrice));

        return products;
    }

    public IEnumerable<ProductEntity> GetProductsInStock()
    {
        IEnumerable<ProductEntity>? products = _products.Where(p => p.Stock > 0);

        return products;
    }

    public ProductEntity? AddSpecs(Guid productId, List<TechnicalSpecsEntity> specs)
    {
        if (!_products.Any(p => p.Id == productId))
        {
            return null;
        }
        
        ProductEntity product = _products.First(p => p.Id == productId);
        product.TechnicalSpecs.AddRange(specs);
        return product;
    }

    public ProductEntity? IncreaseStock(Guid productId, int amount)
    {
        if (!_products.Any(p => p.Id == productId))
        {
            return null;
        }

        ProductEntity product = _products.First(p => p.Id == productId);

        product.Stock += amount;

        return product;
    }

    public ProductEntity? DecreaseStock(Guid productId, int amount)
    {
        if (!_products.Any(p => p.Id == productId))
        {
            return null;
        }

        ProductEntity product = _products.First(p => p.Id == productId);

        product.Stock -= amount;

        return product;
    }
}