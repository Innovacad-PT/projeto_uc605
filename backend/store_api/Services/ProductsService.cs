using store_api.Entities;
using store_api.Exceptions;
using store_api.Repositories;
using store_api.Utils;

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

            if (brand is Failure<BrandEntity>)
                return new Failure<ProductEntity?>(ResultCode.BRAND_NOT_FOUND,
                    $"The brand with id ({dto.BrandId}) does not exist");

            if (category is Failure<CategoryEntity>)
                return new Failure<ProductEntity?>(ResultCode.CATEGORY_NOT_FOUND,
                    $"The category with id ({dto.CategoryId}) does not exist");
            
            List<ProductTechnicalSpecsEntity> finalSpecs = new();

            if (dto.TechnicalSpecs != null)
            {
                foreach (var specInput in dto.TechnicalSpecs)
                {
                    Result<TechnicalSpecsEntity?> specTemplate = await _technicalSpecsService.GetById(specInput.TechnicalSpecsId);
        
                    if (specTemplate is Failure<TechnicalSpecsEntity>)
                        return new Failure<ProductEntity?>(ResultCode.TECHNICAL_SPEC_NOT_FOUND,
                            $"The Technical Spec ID ({specInput.TechnicalSpecsId}) is invalid or does not exist as a template.");
            
                    ProductTechnicalSpecsEntity newSpecEntity = new ProductTechnicalSpecsEntity(
                        productId: dto.Id,
                        technicalSpecsId: specInput.TechnicalSpecsId,
                        value: specInput.Value,
                        key: (specTemplate as Success<TechnicalSpecsEntity>)!.Value.Key
                    );
            
                    finalSpecs.Add(newSpecEntity);
                }
            }

            string? imageUrl = null;

            if (dto.Image != null)
            {
                string folder = Path.Combine("wwwroot", "images");

                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                string filename = $"{dto.Id}{Path.GetExtension(dto.Image.FileName)}";
                string filePath = Path.Combine(folder, filename);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }

                imageUrl = $"/images/{filename}";
            }

            var entity = new ProductEntity(
                dto.Id,
                dto.Name,
                (category as Success<CategoryEntity>)!.Value,
                (brand as Success<BrandEntity>)!.Value,
                finalSpecs,
                imageUrl ?? "", dto.Price,
                dto.Details ?? "",
                dto.Stock
            );

            ProductEntity? prod = await _productsRepository.Add(entity);

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
        
        ProductEntity? product = await _productsRepository.Update(id, productDto);

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

        if (result.ToList().Count <= 0)
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


    public async Task<Result<ProductEntity?>> AddTechnicalSpecs(Guid productId, List<ProductTechnicalSpecsEntity> list)
    {
        Result<ProductEntity?> productCheck = await GetProductById(productId);
        if (productCheck is Failure<ProductEntity?>)
            return new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_FOUND,
                $"Product with the specified id ({productId}) does not exist");

        try
        {
            foreach (var spec in list)
            {
                spec.ProductId = productId;

                Result<TechnicalSpecsEntity?> specTemplate = await _technicalSpecsService.GetById(spec.TechnicalSpecsId);

                if (specTemplate is Failure<TechnicalSpecsEntity>)
                    return new Failure<ProductEntity?>(ResultCode.TECHNICAL_SPEC_NOT_FOUND,
                        $"The Technical Spec ID ({spec.TechnicalSpecsId}) is invalid or does not exist as a template.");


                spec.Key = (specTemplate as Success<TechnicalSpecsEntity>)?.Value.Key;
            }
        }
        catch (Exception e)
        {
            return new Failure<ProductEntity?>(ResultCode.TECHNICAL_SPEC_INVALID, $"Validation error on technical specs: {e.Message}");
        }

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

    public async Task<bool> CanCreateOrder(Guid productId, int quantity)
    {
        return await _productsRepository.CanCreateOrder(productId, quantity);
    }
}