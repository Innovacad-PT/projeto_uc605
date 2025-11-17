using store_api.Entities;

namespace store_api.Dtos.Users;

public class UserCreateDto
{
    public Guid? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }

    public UserCreateDto(Guid? id, string firstName, string lastName, string username, string email)
    {

        if (id == null)
        {
            id =  Guid.NewGuid();
        }

        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Email = email;
    }

    public UserEntity ToEntity()
    {
        return new(Id!.Value, FirstName, LastName, Username, Email);
    }

}