using store_api.Entities;
using store_api.Repositories;
using store_api.Utils;

namespace store_api.Services;

public class ProductsService
{
    private readonly ProductsRepository _productsRepository = new ProductsRepository();

    public ProductEntity? CreateProduct(ProductCreateDto productDto)
    {
        throw new NotImplementedException();
    }

    public ProductEntity? UpdateProduct(Guid id, ProductUpdateDto productDto){
        throw new NotImplementedException();
    }

    public ProductEntity? DeleteProduct(Guid id){
        throw new NotImplementedException();
    }

    public ProductEntity? GetProductById(Guid id){
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ProductEntity>?> GetAllProducts(String search, String category, decimal minPice, decimal maxPrice){
        throw new NotImplementedException();
    }

    public ProductEntity? ApplyDiscount(Guid productId){
        throw new NotImplementedException();
    }

    public ProductEntity? IncreaseStock(Guid productId, int amount){
        throw new NotImplementedException();
    }

    public ProductEntity? DecreaseStock(Guid productId, int amount){
        throw new NotImplementedException();
    }

    public ProductEntity? AddTechnicalSpecs(Guid productId, List<TechnicalSpecsEntity> list){
        throw new NotImplementedException();
    }

    /*public async Task<Result<List<ProductEntity>>> GetAll()
    {
        var products = await _productsRepository.GetAll();

        return new Success<List<ProductEntity>>(ResultCode.PRODUCT_FOUND, "Retrieved products successfuly", products);
    }

    public async Task<Result<ProductEntity>> GetById(Guid id)
    {

        if (Guid.Empty == id)
        {
            return new Failure<ProductEntity>(ResultCode.INVALID_GUID, "Id cannot be empty.");
        }

        var product = await _productsRepository.GetById(id);

        if (product == null)
        {
            return new Failure<ProductEntity>(ResultCode.USER_NOT_FOUND,"User not found.");
        }
        
        return new Success<ProductEntity>(ResultCode.USER_FOUND, "User retrieved successfully.", product);
    }

    public async Task<Result<ProductEntity>> CreateProduct(ProductCreateDto product)
    {
        
        Guid productId = Guid.NewGuid();

        string? imageUrl = null;

        if (product.Image != null)
        {
            string folder = Path.Combine("wwwroot", "images");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            
            string filename = $"{productId}{Path.GetExtension(product.Image.FileName)}";
            string filePath = Path.Combine(folder, filename);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await product.Image.CopyToAsync(stream);
            }
            
            imageUrl = $"/images/{filename}";
        }
        
        var entity = new ProductEntity(productId, product.Name, product.Categories ?? [], imageUrl, product.Price, product.Details ?? "", product.TechInfo ?? "");
        ProductEntity prod = await _productsRepository.CreateProduct(entity);

        return new Success<ProductEntity>(ResultCode.PRODUCT_CREATED, "Product created successfully", prod);
    }

    public async Task<Result<ProductEntity>> UpdateProduct(Guid id, ProductUpdateDto product)
    {
        
        var productEntity = await _productsRepository.GetById(id);

        if (productEntity == null)
        {
            return new Failure<ProductEntity>(ResultCode.PRODUCT_NOT_FOUND, "Product not found.");
        }

        if (product.Name != null)
        {
            productEntity.Name = product.Name;
        } 
        
        if (product.Price != null)
        {
            productEntity.Price = product.Price.Value;
        } 
        
        if (product.Details != null)
        {
            productEntity.Details = product.Details;
        } 
        
        if (product.TechInfo != null)
        {
            productEntity.TechInfo = product.TechInfo;
        }

        if (product.Categories != null)
        {
            productEntity.Categories = product.Categories;
        }

        if (product.Image != null)
        {
            string folder = Path.Combine("wwwroot", "images");
            
            if(!Directory.Exists(folder)) {
                Directory.CreateDirectory(folder);
            }

            if (!string.IsNullOrEmpty(productEntity.ImageUrl))
            {
                string oldFilePath = Path.Combine(folder, productEntity.ImageUrl);
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
                
                string fileName = $"{productEntity.Id}{Path.GetExtension(productEntity.ImageUrl)}";

                using (var stream = new FileStream(oldFilePath, FileMode.Create))
                {
                    await product.Image.CopyToAsync(stream);
                }
                
                productEntity.ImageUrl = $"/images/{fileName}";
            }

            await _productsRepository.UpdateProduct(id, product);
            
            return new Success<ProductEntity>(ResultCode.PRODUCT_UPDATED, "Product updated successfully", productEntity);
        }
            
        
        var newProduct = await _productsRepository.UpdateProduct(id, product);

        return new Success<ProductEntity>(ResultCode.PRODUCT_UPDATED, "Product updated successfully", newProduct);
    }

    public async Task<Result<ProductEntity>> DeleteProduct(String id)
    {
        var oldProduct = await _productsRepository.DeleteProduct(id);

        if(oldProduct == null) return new Failure<ProductEntity>(ResultCode.PRODUCT_NOT_FOUND, "Product not found.");

        return new Success<ProductEntity>(ResultCode.PRODUCT_DELETED, "Product updated successfully", oldProduct);
    }*/
}