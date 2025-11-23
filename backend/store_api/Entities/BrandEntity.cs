namespace store_api.Entities;

public class BrandEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public BrandEntity(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}