using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace mongo_api.Entities;

public class UserEntity(Guid id, string? firstName, string? lastName, string? userName,
    string? email, string? role, DateTime createdAt, string? password) : IBaseEntity
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    [SwaggerIgnore]
    public Guid Id { get; set; } = id;
    public string? FirstName { get; set; } = firstName;
    public string? LastName { get; set; } = lastName;
    public string? UserName { get; set; } = userName;
    public string? Email { get; set; } = email;
    public string? Role { get; set; } = role;
    public DateTime CreatedAt { get; set; } = createdAt;
    public string? Password { get; set; } = password;
}