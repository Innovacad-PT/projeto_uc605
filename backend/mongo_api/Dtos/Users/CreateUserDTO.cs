using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.Annotations;

using mongo_api.Entities;

namespace mongo_api.Dtos;

public class CreateUserDTO(string firstName, string lastName, string userName,
    string email, string password)
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    [SwaggerIgnore]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string UserName { get; set; } = userName;
    public string Email { get; set; } = email;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Password { get; set; } = password;

    public UserEntity ToEntity()
    {
        return new(Id, FirstName, LastName, UserName, Email, CreatedAt, Password);
    }
}