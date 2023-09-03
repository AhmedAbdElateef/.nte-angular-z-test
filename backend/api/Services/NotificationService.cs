using backend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using backend.Protos;
using Google.Protobuf.WellKnownTypes;

namespace backend.Services;

public class NotificationService {
    private readonly IMongoCollection<Notification> _notificationColection;

    public NotificationService(IOptions<MongoDBSettings> mongoDBSettings){
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _notificationColection = database.GetCollection<Notification>(mongoDBSettings.Value.NotificationCollection);
    }

    
    public async Task CreateNotification(Notification notification) {
        await _notificationColection.InsertOneAsync(notification);
        // GRPC Call The Realtime Notification
        var client = new RealtimeNotificationClient();
        // Todo--------------------
        var user = new Usergrpc
          {
            Name = notification.user.name,
            Avatar = notification.user.avatar
          };
        var request = new NotificationGrpcRequest{
            Id = notification._id,
            Deatils = notification.deatils,
            Mainuid = notification.mainuid,
            Targetid = notification.targetid,
            Isreded = false,
            CreatedAt = notification.createdAt != null ? Timestamp.FromDateTime(notification.createdAt.Value.ToUniversalTime()) : null,
            User = user      
            };
        
        client.SendGrpcNotification( request  );
        return;
    }

    public async Task<List<Notification>> GetUserNotification(string uid){
        var filter = Builders<Notification>.Filter
                     .Regex("mainuid", new BsonRegularExpression(uid, "i"));

        var notifications = await _notificationColection
                    .Find(filter)
                    .SortByDescending(p => p.createdAt)
                    .ToListAsync();

        
        return notifications;
    }


    public async Task<bool> MarkNotificationsAsReaded(string uid)
    {
        var filter = Builders<Notification>.Filter
                    .Regex("mainuid", new BsonRegularExpression(uid, "i"));
        var update = Builders<Notification>.Update
                    .Set(x => x.isreded, true);

        var result = await _notificationColection.UpdateManyAsync(filter, update);
        
        if (result == null) {  return false; } else { return true; }

    }



}

















