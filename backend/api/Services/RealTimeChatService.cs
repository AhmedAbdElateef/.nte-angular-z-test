using Grpc.Core;
using backend.Protos;
using backend.Models;

namespace backend.Services
{
    public class RealtimechatService : RealTimeChatService.RealTimeChatServiceBase
    {
        private readonly ILogger<RealtimechatService> logger;
        private readonly ChatService _chatService;

        public RealtimechatService(ChatService chatService,ILogger<RealtimechatService> logger)
        {
            _chatService = chatService;
            this.logger = logger;
        }

        public override Task<MessageResponse> SendMessage(MessageRequest request, ServerCallContext context)
        {
            logger.LogInformation($"New Message: DeviceId: {request.Sender} Location: {request.Receiver}");
            // return base.SendMessage(request, context);

            var msg = new Message {
                sender = request.Sender,
                recever = request.Receiver,
                content = request.Content
            };

            _ = _chatService.SendMessageAsync(msg, request.Sender, request.Receiver);

            return Task.FromResult(new MessageResponse { Message = request.Content });
        }



        public override async Task<UsersIDsListResponse> GetUserFollowingFollowers(UserID uid, ServerCallContext context)
        {

            if(uid.ToString() != "undefined" && uid.ToString() is not null){
                // logger.LogInformation($"New iiiiddd: DeviceId: {uid}");

                var userId = uid.Userid;
                var data = await _chatService.GetUserFollowingAndFollowers(userId);
                var userIDsList = new UserIDsList();
                userIDsList.UserIdsList.AddRange(data);

                var usersIDsListResponse = new UsersIDsListResponse();
                usersIDsListResponse.UserIDsLists.Add(userIDsList);

                return usersIDsListResponse;
            } else {
                throw new Exception("GetUser Follwing Error - userid is not defind");
            }
        }



    }
}