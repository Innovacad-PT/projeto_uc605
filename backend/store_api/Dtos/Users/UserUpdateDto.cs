using store_api.Entities;

namespace store_api.Dtos.Users;

public class UserUpdateDto(string? firstName, string? lastName, string? username, string? email, string? role) : IBaseDto<UserEntity>
{
    public string? FirstName { get; set; } = firstName;
    public string? LastName { get; set; } = lastName;
    public string? Username { get; set; } = username;
    public string? Email { get; set; } = email;
    public string? Role { get; set; } = role;
}