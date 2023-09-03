using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models;

public class UnReadedMessages {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id {get; set;}
    public string mainUserid { get; set; } = null!;
    public string otherUserid { get; set; } = null!;// userid1+userid2 or userid2+userid1
    public int numOfUneadedMessages { get; set; } = 0;
    public bool isReaded { get; set; } = false;
}