using store_api.Entities;

namespace store_api.Dtos.Users;

public class UserLoggedInDao(string token, UserEntity user)
{
    public string Token { get; set; } = token;
    public UserEntity User { get; set; } = user;
}