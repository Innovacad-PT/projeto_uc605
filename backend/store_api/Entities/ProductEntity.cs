namespace store_api.Entities;

public class ProductEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public int Stock { get; set; }

    public CategoryEntity Category { get; set; }
    public BrandEntity Brand { get; set; }
    public string ImageUrl { get; set; }
    public List<TechnicalSpecsEntity> TechnicalSpecs { get; set; }
    public List<ReviewEntity> Reviews { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ProductEntity(
        Guid id,
        string name,
        CategoryEntity category,
        BrandEntity brand,
        List<TechnicalSpecsEntity>? technicalSpecs,
        string? imageUrl,
        double price,
        string description)
    {
        Id = id;
        Name = name;
        Category = category;
        Brand = brand;
        ImageUrl = imageUrl ?? "";
        Price = price;
        Description = description;
        TechnicalSpecs = technicalSpecs ?? new List<TechnicalSpecsEntity>();

        Stock = 0;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}