using store_api.Entities;

namespace store_api.Dtos.Users;

public class UserRegisterDto : IBaseDto<UserEntity>
{
    public Guid? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public string PasswordHash { get; set; }

    public UserRegisterDto(string firstName, string lastName, string username, string email, string role, string passwordHash)
    {

        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Email = email;
        Role = role;
        CreatedAt = DateTime.Now;
        PasswordHash = passwordHash;
    }

    public UserEntity ToEntity()
    {
        return new(Id ?? Guid.NewGuid(), FirstName, LastName, Username, Email, Role, CreatedAt, PasswordHash);
    }
}