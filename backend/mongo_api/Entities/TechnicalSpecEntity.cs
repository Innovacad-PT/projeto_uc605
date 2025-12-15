using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace mongo_api.Entities;

public class TechnicalSpecEntity : IBaseEntity
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
    
    [BsonElement("Key")]
    public string Key { get; set; }
    
    [BsonElement("Value")]
    public string Value { get; set; }

    public TechnicalSpecEntity(Guid id, string key, string value)
    {
        Id = id;
        Key = key;
        Value = value;
    }
}