using store_api.Dtos.Users;

namespace store_api.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }

    public UserEntity(Guid id, string firstName, string lastName, string username, string email)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Email = email;
    }   
    
    public void Update(UserUpdateDto user)
    {
        FirstName = user.FirstName ?? FirstName;
        LastName = user.LastName ?? LastName;
        Username = user.Username ?? Username;
        Email = user.Email ?? Email;;
    }

}