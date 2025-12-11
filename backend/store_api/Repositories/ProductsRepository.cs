using store_api.Controllers;
using store_api.Dtos;
using store_api.Entities;
using store_api.Exceptions;
using store_api.Utils;

namespace store_api.Repositories;

public class ProductsRepository : IBaseRepository<ProductEntity>
{

    private readonly CategoriesRepository _categoriesRepository;
    private readonly BrandsRepository _brandsRepository;

    public ProductsRepository(IConfiguration configuration)
    {
        _categoriesRepository = new();
        _brandsRepository = new(configuration);
    }
    
    private readonly static List<ProductEntity> _products = new ([
       new(Guid.Parse("7bd2718c-5e2c-48b0-8b99-04907b43e614"),
       "Portatil i9",
       new (Guid.Parse("40c9354a-1002-425d-a561-45895910ad86"), "Computador"),
       new(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), "Lenovo"),
       [
       new(Guid.Parse("7bd2718c-5e2c-48b0-8b99-04907b43e614"), Guid.Parse("1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"), "Processor", "i9-14700K")],
       "",
       decimal.Parse((169.99).ToString()),
       "",
       3)
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

    
    public async Task< ProductEntity?> Update(Guid id, IBaseDto<ProductEntity> entity)
    {
        
        ProductUpdateDto updateDto = entity as ProductUpdateDto;

        if (updateDto == null)
        {
            throw new InvalidDtoType("Invalid data transfer object type");
        }
        
        if (_products.All(p => p.Id != id)) return null;
        
        ProductEntity product = _products.First(p => p.Id == id);
        
        product.Name = updateDto.Name ?? product.Name;
        product.Price = updateDto.Price ?? product.Price;
        product.Description = updateDto.Description ?? product.Description;
        product.TechnicalSpecs = updateDto.TechnicalSpecs ?? product.TechnicalSpecs;
        product.Reviews = updateDto.Reviews ?? product.Reviews;
        product.Stock = updateDto.Stock ?? product.Stock;
        product.Category = _categoriesRepository.GetById(updateDto.CategoryId ?? product.Category.Id) ?? product.Category;
        product.Brand = await _brandsRepository.GetById(updateDto.BrandId ?? product.Brand.Id) ?? product.Brand;
        
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
        if (_products.All(p => p.Id != id)) return null;
        
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

    public IEnumerable<ProductEntity>? GetAllWithFilters(string search, Guid categoryId, Guid brandId, decimal minPrice, decimal maxPrice)
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

    public ProductEntity? AddSpecs(Guid productId, List<ProductTechnicalSpecsEntity> specs)
    {
        ProductEntity? product = _products.FirstOrDefault(p => p.Id == productId);
        
        if (product == null)
        {
            return null;
        }

        List<ProductTechnicalSpecsEntity> existingSpecs = product.TechnicalSpecs;
        
        HashSet<Guid> existingSpecIds = new(existingSpecs.Select(s => s.TechnicalSpecsId));

        List<ProductTechnicalSpecsEntity> newSpecs = new();
        List<ProductTechnicalSpecsEntity> duplicateSpecs = new();

        foreach (var spec in specs)
        {
            spec.ProductId = productId;
            
            if (existingSpecIds.Contains(spec.TechnicalSpecsId))
            {
                duplicateSpecs.Add(spec); 
            }
            else
            {
                newSpecs.Add(spec);
            }
        }
        
        if (duplicateSpecs.Count > 0)
        {
            string duplicateKeys = string.Join(", ", duplicateSpecs.Select(d => d.Key));
            throw new InvalidOperationException($"Cannot add technical specs. The following spec keys already exist for product {productId}: {duplicateKeys}");
        }

        product.TechnicalSpecs.AddRange(newSpecs);

        return product;
    }

    public ProductEntity? IncreaseStock(Guid productId, int amount)
    {
        if (_products.All(p => p.Id != productId)) return null;

        ProductEntity product = _products.First(p => p.Id == productId);

        product.Stock += amount;

        return product;
    }

    public ProductEntity? DecreaseStock(Guid productId, int amount)
    {
        if (_products.All(p => p.Id != productId)) return null;

        ProductEntity product = _products.First(p => p.Id == productId);

        product.Stock -= amount;

        return product;
    }

    public bool CanCreateOrder(Guid productId, int quantity)
    {
        if(_products.Find(p => p.Id == productId) == null)
        {
            return false;
        }

        ProductEntity product = _products.First(p => p.Id == productId);

        return product.Stock >= quantity;
    }
}