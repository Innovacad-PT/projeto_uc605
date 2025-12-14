using store_api.Dtos.Users;

namespace store_api.Entities;

public class UserEntity(
    Guid id,
    string firstName,
    string lastName,
    string username,
    string email,
    string role,
    DateTime createdAt, string password)
{
    public Guid Id { get; set; } = id;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Username { get; set; } = username;
    public string Email { get; set; } = email;
    public string Role { get; set; } = role;
    public DateTime CreatedAt { get; set; } = createdAt;
    public string Password { get; set; } = password;

}