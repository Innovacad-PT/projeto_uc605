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

public class UsersService(IConfiguration configuration)
{
    private readonly UsersRepository _repository = new(configuration);

    public Result<UserEntity?> Register(UserRegisterDto registerDto)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<UserLoggedInDao?>> Login(UserLoginDto<UserEntity> loginDto)
    {
        if (loginDto.Type != LoginType.USERNAME && loginDto.Type != LoginType.EMAIL)
            return new Failure<UserLoggedInDao?>(ResultCode.USER_NOT_LOGGED_IN,
                "The login type is not of 'username' nor 'email'.");

        UserEntity? user = await _repository.Login(loginDto);

        if (user == null)
            return new Failure<UserLoggedInDao?>(ResultCode.USER_NOT_LOGGED_IN,
                "User not logged in");

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

    public async Task<UserEntity?> UpdateProfile(Guid id, UserProfileUpdateDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<UserEntity?> ChangePassword(Guid id, UserChangePasswordDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<IEnumerable<UserEntity>?>> GetAll()
    {
        IEnumerable<UserEntity>? users = await _repository.GetAll();

        if (!users.Any())
            return new Success<IEnumerable<UserEntity>?>(ResultCode.USER_NOT_FOUND,
                "Users list is empty", users);
        
        return new Success<IEnumerable<UserEntity>?>(ResultCode.USER_FOUND,
            "Users found", users);
    }

    public async Task<Result<UserEntity>> GetById(Guid id)
    {
        var user = await _repository.GetById(id);

        if (user == null)
            return new Failure<UserEntity>(ResultCode.USER_NOT_FOUND,
                $"User not found with id {id}");

        return new Success<UserEntity>(ResultCode.USER_FOUND,
            "User found", user);
    }

    public async Task<Result<UserEntity?>> Create(UserRegisterDto dto)
    {
        try
        {
            UserEntity? user = await _repository.Add(dto.ToEntity());

            if (user == null)
                return new Failure<UserEntity?>(ResultCode.USER_NOT_CREATED,
                    "User not created.");

            return new Success<UserEntity?>(ResultCode.USER_CREATED,
                "User created", user);
        }
        catch (Exception e)
        {
            return new Failure<UserEntity?>(ResultCode.USER_NOT_CREATED, e.Message);
        }
    }

    public async Task<Result<UserEntity?>> Update(Guid id, UserUpdateDto<UserEntity> dto)
    {
        
        UserEntity? user = await _repository.Update(id, dto);

        if (user == null)
            return new Failure<UserEntity?>(ResultCode.USER_NOT_UPDATED,
                $"User not found with id {id}");
        
        return new Success<UserEntity?>(ResultCode.USER_UPDATED,
            "User updated", user);
    }

    public async Task<Result<UserEntity?>> Delete(Guid id)
    {
        UserEntity? user = await _repository.Delete(id);

        if (user == null)
            return new Failure<UserEntity?>(ResultCode.USER_NOT_DELETED,
                $"User not found with id {id}");
        
        return new Success<UserEntity?>(ResultCode.USER_DELETED,
            "User deleted", user);
    }

}