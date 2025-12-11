namespace store_api.Entities;

public class ProductTechnicalSpecsEntity
{
    public ProductTechnicalSpecsEntity(Guid productId, Guid technicalSpecsId, String key, String value)
    {
        ProductId = productId;
        TechnicalSpecsId = technicalSpecsId;
        Key = key;
        Value = value;
    }

    public Guid ProductId { get; set; }
    public Guid TechnicalSpecsId  { get; set; }
    public String? Key { get; set; }
    public String Value { get; set; }
}