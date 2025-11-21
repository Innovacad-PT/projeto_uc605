using store_api.Utils;

namespace store_api.Dtos.Users;

public class UserLoginDto
{
    public String Identifier { get; set; }
    public String PasswordHash { get; set; }
    public LoginType Type { get; set; }
}