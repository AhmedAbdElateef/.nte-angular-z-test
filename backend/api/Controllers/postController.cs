using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Models;
using backend.interfaces;
using System.Security.Claims;

namespace backend.Controllers;

[Controller]
[Route("/posts")]

public class PostController: Controller {
    private readonly IConfiguration _configuration;
    private readonly NotificationService _notificationService;
    private readonly PostService _postService;

    public PostController(PostService postService, 
                         NotificationService notificationService,
                          IConfiguration configuration) {
        _postService = postService;
        _configuration = configuration;
        _notificationService = notificationService;
    }






    [HttpPost]
    [Route(""), Authorize]
    public async Task<IActionResult> CreatePost([FromBody] CraeteOrUpdatePostInterface body){
        var post = new Post{};
        if (body.title == null 
            || body.message == null 
            || body.selectedFile == null ){

            return BadRequest(new {message = "proplem with provided body data."});
        }

        post.title = body.title;
        var userIDToken = User.FindFirstValue(ClaimTypes.NameIdentifier)?.ToString();
        post.creator = userIDToken;
        post.message = body.message;
        post.selectedFile = body.selectedFile;

        await _postService.CreateOnePostAsync(post);

            
        // create token
        if (post == null){
            return BadRequest(new {message = "some thing went worng!."});
        } 

        return Ok(new {post = post});


    }


[HttpPost]
[Route("{id}/commentPost"), Authorize]
public async Task<IActionResult> AddComment([FromRoute] string id, [FromBody] CommentBodyInterface body){
   if (body.value is null || id is null){
      return BadRequest(new {message = "proplem with provided body data."}); 
   }
   var post = await _postService.GetPostByID(id);

   if (post is null) return NotFound( new {message = "User Not Found", Success = false} );
   
   post.comments.Add(body.value);
   
   var npost = await _postService.UpdatePost(id, post);

   if (npost is null)   return BadRequest(new {message = "proplem with provided body data."}); 

    // Call Notification start
    var userIDToken = User.FindFirstValue(ClaimTypes.NameIdentifier)?.ToString();
    if (post.creator != null && userIDToken != null){

            var user = new User{};
            user = await _postService.GetUsByid(userIDToken);
            if(user is not null){
               var deat = user.name + "  Comment On Your Post";
               var us = new UserIn{name = user.name, avatar= user.imageUrl}; 

                var notification = new Notification
                {
                mainuid = post.creator,
                targetid = id,
                deatils = deat,
                user = us
                };


                await _notificationService.CreateNotification(notification);
            }

    }
    // Call Nofification End

   return Ok(new {data = post});

}



[HttpGet]
[Route("{id}")]
public async Task<IActionResult> GetPost([FromRoute] string id){
    if (id is null){
        return BadRequest(new {message = "proplem with provided body data."}); 
    }

    var post = new Post{};
    post = await _postService.GetPostByID(id);

   if (post is null) return NotFound( new {message = "User Not Found", Success = false} );

   return Ok(new {post = post});
}



[HttpGet]
[Route("search")]
public async Task<IActionResult> SearchForUsersPost([FromQuery] string searchQuery){
    if (searchQuery is null){
        return BadRequest(new {message = "proplem with provided body data."}); 
    }    

    var posts = new List<Post>();
    var users = new List<User>();

    (posts, users) = await _postService.Search(searchQuery);

    if (posts is null) return NotFound( new {message = "User Not Found", Success = false} );

    
   return Ok(new {posts = posts, user = users});
}




[HttpGet]
[Route("")] // Pagenation post
public async Task<IActionResult> GetPostsPageNationAsync([FromQuery] int Page, [FromQuery] string id){
        if (id == "undefined" ) return BadRequest( new {Message = "id is undefined ", Success = false} );
    
    var user = new User{};
    user = await _postService.GetUsByid(id);

    if(user is null || user._id is null){
        return NotFound(new {message = "user With given id is not found."});
    }

    var ides = user.following;
    ides.Add(user._id.ToString());

     return  Ok( _postService.Query(ides, Page));

}






[HttpPatch]
[Route("{id}"), Authorize]
public async Task<IActionResult> UpdatePost([FromRoute] string id, [FromBody] CraeteOrUpdatePostInterface body){
        // var post = new Post{};
        
        if (body.title == null 
            || body.creator == null 
            || body.message == null 
            || body.selectedFile == null ){

            return BadRequest(new {message = "proplem with provided body data."});
        }

        var userIDToken = User.FindFirstValue(ClaimTypes.NameIdentifier)?.ToString();
        if (userIDToken is null){
            return NotFound(new {message = "Not Authorized ."});
        }

  
        var post = new Post{};
        post = await _postService.GetPostByID(id);

        if(post is null){
            return NotFound(new {message = "post With given id is not found."});
        }
  
        if (userIDToken != post.creator){
            return Unauthorized(new {message = "you are not authorized."});
        }

  
  
        // add new data to post
        post.title = body.title;
        post.creator = body.creator;
        post.message = body.message;
        post.selectedFile = body.selectedFile;

        // update post 
        var upPost = await _postService.UpdatePost(id, post);
        if (upPost is null){
            return BadRequest(new {message = "can not update the post."});
        }

        return Ok(new {post = post});
}








[HttpPatch]
[Route("{id}/likePost"), Authorize]
public async Task<IActionResult> LikeDisLikePost([FromRoute] string id){
    
    var userIDToken = User.FindFirstValue(ClaimTypes.NameIdentifier)?.ToString();
    if (userIDToken is null){
        return NotFound(new {message = "Not Authorized ."});
    }
    var post = new Post{};
    post = await _postService.GetPostByID(id);

    if (post is null){
        return NotFound(new {message = "Post With given id is not found."});
    }

    if(post.likes.Contains(userIDToken)){
        post.likes.Remove(userIDToken);
    } else {
        post.likes.Add(userIDToken);
        // Call Notification
        if(post.creator != null){ 
            // await _notificationController.CreateNotification(post.creator, userIDToken, id, 0);
            var user = new User{};
            user = await _postService.GetUsByid(userIDToken);
            if(user is not null){
               var deat = user.name + "  Like Your Post";
               var us = new UserIn {
                    name = user.name,
                    avatar = user.imageUrl
                };
                var notification = new Notification
                {
                mainuid = post.creator,
                targetid = id,
                deatils = deat,
                user = us
                };

                await _notificationService.CreateNotification(notification);
            }
        }
    }

    // update the post
    var upPost = await _postService.UpdatePost(id, post);
    if (upPost is null){
        return NotFound(new {message = "Can Not Update the post."});
    }


    return Ok(new {post = post});

}






[HttpDelete]
[Route("{id}")]
public async Task<IActionResult> DeletePost([FromRoute] string id){
     var userIDToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
     if (userIDToken is null){
        return NotFound(new {message = "Not Authorized ."});
     }

    var post = new Post{};
    post = await _postService.GetPostByID(id);
    if (post is null){
        return NotFound(new {message = "Post With given id is not found."});
    }

    if(userIDToken != post.creator){
      return NotFound(new {message = "Not Authorized you are not the creator of this post."});
    }
    
    await _postService.DeletePostAsync(id);
    return Ok(new {message = "post deleted successfully."});

} 









}




