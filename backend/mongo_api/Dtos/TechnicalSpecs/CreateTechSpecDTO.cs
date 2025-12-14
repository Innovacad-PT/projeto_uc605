using mongo_api.Entities;

namespace mongo_api.Dtos.TechnicalSpecs;

public class CreateTechSpecDTO(string key)
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string? Key { get; set; } = key;
    

    public TechnicalSpecEntity ToEntity()
    {
        return new (Id, Key);
    }
}