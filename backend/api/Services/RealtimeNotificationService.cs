using backend.Protos;
using Grpc.Net.Client;

public class RealtimeNotificationClient
{
    private readonly NotificationGrpcService.NotificationGrpcServiceClient _client;

    public RealtimeNotificationClient()
    {
        var channel = GrpcChannel.ForAddress("https://localhost:8090");
        _client = new NotificationGrpcService.NotificationGrpcServiceClient(channel);
    }

    public void SendGrpcNotification( NotificationGrpcRequest request  )
    {
         _client.SendGrpcNotification(request);
    }

}