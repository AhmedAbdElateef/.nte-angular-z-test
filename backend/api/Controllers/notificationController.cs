using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Models;
using backend.interfaces;

namespace backend.Controllers;


[Controller]
[Route("/notification")]

public class NotificationController: Controller {


    private readonly NotificationService _notificationService;

    public NotificationController(NotificationService notificationService) {
        _notificationService = notificationService;
    }
    


    [HttpGet]
    [Route("{id}")]
     public async Task<IActionResult> GetUserNotification([FromRoute] string id){

        if (id is null){
            return BadRequest(new {message = "proplem with provided body data."}); 
        }

        List<Notification>  notifications = await _notificationService.GetUserNotification(id);

        if (notifications is null) return NotFound( new {message = "No notificaion yet.", Success = false} );
    
        // var responseNotifications = notifications.Select(notification => new NotificationRresponseInterface
        // {
        //     _id = notification._id,
        //     mainuid = notification.mainuid,
        //     otheruid = notification.otheruid,
        //     targetid = notification.targetid,
        //     isreded = notification.isreded,
        //     Type = notification.Type.ToString()
        // }).ToList();

        // responseNotifications.Reverse();


        return Ok(new { notifications  });
    
    }


    [HttpGet]
    [Route("mark-notification-asreaded")]
    public async Task<IActionResult> MarkNotifyAsReaded([FromQuery] string id)
    {
        if(string.IsNullOrEmpty(id)){
            return BadRequest(new { message = "Problem with provided query parameters." });
        }

        bool isMarked = await _notificationService.MarkNotificationsAsReaded(id);
        if(!isMarked){
            return BadRequest(new { message = "Problem can not mark notificaion as readed." });
        }        

        // return Ok(new {isMarked});

        // if is marked return the new notification
        List<Notification>  notifications = await _notificationService.GetUserNotification(id);

        if (notifications is null) return NotFound( new {message = "No notificaion yet.", Success = false} );
    
        // var responseNotifications = notifications.Select(notification => new NotificationRresponseInterface
        // {
        //     _id = notification._id,
        //     mainuid = notification.mainuid,
        //     otheruid = notification.otheruid,
        //     targetid = notification.targetid,
        //     isreded = notification.isreded,
        //     Type = notification.Type.ToString()
        // }).ToList();

        return Ok(new { notifications  });
    }





} 












