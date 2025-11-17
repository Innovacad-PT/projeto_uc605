using Microsoft.AspNetCore.Authorization;
using store_api.Dtos.Products;
using store_api.Entities;

namespace store_api.Repositories;

public class ProductsRepository
{
    
    static List<ProductEntity> products = new ([
        new(Guid.NewGuid(), "Pipocas Doces", 3.99, "Pipocas doces com manteiga", ""),
        new(Guid.NewGuid(), "Pipocas Salgadas", 3.99, "Pipocas doces com sal", ""),
        new(Guid.NewGuid(), "Pão Caseiro", 3.99, "", ""),
        new(Guid.NewGuid(), "Salsichas Frankfurt", 3.99, "", ""),
    ]);

    public async Task<List<ProductEntity>> GetAll()
    {
        return products;
    }
    
    public async Task<ProductEntity?> GetById(Guid id)
    {
        return products.FirstOrDefault((p) => p.Id == id);
    }

    public async Task<ProductEntity> CreateProduct(ProductCreateDto product)
    {

        ProductEntity newProduct = product.ToEntity();
        products.Add(newProduct);
        
        return newProduct;
    }

    public async Task<ProductEntity?> UpdateProduct(Guid id, ProductUpdateDto product)
    {
        int productIndex  = products.FindIndex(x => x.Id == id);

        if (productIndex == -1) return null;
        
        ProductEntity oldProduct = products.ElementAt(productIndex);

        oldProduct.Update(product);
        
        return oldProduct;
    }

    public async Task<ProductEntity?> DeleteProduct(String id)
    {
        int productIndex  = products.FindIndex(x => x.Id == Guid.Parse(id));

        if (productIndex == -1)
        {
            return null;
        }

        ProductEntity oldProduct = products.ElementAt(productIndex);    
        products.RemoveAt(productIndex);
        return oldProduct;
    }
}