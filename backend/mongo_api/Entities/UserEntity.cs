using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace mongo_api.Entities;

public class UserEntity(Guid id, string name, string userName, string password)
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    [SwaggerIgnore]
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string UserName { get; set; } = userName;
    public string Password { get; set; } = password;
}