using backend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace backend.Services;


public class PostService {
    private readonly IMongoCollection<Post> _postColection;
    private readonly IMongoCollection<User> _userColection;

    public PostService(IOptions<MongoDBSettings> mongoDBSettings){
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _postColection = database.GetCollection<Post>(mongoDBSettings.Value.postsCollection);
        _userColection = database.GetCollection<User>(mongoDBSettings.Value.UserCollection);

    }



    public async Task CreateOnePostAsync(Post post) {
        await _postColection.InsertOneAsync(post);
        return;
    }

    public async Task<(List<Post>,List<User>)> Search(string searchQuery){


        FilterDefinition<Post> filterPost = new BsonDocument
        {
            { "title", new BsonDocument("$ne", searchQuery) }, 
            { "message", new BsonDocument("$regex", searchQuery) }
        };


        FilterDefinition<User> filterUser = new BsonDocument
        {
            { "name", new BsonDocument("$ne", searchQuery) }, 
            { "email", new BsonDocument("$regex", searchQuery) }
        };



        List<Post> posts = (await _postColection.FindAsync(filterPost))
            .ToList();

        List<User> users = (await _userColection.FindAsync(filterUser))
            .ToList();


        if (posts is null ){
            posts = new List<Post>();
        } else if (users is null){
            users = new List<User>();
        }

        return (posts,users);

    }

    public async Task<Post?> UpdatePost(string id, Post newpost){
          return await _postColection.FindOneAndReplaceAsync(x => x._id == id, newpost);
    }
    public async  Task<Post?>  GetPostByID(string id){
        return await _postColection.Find(x => x._id == id).FirstOrDefaultAsync();
    }

    public async  Task<User?>  GetUsByid(string id){
        return await _userColection.Find(x => x._id == id).FirstOrDefaultAsync();
    }

    public async Task DeletePostAsync(string id){
        FilterDefinition<Post> filter = Builders<Post>.Filter.Eq("Id", id);
        await _postColection.DeleteOneAsync(filter);
        return;
    }


    public Object Query(List<string> ides, int? queryPage)
        {
            var filter = Builders<Post>.Filter.Empty;

            foreach (var id in ides)
            {
                filter |= Builders<Post>.Filter.Regex("creator", new BsonRegularExpression(id, "i"));
            }


            var sort = Builders<Post>.Sort.Descending("_id");
            var find = _postColection.Find(filter).Sort(sort);

 

            int currentPage = queryPage.GetValueOrDefault(1) == 0 ? 1 : queryPage.GetValueOrDefault(1);
            //int currentPage = 3;
            int perPage = 3;
            var numberOfPages = find.CountDocuments() / perPage;

            return new
            {
                data = find.Skip((currentPage - 1) * perPage).Limit(perPage).ToList(),
                numberOfPages,
                currentPage,
            };
        }
    




}




















