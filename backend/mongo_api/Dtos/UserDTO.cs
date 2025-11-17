using mongo_api.Entities;

namespace mongo_api.Dtos;

public class UserDTO(Guid id, string name, string userName, string password)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string UserName { get; set; } = userName;
    public string Password { get; set; } = password;

    public UserEntity ToUserEntity()
    {
        return new UserEntity(Id, Name, UserName, Password);
    }
}