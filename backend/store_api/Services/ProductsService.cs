using store_api.Entities;
using store_api.Exceptions;
using store_api.Repositories;
using store_api.Utils;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace store_api.Services;

public class ProductsService
{
    private readonly ProductsRepository _productsRepository;
    private readonly BrandsService _brandsService;
    private readonly CategoriesService _categoriesService;
    private readonly DiscountService _discountsService;
    private readonly TechnicalSpecsService _technicalSpecsService;

    public ProductsService(IConfiguration configuration)
    {
        _productsRepository = new (configuration);
        _brandsService = new (configuration);
        _categoriesService = new(configuration);
        _discountsService = new (configuration);
        _technicalSpecsService = new(configuration);
    }

    public async Task<Result<ProductEntity?>> CreateProduct(ProductCreateDto dto)
    {
        try
        {
            Result<BrandEntity?> brand = await _brandsService.GetById(dto.BrandId);
            Result<CategoryEntity?> category = await _categoriesService.GetById(dto.CategoryId);

            if (brand is Failure<BrandEntity?>)
                return new Failure<ProductEntity?>(ResultCode.BRAND_NOT_FOUND,
                    $"The brand with id ({dto.BrandId}) does not exist");

            if (category is Failure<CategoryEntity?>)
                return new Failure<ProductEntity?>(ResultCode.CATEGORY_NOT_FOUND,
                    $"The category with id ({dto.CategoryId}) does not exist");

            List<TechnicalSpecsEntity> finalSpecs = new();

            if (dto.TechnicalSpecs != null)
            {
                foreach (var specId in dto.TechnicalSpecs)
                {
                    Result<TechnicalSpecsEntity?> specTemplate = await _technicalSpecsService.GetById(specId);
        
                    if (specTemplate is Failure<TechnicalSpecsEntity>)
                        return new Failure<ProductEntity?>(ResultCode.TECHNICAL_SPEC_NOT_FOUND,
                            $"The Technical Spec ID ({specId}) is invalid or does not exist as a template.");
            
                    finalSpecs.Add((specTemplate as Success<TechnicalSpecsEntity?>).Value);
                }
            }

            string? imageUrl = null;
            
            Console.WriteLine(dto.ImageFile);

            if (dto.ImageFile != null)
            {
                string folder = Path.Combine("wwwroot", "images");

                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                string filename = $"{dto.Id}{Path.GetExtension(dto.ImageFile.FileName)}";
                string filePath = Path.Combine(folder, filename);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }

                imageUrl = $"/images/{filename}";
            }

            var entity = new ProductEntity(
                dto.Id,
                dto.Name,
                (category as Success<CategoryEntity>)!.Value,
                (brand as Success<BrandEntity>)!.Value,
                finalSpecs,
                imageUrl ?? "",
                dto.Price,
                dto.Details ?? "",
                dto.Stock
            );

            ProductEntity? prod = await _productsRepository.Add(entity);

            if (prod == null)
            {
                return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_CREATED, "Product was not created");
            }

