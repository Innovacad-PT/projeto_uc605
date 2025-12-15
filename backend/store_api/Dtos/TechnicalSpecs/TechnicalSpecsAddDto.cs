using store_api.Entities;

namespace store_api.Dtos;

public class TechnicalSpecsAddDto(string key, string value)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Key { get; set; } = key;
    public string Value { get; set; } = value;
    
    public TechnicalSpecsEntity ToEntity()
    {
        return new(Id, Key, Value);
    }

}