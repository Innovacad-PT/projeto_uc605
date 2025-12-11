using mongo_api.Entities;

namespace mongo_api.Dtos.Brands;

public class UpdateBrandDTO(string? name)
{
    public string? Name { get; set; } = name;

    public BrandEntity ToEntity(Guid id)
    {
        return new(id, Name);
    }
}