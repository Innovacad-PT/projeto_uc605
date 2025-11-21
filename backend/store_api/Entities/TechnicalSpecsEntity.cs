namespace store_api.Entities;

public class TechnicalSpecsEntity
{
    public Guid TechnicalSpecsId { get; set; }
    public Guid ProductId { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
}