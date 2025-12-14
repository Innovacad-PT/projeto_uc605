using System.Security.Cryptography;
using System.Text;
using mongo_api.Dtos;
using mongo_api.Entities;
using mongo_api.Utils;
using MongoDB.Driver;

namespace mongo_api.Repositories;

public class UserRepository(IMongoCollection<UserEntity> collection)
{
    private readonly IMongoCollection<UserEntity> _collection = collection;
    
    public async Task<List<UserEntity>> GetAll()
    {
        var cursor = await _collection.FindAsync(_ => true);

        return await cursor.ToListAsync();
    }

    public async Task<UserEntity?> GetById(Guid id)
    {
        var cursor = await _collection.FindAsync(u => u.Id.Equals(id));

        return await cursor.FirstOrDefaultAsync();
    }

    public async Task<UserEntity?> Create(CreateUserDTO dto)
    {
        var user = await GetById(dto.Id);
        if (user != null) return null;

        dto.Password = Crypto.ToHexString(dto.Password);

        await _collection.InsertOneAsync(dto.ToEntity());

        return await GetById(dto.Id);
    }

    public async Task<UserEntity?> Delete(Guid id)
    {
        var user = await GetById(id);
        if (user == null) return null;

        var result = await _collection.DeleteOneAsync(u => u.Id.Equals(user.Id));

        return result.DeletedCount == 0 ? null : user;
    }

    public async Task<UserEntity?> Update(Guid id, UpdateUserDTO dto)
    {
        var user = await GetById(id);
        if (user == null) return null;

        dto.Password = Crypto.ToHexString(dto.Password);

        var result = await _collection.UpdateOneAsync(
            b => b.Id.Equals(user.Id),
            Builders<UserEntity>.Update
                .Set(u => u.FirstName, dto.FirstName ?? user.FirstName)
                .Set(u => u.LastName, dto.LastName ?? user.LastName)
                .Set(u => u.Email, dto.Email ?? user.Email)
                .Set(u => u.Role, dto.Role ?? user.Role)
                .Set(u => u.Password, dto.Password ?? user.Password)
        );

        if (result.MatchedCount == 0 || result.ModifiedCount == 0) return null;

        return await GetById(id);
    }
}