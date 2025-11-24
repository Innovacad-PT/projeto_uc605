using System.Runtime.InteropServices.JavaScript;
using store_api.Controllers;
using store_api.Dtos;
using store_api.Dtos.Users;
using store_api.Entities;
using store_api.Exceptions;
using store_api.Utils;

namespace store_api.Repositories;

public class UsersRepository : IBaseRepository<UserEntity>
{
    private readonly static List<UserEntity> _users = new ();

    public UserEntity? Add(UserEntity entity)
    {
        if (_users.Any(u => u.Id == entity.Id))
            throw new SameIdException("An user with the provided id already exists!");

        if (_users.Any(u => u.Username.Equals(entity.Username)))
            throw new SameNameException("An user with the provided username already exists!");

        if (_users.Any(u => u.Email.Equals(entity.Email)))
        {
            throw new SameEmailException("An user with the provided email already exists!");
        }

        _users.Add(entity);
        return entity;
    }

    public UserEntity? Update(Guid id, IBaseDto<UserEntity> dto)
    {
        var updateDto = dto as UserUpdateDto;
        
        if (updateDto == null)
            throw new InvalidDtoType("Invalid data transfer object type.");

        var entity = _users.FirstOrDefault((b) => b.Id == id);

        if (entity == null)
            return null;

        entity.FirstName = updateDto.FirstName ?? entity.FirstName;
        entity.LastName = updateDto.LastName ?? entity.LastName;
        entity.Username = updateDto.Username ?? entity.Username;
        entity.Email = updateDto.Email ?? entity.Email;
        entity.Role = updateDto.Role ?? entity.Role;

        return entity;
    }

    public UserEntity? Delete(Guid id)
    {
        var entity = _users.FirstOrDefault(u => u.Id == id);

        if (entity == null)
            return null;

        _users.Remove(entity);
        return entity;
    }

    public IEnumerable<UserEntity> GetAll()
    {
        return _users;
    }

    public IEnumerable<UserEntity>? GetAllByName(string? firstName, string? lastName)
    {
        var isFNameNullOrEmpty = string.IsNullOrEmpty(firstName);
        var isLNameNullOrEmpty = string.IsNullOrEmpty(lastName);

        switch (isFNameNullOrEmpty, isLNameNullOrEmpty)
        {
            case (false, false):
                return _users.Where(u => u.FirstName.Equals(firstName) && u.LastName.Equals(lastName));
            case (false, true):
                return _users.Where(u => u.FirstName.Equals(firstName));
            case (true, false):
                return _users.Where(u => u.LastName.Equals(lastName));
            default: return null;
        }
    }

    public UserEntity? GetById(Guid id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }

    public UserEntity? GetByEmail(String email)
    {
        return _users.FirstOrDefault(u => u.Email == email);
    }

    public UserEntity? GetByUsername(String username)
    {
        return _users.FirstOrDefault(u => u.Username == username);
    }

    public UserEntity? Login(IBaseDto<UserEntity> dto)
    {
        UserLoginDto loginDto = dto as UserLoginDto;
        
        if (loginDto == null)
        {
            return null;
        }

        UserEntity? user = null;
        
        if (loginDto.Type == LoginType.USERNAME)
        {
            user = _users.FirstOrDefault(u => u.Username == loginDto.Identifier);
        }

        if (loginDto.Type == LoginType.EMAIL)
        {
            user = _users.FirstOrDefault(u => u.Email == loginDto.Identifier);
        }

        if (user == null)
        {
            return null;
        }

        if (user.PasswordHash == loginDto.PasswordHash)
        {
            return user;
        }

        return null;
    }
}