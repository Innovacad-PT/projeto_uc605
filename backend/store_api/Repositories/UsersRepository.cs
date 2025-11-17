using store_api.Dtos.Users;
using store_api.Entities;

namespace store_api.Repositories;

public class UsersRepository
{
    static List<UserEntity> users = new ();

    public async Task<List<UserEntity>> GetAll()
    {
        return users;
    }
    
    public async Task<UserEntity?> GetById(Guid id)
    {
        return users.FirstOrDefault((p) => p.Id == id);
    }

    public async Task<List<UserEntity>> Create(UserCreateDto user)
    {
        users.Add(user.ToEntity());
        
        return users;
    }

    public async Task<List<UserEntity>> Update(Guid id, UserUpdateDto user)
    {
        int userIndex  = users.FindIndex(x => x.Id == id);
        UserEntity oldUser = users.ElementAt(userIndex);

        // TODO: Check if oldProduct is updated without inserting the item again in list.
        oldUser.Update(user);
        
        return users;
    }

    public async Task<List<UserEntity>> Delete(String id)
    {
        int userIndex  = users.FindIndex(x => x.Id == Guid.Parse(id));
        users.RemoveAt(userIndex);
        return users;
    }
}