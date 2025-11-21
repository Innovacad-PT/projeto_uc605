namespace store_api.Dtos.Users;

public class UserChangePasswordDto
{
    public string PasswordHash { get; set; }
    public string NewPasswordHash { get; set; }

    public UserChangePasswordDto(String passwordHash, String newPasswordHash)
    {
        PasswordHash = passwordHash;
        NewPasswordHash = newPasswordHash;
    }
}