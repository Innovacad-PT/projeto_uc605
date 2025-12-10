using System.Security.Cryptography;
using System.Text;
using mongo_api.Dtos;
using mongo_api.Entities;
using MongoDB.Driver;

namespace mongo_api.Repositories;

public class UserRepository(IMongoCollection<UserEntity> collection)
{
    private readonly IMongoCollection<UserEntity> _collection = collection;
    
    public async Task<List<UserEntity>> GetUsers()
    {
        var cursor = await _collection.FindAsync(_ => true);

        return await cursor.ToListAsync();
    }

    public async Task<UserEntity?> GetUser(Guid id)
    {
        var cursor = await _collection.FindAsync(u => u.Id.Equals(id));

        return await cursor.FirstOrDefaultAsync();
    }

    public async Task<UserEntity?> CreateUser(CreateUserDTO dto)
    {
        var passBytes = Encoding.UTF8.GetBytes(dto.Password);
        var passHash = SHA3_256.Create().ComputeHash(passBytes);
        dto.Password = Convert.ToHexString(passHash);

        await _collection.InsertOneAsync(dto.ToEntity());

        return await GetUser(dto.Id);
    }

    /*
    public async Task<UserEntity?> UpdateUser(UserDTO dto)
    {
        var updateResult = await _usersCollection.UpdateOneAsync(
            u => u.Id.Equals(dto.Id),
            Builders<UserEntity>.Update
                .Set(u => u.Name, dto.Name)
                .Set(u => u.Password, dto.Password)
        );

        if (updateResult.MatchedCount == 0 || updateResult.ModifiedCount == 0) return null;

        return await GetUser(dto.Id);
    }*/

    public async Task<UserEntity?> DeleteUser(Guid id)
    {
        var user = await GetUser(id);
        if (user == null) return null;

        var result = await _collection.DeleteOneAsync(u => u.Id.Equals(user.Id));

        return result.DeletedCount == 0 ? null : user;
    }
}