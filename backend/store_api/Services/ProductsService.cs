using store_api.Entities;
using store_api.Exceptions;
using store_api.Repositories;
using store_api.Utils;

namespace store_api.Services;

public class ProductsService
{
    private readonly ProductsRepository _productsRepository = new();
    private readonly BrandsService _brandsService = new();
    private readonly CategoriesService _categoriesService = new();
    private readonly DiscountService _discountsService = new();

    public async Task<Result<ProductEntity?>> CreateProduct(ProductCreateDto dto)
    {
        try
        {
            Result<BrandEntity?> brand = _brandsService.GetById(dto.BrandId);
            Result<CategoryEntity?> category = _categoriesService.GetById(dto.CategoryId);

            if (brand is Failure<BrandEntity>)
            {
                return new Failure<ProductEntity?>(ResultCode.BRAND_NOT_FOUND,
                    $"The brand with id ({dto.BrandId}) does not exist");
            }

            if (category is Failure<CategoryEntity>)
            {
                return new Failure<ProductEntity?>(ResultCode.CATEGORY_NOT_FOUND,
                    $"The category with id ({dto.CategoryId}) does not exist");
            }

            string? imageUrl = null;

            if (dto.Image != null)
            {
                string folder = Path.Combine("wwwroot", "images");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string filename = $"{dto.Id}{Path.GetExtension(dto.Image.FileName)}";
                string filePath = Path.Combine(folder, filename);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }

                imageUrl = $"/images/{filename}";
            }

            var entity = new ProductEntity(dto.Id, dto.Name, (category as Success<CategoryEntity>).Value,
                (brand as Success<BrandEntity>).Value, dto.TechnicalSpecs, imageUrl ?? "", dto.Price,
                dto.Details ?? "");
            ProductEntity? prod = _productsRepository.Add(entity);

            return new Success<ProductEntity?>(ResultCode.PRODUCT_CREATED, "Product created successfully", prod);
        }
        catch (SameIdException e)
        {
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_CREATED, "Product with the same id already exists");
        }
        
    }

    public async Task<Result<ProductEntity?>> UpdateProduct(Guid id, ProductUpdateDto productDto) {
        ProductEntity? productEntity = _productsRepository.GetById(id);

        if (productDto.ImageFile != null)
        {
            string folder = Path.Combine("wwwroot", "images");

            if (!Directory.Exists(folder))
            {
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
                    await productDto.ImageFile.CopyToAsync(stream);
                }

                productEntity.ImageUrl = $"/images/{fileName}";
            }
        }

        ProductEntity? product = _productsRepository.Update(id, productDto);

        if (product == null)
        {
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_UPDATED,
                "Product with the specified id does not exist");
        }

        return new Success<ProductEntity?>(ResultCode.PRODUCT_UPDATED, "Product updated successfully", product);
    }

    public Result<ProductEntity?> DeleteProduct(Guid id)
    {
        ProductEntity? product = _productsRepository.Delete(id);

        if (product == null)
        {
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_DELETED, "Product with the specified id does not exist");
        }

        if (product.ImageUrl.Length > 0)
        {
            string folder = Path.Combine("wwwroot", "images");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                string oldFilePath = Path.Combine(folder, product.ImageUrl);
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }
        }
        
        return new Success<ProductEntity?>(ResultCode.PRODUCT_DELETED, "Product deleted successfully", product);
    }

    public Result<ProductEntity?> GetProductById(Guid id)
    {
        ProductEntity? product = _productsRepository.GetById(id);

        if (product == null)
        {
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_FOUND, "Product with the specified id does not exist");
        }
        
        return new Success<ProductEntity?>(ResultCode.PRODUCT_FOUND, "Product found successfully", product);
    }

    public async Task<Result<IEnumerable<ProductEntity>?>> GetAllProducts(String search, String category, decimal minPice, decimal maxPrice){
        IEnumerable<ProductEntity>? result = _productsRepository.GetAll();

        if (result.ToList().Count <= 0)
        {
            return new Success<IEnumerable<ProductEntity>?>(ResultCode.PRODUCT_NOT_FOUND, "Products list is empty", result);
        }

        return new Success<IEnumerable<ProductEntity>?>(ResultCode.PRODUCT_FOUND, "Products list retrieved successfuly", result);
    }

    public Result<DiscountEntity?> GetActiveDiscount(Guid productId)
    {
        Result<DiscountEntity?> product = _discountsService.GetActiveDiscount(productId);

        if (product is Failure<DiscountEntity?>)
        {
            return new Failure<DiscountEntity?>(ResultCode.PRODUCT_NOT_FOUND, "Product with the specified id does not exist");
        }

        return new Success<DiscountEntity?>(ResultCode.PRODUCT_DISCOUNT_FOUND, "Discount found successfully", (product as Success<DiscountEntity>).Value);
    }

    public Result<ProductEntity?> IncreaseStock(Guid productId, int amount){
        ProductEntity? product = _productsRepository.IncreaseStock(productId, amount);

        if (product == null)
        {
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_FOUND, "Product with the specified id does not exist");
        }

        return new Success<ProductEntity?>(ResultCode.PRODUCT_STOCK_INCREASED,
            $"Product stock was increased by {amount}", product);
    }

    public Result<ProductEntity?> DecreaseStock(Guid productId, int amount){
        ProductEntity? product = _productsRepository.DecreaseStock(productId, amount);

        if (product == null)
        {
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_FOUND, "Product with the specified id does not exist");
        }

        return new Success<ProductEntity?>(ResultCode.PRODUCT_STOCK_DECREASED,
            $"Product stock was decreased by {amount}", product);
    }

    public Result<ProductEntity?> AddTechnicalSpecs(Guid productId, List<TechnicalSpecsEntity> list){
        ProductEntity? product = _productsRepository.AddSpecs(productId, list);

        if (product == null)
        {
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_FOUND,  "Product with the specified id does not exist");
        }

        return new Success<ProductEntity?>(ResultCode.PRODUCT_TECHNICAL_SPECS_ADDED, $"Technical specs added to product with id {product}", product);
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