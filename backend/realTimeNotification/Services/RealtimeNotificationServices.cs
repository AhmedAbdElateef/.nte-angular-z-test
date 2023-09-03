

using System.Threading.Tasks;

using Grpc.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using realTimeNotification.Protos;


namespace realTimeNotification.Services
{
    public class NotificationService : NotificationGrpcService.NotificationGrpcServiceBase 
    {
        private readonly ILogger<NotificationService> logger;
        // private readonly NotificationSender notificationSender;
         private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(ILogger<NotificationService> logger,
        IHubContext<NotificationHub> hubContext
         )
        {
            this.logger = logger;
            // this.notificationSender = notificationSender;
             _hubContext = hubContext;

        }


    public override async Task<Google.Protobuf.WellKnownTypes.Empty> SendGrpcNotification(NotificationGrpcRequest request, ServerCallContext context)
    {
        // await  notificationSender.SendNotyfyToUser(request);
        await _hubContext.Clients.Group(request.Mainuid).SendAsync("ReceiveNotification", request);
        logger.LogInformation($"Not Id: {request.Id} Not : {request.Mainuid} Not tarid : {request.Targetid} Not deatils {request.Deatils}");
         return new Google.Protobuf.WellKnownTypes.Empty();
    }




    }
}