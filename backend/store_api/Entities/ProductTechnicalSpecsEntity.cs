namespace store_api.Entities;

public class ProductTechnicalSpecsEntity
{
    public Guid ProductId { get; set; }
    public Guid TechnicalSpecsId  { get; set; }
    public String Key { get; set; }
    public String Value { get; set; }
}