namespace store_api.Entities;

public class TechnicalSpecsEntity(Guid id, string? key, string? value)
{
    public Guid Id { get; set; } = id;
    public string? Key { get; set; } = key;
    public string? Value { get; set; } = value;
}