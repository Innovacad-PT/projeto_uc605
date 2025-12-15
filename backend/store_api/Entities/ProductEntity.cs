using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using store_api.Repositories;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace store_api.Entities;

public class ProductEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public CategoryEntity? Category { get; set; }
    public BrandEntity? Brand { get; set; }
    public string ImageUrl { get; set; }
    public List<TechnicalSpecsEntity>? TechnicalSpecs { get; set; }
    public List<ReviewEntity>? Reviews { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ProductEntity(
        Guid id,
        string name,
        CategoryEntity category,
        BrandEntity brand,
        List<TechnicalSpecsEntity>? technicalSpecs,
        string imageUrl,
        decimal price,
        string? description,
        int stock)
    {
        Id = id;
        Name = name;
        Category = category;
        Brand = brand;
        ImageUrl = imageUrl;
        Price = price;
        Description = description;
        TechnicalSpecs = technicalSpecs ?? new List<TechnicalSpecsEntity>();

        Stock = stock;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Dictionary<String, dynamic> ToJson()
    {
        Dictionary<String, dynamic> content = new Dictionary<String, dynamic>();

        List<Guid> specIds = new List<Guid>();
        TechnicalSpecs.ForEach(s => specIds.Add(s.Id));
        
        Console.WriteLine(Brand);
        Console.WriteLine(Category);
        
        content.Add("id", Id);
        content.Add("name", Name);
        content.Add("description", Description);
        content.Add("imageUrl", ImageUrl);
        content.Add("price", Price);
        content.Add("stock", Stock);
        if (Category != null)
        {
            content.Add("categoryId", Category.Id);
        }
        
        if (Brand != null)
        {
            content.Add("brandId", Brand.Id);
        }
        
        content.Add("technicalSpecs", specIds);
        content.Add("reviews", Reviews);
        content.Add("createdAt", CreatedAt);
        content.Add("updatedAt", UpdatedAt);

        return content;
    }

    public static string? ToStringHelper(dynamic value)
    {
        return value?.ToString();
    }

    public static async Task<ProductEntity?> FromJson(IConfiguration config, Dictionary<String, dynamic> json)
    {
        BrandsRepository brandsRepository = new(config);
        CategoriesRepository categoriesRepository = new(config);
        TechnicalSpecsRepository technicalSpecsRepository = new(config);

        Guid id = Guid.Parse(ToStringHelper(json["id"]) ?? Guid.Empty.ToString());
        string name = ToStringHelper(json["name"]) ?? string.Empty;
        string description = ToStringHelper(json["description"]) ?? string.Empty;
        string imageUrl = json["imageUrl"].ToString();
        decimal price = decimal.Parse(ToStringHelper(json["price"]) ?? "0");
        int stock = int.Parse(ToStringHelper(json["stock"]) ?? "0");
        
        Guid brandId = Guid.Parse(ToStringHelper(json["brandId"]) ?? Guid.Empty.ToString());
        Guid categoryId = Guid.Parse(ToStringHelper(json["categoryId"]) ?? Guid.Empty.ToString());

        BrandEntity brand = (await brandsRepository.GetById(brandId))!;
        CategoryEntity category = (await categoriesRepository.GetById(categoryId))!;

        JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };
        
        List<Guid> specsIds = JsonSerializer.Deserialize<List<Guid>>(JsonSerializer.Serialize(json["technicalSpecs"]), _jsonOptions)!;
        
        List<TechnicalSpecsEntity> specs = new();

        foreach (var specsId in specsIds)
        {
            var result = (await technicalSpecsRepository.GetById(specsId))!;
            specs.Add(result);
        }
        
        Console.WriteLine($"IMAGEM: {imageUrl}");

        return new ProductEntity(
            id,
            name,
            category,
            brand,
            specs,
            imageUrl,
            price,
            description,
            stock
        );
    }
}