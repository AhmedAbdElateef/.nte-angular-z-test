using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

// title string
// message string
// creator string | mongose.objid
// selectedFile string
// likes [string] default []
// comments [string] default []
// createdAt Date() default Date()

namespace backend.Models;

public class Post {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }
    [BsonElement("title")]
    public string? title { get; set; }
    public string? creator { get; set; }
    [BsonElement("message")]
    public string? message { get; set; }
    public string? selectedFile { get; set; }
    public List<string> likes { get; set; } = new List<string>{};
    public List<string> comments { get; set; } = new List<string>{};
    public DateTime? createdAt { get; set; } =  DateTime.Now;

}
























