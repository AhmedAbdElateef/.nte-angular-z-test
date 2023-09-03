using realTimeServices.Protos; 
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

public class RealTimeChatClient
{
    private readonly RealTimeChatService.RealTimeChatServiceClient _client;

    public RealTimeChatClient()
    {
        
        var channel = GrpcChannel.ForAddress("https://localhost:7288", new GrpcChannelOptions
        
        {
            HttpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                SslProtocols = System.Security.Authentication.SslProtocols.Tls13 | System.Security.Authentication.SslProtocols.Tls12, // Specify the desired SSL/TLS protocol versions
                // You can adjust the SSL/TLS protocol versions according to your requirements
            }
        });
        
        _client = new RealTimeChatService.RealTimeChatServiceClient(channel);

    }

    public void SendMessage(string content, string sender, string receiver)
    {
        var request = new MessageRequest
        {
            Content = content,
            Sender = sender,
            Receiver = receiver
        };

        var response = _client.SendMessage(request);

        Console.WriteLine($"Received message: {response.Message}");
    }



public List<string> GetUsersIdes(string id)
{
    var request = new UserID
    {
        Userid = id
    };
    if(request.Userid != "undefined" && request.Userid != "" ){
        var response = _client.GetUserFollowingFollowers(request);

        var userIDsList = response.UserIDsLists.FirstOrDefault();
        if (userIDsList != null)
        {
            return userIDsList.UserIdsList.ToList();
        }
    }
    return new List<string>();
}

}











