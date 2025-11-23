namespace store_api.Entities;

public class CategoryEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public CategoryEntity(Guid id, string name)
    {
         Id = id;
         Name = name;   
    }
}