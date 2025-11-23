namespace store_api.Entities;

public class FilterEntity<T>
{
    public String Key { get; set; }
    public T Value { get; set; }

    public FilterEntity(String key, T value)
    {
        Key = key;
        Value = value;
    }
}