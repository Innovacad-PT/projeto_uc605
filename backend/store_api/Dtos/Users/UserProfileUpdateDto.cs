namespace store_api.Dtos.Users;

public class UserProfileUpdateDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }

    public UserProfileUpdateDto(Guid? id, string firstName, string lastName, string username, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Email = email;
    }
}