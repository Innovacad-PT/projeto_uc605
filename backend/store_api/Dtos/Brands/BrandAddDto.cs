namespace store_api.Dtos.Brands;

public class BrandAddDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public BrandAddDto(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
}