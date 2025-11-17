namespace mongo_api.Entities;

public class MongoEntity
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string EndPoint { get; set; } = null!;
    public string Database { get; set; } = null!;
}