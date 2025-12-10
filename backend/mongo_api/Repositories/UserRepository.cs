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
        dto.Password = Crypto.ToHexString(dto.Password);

        await _collection.InsertOneAsync(dto.ToEntity());

        return await GetById(dto.Id);
    }

    /*
    public async Task<UserEntity?> Update(UserDTO dto)
    {
        var updateResult = await _usersCollection.UpdateOneAsync(
            u => u.Id.Equals(dto.Id),
            Builders<UserEntity>.Update
                .Set(u => u.Name, dto.Name)
                .Set(u => u.Password, dto.Password)
        );

        if (updateResult.MatchedCount == 0 || updateResult.ModifiedCount == 0) return null;

        return await GetById(dto.Id);
    }*/

    public async Task<UserEntity?> Delete(Guid id)
    {
        var user = await GetById(id);
        if (user == null) return null;

        var result = await _collection.DeleteOneAsync(u => u.Id.Equals(user.Id));

        return result.DeletedCount == 0 ? null : user;
    }
}