using store_api.Controllers;
using store_api.Dtos;
using store_api.Dtos.Users;
using store_api.Entities;
using store_api.Utils;

namespace store_api.Repositories;

public class UsersRepository : IBaseRepository<UserEntity>
{
    static List<UserEntity> users = new ();

    public Result<UserEntity> Add(IBaseDto<UserEntity> entity)
    {
        throw new NotImplementedException();
    }

    public Result<UserEntity> Update(Guid id, IBaseDto<UserEntity> entity)
    {
        throw new NotImplementedException();
    }

    public Result<UserEntity> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Result<UserEntity> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Result<UserEntity> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Result<IEnumerable<UserEntity>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Result<UserEntity> GetByEmail(String email)
    {
        throw new NotImplementedException();
    }

    public Result<bool> UserExists(String email)
    {
        throw new NotImplementedException();
    }
}