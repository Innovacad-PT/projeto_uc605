using mongo_api.Entities;

namespace mongo_api.Dtos;

public class UpdateUserDTO(string? firstName, string? lastName, string? userName,
    string? role, string? email, string? password)
{
    public string? FirstName { get; set; } = firstName;
    public string? LastName { get; set; } = lastName;
    public string? UserName { get; set; } = userName;
    public string? Role { get; set; } = role;
    public string? Email { get; set; } = email;
    public string? Password { get; set; } = password;

    public UserEntity ToEntity(Guid userId, DateTime createdAt)
    {
        return new(userId, FirstName, LastName, UserName, Email, Role, createdAt, Password);
    }
}