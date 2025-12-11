using mongo_api.Entities;

namespace mongo_api.Dtos.TechnicalSpecs;

public class UpdateTechSpecDTO(string? key)
{
    public string? Key { get; set; } = key; 

    public TechnicalSpecEntity ToEntity(Guid id)
    {
        return new(id, Key, null);
    }
}