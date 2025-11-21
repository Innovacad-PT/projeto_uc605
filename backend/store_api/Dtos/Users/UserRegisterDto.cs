namespace store_api.Dtos.Users;

public class UserRegisterDto
{
    public Guid? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }

    public UserRegisterDto(string firstName, string lastName, string username, string email)
    {

        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Email = email;
    }
}