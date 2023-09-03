using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Models;
using backend.interfaces;


// 
namespace backend.Controllers;

[Controller]
[Route("/chat")]

public class ChatController: Controller {
    private readonly IConfiguration _configuration;


    private readonly ChatService _chatService;

    public ChatController(ChatService chatService, IConfiguration configuration) {
        _chatService = chatService;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("sendmessage")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageInterface body) {
        var msg = new Message{};
        msg.content = body.content;
        msg.sender = body.sender;
        msg.recever = body.recever;

        await _chatService.SendMessageAsync(msg, body.sender, body.recever);
            
        // create token
        if (msg == null){
            return BadRequest();
        }

        return Ok(new {sucuss = true});
    }

    // getmessagesbynumBetweentwoUsers
    [HttpGet]
    [Route("getmsgsbynums")]
    public async Task<IActionResult> GetMessagesByNumsBetweenTwoUsers([FromQuery] string from, [FromQuery] string firstuid, [FromQuery] string seconduid)
    {
        if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(firstuid) || string.IsNullOrEmpty(seconduid))
        {
            return BadRequest(new { message = "Problem with provided query parameters." });
        }
        
        List<Message> msgs = await _chatService.GetMessagesByNum(int.Parse(from), firstuid, seconduid);
        
        return Ok(new { msgs });
    }


    // getuserunreadedmessages
    [HttpGet]
    [Route("get-user-unraededmsg")]
    public async Task<IActionResult> GetUserUnReadedMessage([FromQuery] string userid){
        if (string.IsNullOrEmpty(userid)){
            return BadRequest(new { message = "Problem with provided query parameters." });
        }

        List<UnReadedMessages> urm = await _chatService.GetUserUnreadedmsgs(userid);

        int totalUnreadMessageCount = urm.Sum(msg => msg.numOfUneadedMessages);

        return Ok(new {messages = urm, total = totalUnreadMessageCount});
    }


    // markmsgsasreaded
    [HttpGet]
    [Route("mark-msg-asreaded")]
    public async Task<IActionResult> MarkMessageasreaded([FromQuery] string mainuid, [FromQuery] string otheruid){
        if (string.IsNullOrEmpty(mainuid) || string.IsNullOrEmpty(otheruid)){
            return BadRequest(new { message = "Problem with provided query parameters." });
        }

        bool isMarked = await _chatService.MarkMsgsAsReaded(otheruid, mainuid);

        return Ok(new {isMarked});
    } 

}
