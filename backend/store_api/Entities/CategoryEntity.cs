namespace store_api.Entities;

public class CategoryEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    List<ProductEntity> Products { get; set; }
}