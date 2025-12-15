namespace store_api.Entities;

public class ProductTechnicalSpecsEntity
{
    public ProductTechnicalSpecsEntity(Guid id, String key, String value)
    {
        Id = id;
        Key = key;
        Value = value;
    }

    public Guid Id  { get; set; }
    public String? Key { get; set; }
    public String Value { get; set; }
}