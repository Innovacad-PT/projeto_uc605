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

    public async Task<Result<ProductEntity?>> UpdateProduct(Guid id, ProductUpdateDto productDto){
        ProductEntity? productEntity = _productsRepository.GetById(id);

        if (productEntity == null)
        {
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_FOUND,
                "Product with the specified id does not exist");
        }

        if (productDto.ImageFile != null)
        {
            string folder = Path.Combine("wwwroot", "images");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (!string.IsNullOrEmpty(productEntity.ImageUrl))
            {
                string oldFileName = productEntity.ImageUrl.Replace("/images/", "");
                string oldFilePath = Path.Combine(folder, oldFileName);
                
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }

            string fileExtension = Path.GetExtension(productDto.ImageFile.FileName);
            string newFileName = $"{productEntity.Id}{fileExtension}";

            string newFilePath = Path.Combine(folder, newFileName);

            using (var stream = new FileStream(newFilePath, FileMode.Create))
            {
                await productDto.ImageFile.CopyToAsync(stream);
            }

            productEntity.ImageUrl = $"/images/{newFileName}";
        }
        
        ProductEntity? product = _productsRepository.Update(id, productDto);

        if (product == null)
        {
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_UPDATED,
                "Product update failed after processing");
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
            return new Success<DiscountEntity?>(ResultCode.DISCOUNT_NOT_FOUND, "This product has no discount.", null);
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

    public Result<ProductEntity?> AddTechnicalSpecs(Guid productId, List<ProductTechnicalSpecsEntity> list){
        ProductEntity? product = _productsRepository.AddSpecs(productId, list);

        if (product == null)
        {
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_FOUND,  "Product with the specified id does not exist");
        }

        return new Success<ProductEntity?>(ResultCode.PRODUCT_TECHNICAL_SPECS_ADDED, $"Technical specs added to product with id {product}", product);
    }

    public async Task<bool> CanCreateOrder(Guid productId, int quantity)
    {
        bool can = _productsRepository.CanCreateOrder(productId, quantity);

        return can;
    }
}