            return new Success<ProductEntity?>(ResultCode.PRODUCT_CREATED,
                "Product created successfully", prod);
        }
        catch (SameIdException e)
        {
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_CREATED,
                "Product with the same id already exists");
        }
        
    }

    public async Task<Result<ProductEntity?>> UpdateProduct(Guid id, ProductUpdateDto<ProductEntity> productDto){
        ProductEntity? productEntity = await _productsRepository.GetById(id);

        if (productEntity == null)
        {
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_FOUND,
                "Product with the specified id does not exist");
        }

        if (productDto.ImageFile != null)
        {
            Console.WriteLine("NAO NULO");
            Console.WriteLine(productDto.ImageFile);
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

        productEntity.Name  = productDto.Name ?? productEntity.Name;
        productEntity.Price = productDto.Price ?? productEntity.Price;
        productEntity.Stock = productDto.Stock ?? productEntity.Stock;
        
        List<TechnicalSpecsEntity> finalSpecs = new();
        
        foreach (var s in productDto.TechnicalSpecs)
        {
            Result<TechnicalSpecsEntity?> res = await _technicalSpecsService.GetById(s);

            if (res is Failure<TechnicalSpecsEntity>)
            {
                continue;
            }
            
            finalSpecs.Add((res as Success<TechnicalSpecsEntity?>).Value);
        }
        productEntity.TechnicalSpecs = productDto.TechnicalSpecs != null ? finalSpecs : productEntity.TechnicalSpecs;

        if (productDto.BrandId != null)
        {
            Result<BrandEntity?> res = await _brandsService.GetById(productDto.BrandId.Value);

            if (res is Success<BrandEntity> s)
            {
                productEntity.Brand = s.Value;
            }
        }
        
        if (productDto.CategoryId != null)
        {
            Result<CategoryEntity?> res = await _categoriesService.GetById(productDto.CategoryId.Value);

            if (res is Success<CategoryEntity> s)
            {
                productEntity.Category = s.Value;
            }
        }
        
        productEntity.Reviews = productDto.Reviews ?? productEntity.Reviews;
        productEntity.UpdatedAt = DateTime.Now;
        
        ProductEntity? product = await _productsRepository.UpdateWithImage(id, productEntity);

        if (product == null)
        {
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_UPDATED,
                "Product update failed after processing");
        }

        return new Success<ProductEntity?>(ResultCode.PRODUCT_UPDATED, "Product updated successfully", product);
    }

    public async Task<Result<ProductEntity?>> DeleteProduct(Guid id)
    {
        ProductEntity? product = await _productsRepository.Delete(id);

        if (product == null)
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_DELETED,
                "Product with the specified id does not exist");

        if (product.ImageUrl.Length > 0)
        {
            string folder = Path.Combine("wwwroot", "images");

            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                string oldFilePath = Path.Combine(folder, product.ImageUrl);
                if (File.Exists(oldFilePath)) File.Delete(oldFilePath);
            }
        }
        
        return new Success<ProductEntity?>(ResultCode.PRODUCT_DELETED,
            "Product deleted successfully", product);
    }

    public async Task<Result<ProductEntity?>> GetProductById(Guid id)
    {
        ProductEntity? product = await _productsRepository.GetById(id);

        if (product == null)
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_FOUND,
                "Product with the specified id does not exist");
        
        return new Success<ProductEntity?>(ResultCode.PRODUCT_FOUND,
            "Product found successfully", product);
    }

    public async Task<Result<IEnumerable<ProductEntity>?>> GetAllProducts(String search, String category, decimal minPice, decimal maxPrice){
        IEnumerable<ProductEntity>? result = await _productsRepository.GetAll();

        if (result == null || result!.ToList().Count <= 0)
            return new Success<IEnumerable<ProductEntity>?>(ResultCode.PRODUCT_NOT_FOUND,
                "Products list is empty", result);

        return new Success<IEnumerable<ProductEntity>?>(ResultCode.PRODUCT_FOUND,
            "Products list retrieved successfuly", result);
    }

    public async Task<Result<DiscountEntity?>> GetActiveDiscount(Guid productId)
    {
        Result<DiscountEntity?> product = await _discountsService.GetActiveDiscount(productId);

        if (product is Failure<DiscountEntity?>)
            return new Success<DiscountEntity?>(ResultCode.DISCOUNT_NOT_FOUND,
                "This product has no discount.", null);

        return new Success<DiscountEntity?>(ResultCode.PRODUCT_DISCOUNT_FOUND,
            "Discount found successfully", (product as Success<DiscountEntity>)?.Value);
    }

    public async Task<Result<ProductEntity?>> IncreaseStock(Guid productId, int amount){
        ProductEntity? product = await _productsRepository.IncreaseStock(productId, amount);

        if (product == null)
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_FOUND,
                "Product with the specified id does not exist");

        return new Success<ProductEntity?>(ResultCode.PRODUCT_STOCK_INCREASED,
            $"Product stock was increased by {amount}", product);
    }

    public async Task<Result<ProductEntity?>> DecreaseStock(Guid productId, int amount){
        ProductEntity? product = await _productsRepository.DecreaseStock(productId, amount);

        if (product == null)
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_FOUND,
                "Product with the specified id does not exist");

        return new Success<ProductEntity?>(ResultCode.PRODUCT_STOCK_DECREASED,
            $"Product stock was decreased by {amount}", product);
    }


    public async Task<Result<ProductEntity?>> AddTechnicalSpecs(Guid productId, List<Guid> list)
    {
        Result<ProductEntity?> productCheck = await GetProductById(productId);
        if (productCheck is Failure<ProductEntity?>)
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_FOUND,
                $"Product with the specified id ({productId}) does not exist");

        try
        {
            ProductEntity? product = await _productsRepository.AddSpecs(productId, list);

            if (product == null)
                return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_FOUND,
                    $"Product with the specified id ({productId}) does not exist");

            return new Success<ProductEntity?>(ResultCode.PRODUCT_TECHNICAL_SPECS_ADDED,
                $"Technical specs added to product with id {productId}", product);
        }
        catch (InvalidOperationException e)
        {
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_TECHNICAL_SPECS_NOT_ADDED, e.Message);
        }
    }
    
    public async Task<Result<ProductEntity?>> RemoveTechnicalSpec(Guid productId, Guid specId)
    {
        Result<ProductEntity?> productCheck = await GetProductById(productId);
        if (productCheck is Failure<ProductEntity?>)
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_FOUND,
                $"Product with the specified id ({productId}) does not exist");

        try
        {
            ProductEntity? product = await _productsRepository.RemoveSpec(productId, specId);

            if (product == null)
                return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_FOUND,
                    $"Product with the specified id ({productId}) does not exist");

            return new Success<ProductEntity?>(ResultCode.PRODUCT_TECHNICAL_SPECS_REMOVED,
                $"Technical specs removed to product with id {productId}", product);
        }
        catch (InvalidOperationException e)
        {
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_TECHNICAL_SPECS_NOT_ADDED, e.Message);
        }
    }

    public async Task<bool> CanCreateOrder(Guid productId, int quantity)
    {
        return await _productsRepository.CanCreateOrder(productId, quantity);
    }
}