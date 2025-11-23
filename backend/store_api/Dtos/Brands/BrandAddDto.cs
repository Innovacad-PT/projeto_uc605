using store_api.Entities;

namespace store_api.Dtos.Brands;

public class BrandAddDto<T> : IBaseDto<T>
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public BrandAddDto(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public BrandEntity ToEntity()
    {
        return new (Id, Name);
    }
}