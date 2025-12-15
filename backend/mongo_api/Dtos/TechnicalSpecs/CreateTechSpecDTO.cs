using mongo_api.Entities;

namespace mongo_api.Dtos.TechnicalSpecs;

public class CreateTechSpecDTO(string key, string value)
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Key { get; set; } = key;
    public string Value { get; set; } = value;
    

    public TechnicalSpecEntity ToEntity()
    {
        return new (Id, Key, Value);
    }
}