using backend.Models;
using backend.Protos;
using Microsoft.Extensions.Options;
using MongoDB.Driver;


namespace backend.Services;

public class ChatService {
    // private readonly IMongoCollection<User> _userColection;
    private readonly IMongoCollection<UnReadedMessages> _unReadedmessageColection;
    private readonly IMongoCollection<Message> _messageColection;
    private readonly IMongoCollection<User> _userColection;

    public ChatService(IOptions<MongoDBSettings> mongoDBSettings){
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _unReadedmessageColection = database.GetCollection<UnReadedMessages>(mongoDBSettings.Value.UnMessagesCollection);
        _messageColection = database.GetCollection<Message>(mongoDBSettings.Value.MessageCollection);
        _userColection = database.GetCollection<User>(mongoDBSettings.Value.UserCollection);

    }


    public async Task SendMessageAsync(Message msg, string sender, string recever) {
        await _messageColection.InsertOneAsync(msg);

        setUpdateUnreadedMessagesBetweenUsers(sender, recever);
        return;
    }


public async Task<List<string>> GetUserFollowingAndFollowers(string userId)
{
 if (userId != "undefined" && userId != ""){
    var filter = Builders<User>.Filter.Eq(u => u._id, userId);
    var user = await _userColection.Find(filter).FirstOrDefaultAsync();


    var followingIds = user.following;
    var followersIds = user.followers;

    var followingFilter = Builders<User>.Filter.In(u => u._id, followingIds);
    var followersFilter = Builders<User>.Filter.In(u => u._id, followersIds);

    var following = await _userColection.Find(followingFilter).ToListAsync();
    var followers = await _userColection.Find(followersFilter).ToListAsync();

    var followingAndFollowers = new List<string>();

    foreach (var followee in following)
    {
        if (followee._id != null && !followingAndFollowers.Contains(followee._id))
        {
            followingAndFollowers.Add(followee._id);
        }
    }

    foreach (var follower in followers)
    {
        if (follower._id != null && !followingAndFollowers.Contains(follower._id))
        {
            followingAndFollowers.Add(follower._id);
        }
    }

    return followingAndFollowers;
    } else {
        throw new Exception("GetUserFollowingAndFollowers-error id is undefind");
    }
    
}


    public async void setUpdateUnreadedMessagesBetweenUsers(string sender,string recever)
    {
        var filter = Builders<UnReadedMessages>.Filter.And(
            Builders<UnReadedMessages>.Filter.Eq(x => x.mainUserid, recever),
            Builders<UnReadedMessages>.Filter.Eq(x => x.otherUserid, sender)
        );

        var update = Builders<UnReadedMessages>.Update
            .Set(x => x.isReaded, false)
            .Inc(x => x.numOfUneadedMessages, 1);

        var options = new FindOneAndUpdateOptions<UnReadedMessages>
        {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };

        var result = await _unReadedmessageColection.FindOneAndUpdateAsync(filter, update, options);

        if (result == null)
        {
            var newUnreadMsg = new UnReadedMessages
            {
                mainUserid = recever,
                otherUserid = sender,
                isReaded = false,
                numOfUneadedMessages = 1
            };

            await _unReadedmessageColection.InsertOneAsync(newUnreadMsg);
        }
    }



   public async Task<List<Message>> GetMessagesByNum(int from, string firstuid, string seconduid)
    {
        var senderFilter = Builders<Message>.Filter.Eq("sender", firstuid);
        var receverFilter = Builders<Message>.Filter.Eq("recever", seconduid);

        var senderFilter1 = Builders<Message>.Filter.Eq("recever", firstuid);
        var receverFilter1 = Builders<Message>.Filter.Eq("sender", seconduid);

        var combinedFilter = Builders<Message>
            .Filter.Or(
                Builders<Message>.Filter.And(senderFilter, receverFilter),
                Builders<Message>.Filter.And(senderFilter1, receverFilter1)
            );


        var sort = Builders<Message>.Sort.Descending("_id");
        var numOfReruningMessages = 8;
        var messages = await _messageColection
            .Find(combinedFilter) 
            .Sort(sort)
            .Skip(from * numOfReruningMessages)
            .Limit(numOfReruningMessages)
            .ToListAsync();


        messages.Reverse(); // Reverse the order of messages

        return messages;
    }

    public async Task<List<UnReadedMessages>> GetUserUnreadedmsgs(string userid)
    {

        var filter1 = Builders<UnReadedMessages>.Filter.Eq("mainUserid", userid);
        var filter2 = Builders<UnReadedMessages>.Filter.Eq("isReaded", false);

        var combinedFilter = Builders<UnReadedMessages>.Filter.And(filter1, filter2);

        var urms = await _unReadedmessageColection.Find(combinedFilter).ToListAsync();

        return urms;

    }


    public async Task<bool> MarkMsgsAsReaded(string otheruid,string mainuid)
    {
        var filter = Builders<UnReadedMessages>.Filter.And(
            Builders<UnReadedMessages>.Filter.Eq(x => x.mainUserid, mainuid),
            Builders<UnReadedMessages>.Filter.Eq(x => x.otherUserid, otheruid)
        );

        var update = Builders<UnReadedMessages>.Update
            .Set(x=> x.isReaded, true)
            .Set(x=> x.numOfUneadedMessages, 0);

        var options = new FindOneAndUpdateOptions<UnReadedMessages>
        {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };
        var result = await _unReadedmessageColection.FindOneAndUpdateAsync(filter, update, options);

        if (result == null)
        { 
            return false;            
        } else {
             return true; 
        }
    }
}