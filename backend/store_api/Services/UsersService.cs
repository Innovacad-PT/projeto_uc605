using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using store_api.Dtos.Users;
using store_api.Entities;
using store_api.Repositories;
using store_api.Utils;

namespace store_api.Services;

public class UsersService
{
    
    private readonly UsersRepository _repository = new UsersRepository();

    public Result<UserEntity?> Register(UserRegisterDto registerDto)
    {
        throw new NotImplementedException();
    }

    public Result<UserLoggedInDao?> Login(UserLoginDto loginDto, IConfiguration configuration)
    {
        if (loginDto.Type != LoginType.USERNAME && loginDto.Type != LoginType.EMAIL)
        {
            return new Failure<UserLoggedInDao?>(ResultCode.USER_NOT_LOGGED_IN, "The login type is not of 'username' nor 'email'.");
        }

        UserEntity? user = _repository.Login(loginDto);

        if (user == null)
        {
            return new Failure<UserLoggedInDao?>(ResultCode.USER_NOT_LOGGED_IN, "User not logged in");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecretKey"] ?? ""));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
            expires: DateTime.Now.AddDays(1), signingCredentials: creds);

        UserLoggedInDao dao = new(new JwtSecurityTokenHandler().WriteToken(token), user);

        return new Success<UserLoggedInDao?>(ResultCode.USER_LOGGED_IN, "User successfully logged in", dao);
    }

    public UserEntity? UpdateProfile(Guid id, UserProfileUpdateDto dto)
    {
        throw new NotImplementedException();
    }

    public UserEntity? ChangePassword(Guid id, UserChangePasswordDto dto)
    {
        throw new NotImplementedException();
    }

    public Result<IEnumerable<UserEntity>?> GetAll()
    {
        IEnumerable<UserEntity>? users = _repository.GetAll();

        if (users.Count() <= 0)
        {
            return new Success<IEnumerable<UserEntity>?>(ResultCode.USER_NOT_FOUND, "Users list is empty", users);
        }
        
        return new Success<IEnumerable<UserEntity>?>(ResultCode.USER_FOUND, "Users found", users);
    }

    public Result<UserEntity> GetById(Guid id)
    {
        var user =  _repository.GetById(id);

        if (user == null)
        {
            return new Failure<UserEntity>(ResultCode.USER_NOT_FOUND, $"User not found with id {id}");
        }

        return new Success<UserEntity>(ResultCode.USER_FOUND, "User found", user);
    }

    public Result<UserEntity?> Create(UserRegisterDto dto)
    {
        try
        {

            UserEntity? user = _repository.Add(dto.ToEntity());

            if (user == null)
            {
                return new Failure<UserEntity?>(ResultCode.USER_NOT_CREATED, $"User not created with id");
            }

            return new Success<UserEntity?>(ResultCode.USER_CREATED, "User created", user);
        }
        catch (Exception e)
        {
            return new Failure<UserEntity?>(ResultCode.USER_NOT_CREATED, e.Message);
        }
    }

    public Result<UserEntity?> Update(Guid id, UserUpdateDto dto)
    {
        
        UserEntity? user = _repository.Update(id, dto);

        if (user == null)
        {
            return new Failure<UserEntity?>(ResultCode.USER_NOT_UPDATED, $"User not found with id {id}");
        }
        
        return new Success<UserEntity?>(ResultCode.USER_UPDATED, "User updated", user);
    }

    public Result<UserEntity?> Delete(Guid id)
    {

        UserEntity? user = _repository.Delete(id);

        if (user == null)
        {
            return new Failure<UserEntity?>(ResultCode.USER_NOT_DELETED, $"User not found with id {id}");
        }
        
        return new Success<UserEntity?>(ResultCode.USER_DELETED, "User deleted", user);
    }

}