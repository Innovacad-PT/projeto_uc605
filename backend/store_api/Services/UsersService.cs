using store_api.Dtos.Users;
using store_api.Entities;
using store_api.Repositories;
using store_api.Utils;

namespace store_api.Services;

public class UsersService
{
    
    private readonly UsersRepository _repository = new UsersRepository();

    public UserEntity? Register(UserRegisterDto registerDto)
    {
        throw new NotImplementedException();
    }

    public UserEntity? Login(UserLoginDto loginDto)
    {
        throw new NotImplementedException();
    }

    public UserEntity? GetProfile(Guid id)
    {
        throw new NotImplementedException();
    }

    public UserEntity? UpdateProfile(Guid id, UserProfileUpdateDto dto)
    {
        throw new NotImplementedException();
    }

    public UserEntity? ChangePassword(Guid id, UserChangePasswordDto dto)
    {
        throw new NotImplementedException();
    }

    /*public async Task<List<UserEntity>> GetAll()
    {
        return await _repository.GetAll();
    }

    public async Task<Result<UserEntity>> GetById(Guid id)
    {

        if (Guid.Empty == id)
        {
            return new Failure<UserEntity>(ResultCode.INVALID_GUID, $"O GUID passado estava vazio!");
        }

        var user = await _repository.GetById(id);

        if (user == null)
        {
            return new Failure<UserEntity>(ResultCode.USER_NOT_FOUND, $"Erro buscar utilizador com o id {id}");
        }

        return new Success<UserEntity>(ResultCode.USER_FOUND, "Retrieved user successfuly", user);
    }

    public async Task<List<UserEntity>> Create(UserCreateDto product)
    {
        return await _repository.Create(product);
    }

    public async Task<List<UserEntity>> Update(Guid id, UserUpdateDto product)
    {
        return await _repository.Update(id, product);
    }

    public async Task<List<UserEntity>> Delete(String id)
    {
        return await _repository.Delete(id);
    }*/

}