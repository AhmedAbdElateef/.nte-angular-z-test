using backend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace backend.Services;

public class UserService {
    private readonly IMongoCollection<User> _userColection;
    private readonly IMongoCollection<Post> _postColection;

    public UserService(IOptions<MongoDBSettings> mongoDBSettings){
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _userColection = database.GetCollection<User>(mongoDBSettings.Value.UserCollection);
        _postColection = database.GetCollection<Post>(mongoDBSettings.Value.postsCollection);
    }

    public async Task CreateAsync(User user) {
        await _userColection.InsertOneAsync(user);
        return;
    }

    public async Task<List<User>> GetAsync(){
        return await _userColection.Find(new BsonDocument()).ToListAsync();
    }



    public async Task<List<Post>> GetUserPosts(string id){


        FilterDefinition<Post> filterPost = new BsonDocument
        {
            // { "creator", new BsonDocument("$ne", id) }, 
            { "creator", new BsonDocument("$regex", id) }
        };


        List<Post> posts = (await _postColection.FindAsync(filterPost))
            .ToList();


        if (posts is null ){
            posts = new List<Post>();
        } 

        return posts;

    }


    public async Task<User?> UpdateUser(string id, User newuser){
          return await _userColection.FindOneAndReplaceAsync(x => x._id == id, newuser);
    }
    public async  Task<User?>  GetUserByID(string id){
        return await _userColection.Find(x => x._id == id).FirstOrDefaultAsync();
    }


    public async  Task<User?>  GetUserByEmail(string email){
        return await _userColection.Find(x => x.email == email).FirstOrDefaultAsync();
    }

    public async Task DeleteAsync(string id){
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("_id", id);
        await _userColection.DeleteOneAsync(filter);
        return;
    }





}










