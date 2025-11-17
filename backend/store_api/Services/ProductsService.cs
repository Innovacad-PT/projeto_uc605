using store_api.Dtos.Products;
using store_api.Entities;
using store_api.Repositories;
using store_api.Utils;

namespace store_api.Services;

public class ProductsService
{
    private readonly ProductsRepository _productsRepository = new ProductsRepository();

    public async Task<Result<List<ProductEntity>>> GetAll()
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

        var newProduct = await _productsRepository.CreateProduct(product);

        return new Success<ProductEntity>(ResultCode.PRODUCT_CREATED, "Product created successfully", newProduct);
    }

    public async Task<Result<ProductEntity>> UpdateProduct(Guid id, ProductUpdateDto product)
    {
        var newProduct = await _productsRepository.UpdateProduct(id, product);

        if(newProduct == null) return new Failure<ProductEntity>(ResultCode.PRODUCT_NOT_FOUND, "Product not found.");

        return new Success<ProductEntity>(ResultCode.PRODUCT_UPDATED, "Product updated successfully", newProduct);
    }

    public async Task<Result<ProductEntity>> DeleteProduct(String id)
    {
        var oldProduct = await _productsRepository.DeleteProduct(id);

        if(oldProduct == null) return new Failure<ProductEntity>(ResultCode.PRODUCT_NOT_FOUND, "Product not found.");

        return new Success<ProductEntity>(ResultCode.PRODUCT_DELETED, "Product updated successfully", oldProduct);
    }
}