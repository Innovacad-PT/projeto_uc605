namespace store_api.Dtos;

public class LoginDto
{
    public string User { get; set; }
    public string Password { get; set; }
    public string Type { get; set; }

    public LoginDto(string user, string password, string type)
    {
        User = user;
        Password = password;
        Type = type;
    }

    public bool IsValid()
    {
        return Type == "username" || Type == "email";
    }
}