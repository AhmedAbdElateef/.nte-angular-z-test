using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Models;
using backend.interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace backend.Controllers;

[Controller]
[Route("/user")]

public class UserController: Controller {
    private readonly IConfiguration _configuration;
    private readonly NotificationService _notificationService;

    private readonly UserService _userService;

    public UserController(UserService userService, 
                           NotificationService notificationService,
                          IConfiguration configuration) {
        _userService = userService;
        _configuration = configuration;
        _notificationService = notificationService;
    }

    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> CreateAccount([FromBody] CreateUserInterface body) {
        var user = new User{};
        if (body.firstName == null || body.lastName == null || body.email == null || body.password == null ){
            return BadRequest(new {message = "proplem with provided body data."});
        }
        user.name = body.firstName +  body.lastName;
        user.email = body.email;
        user.password = user.EncryptPasswordBase64(body.password);
        
        var checkuser = await _userService.GetUserByEmail(body.email);
        if (checkuser is not null)
        {
            return BadRequest(new {message = "User Already Exist."});
        }

        await _userService.CreateAsync(user);

            
        // create token
        if (user == null){
            return BadRequest();
        }

            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user._id ?? throw new InvalidOperationException()),
                    new Claim(ClaimTypes.Name, user.name ?? throw new InvalidOperationException()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user._id.ToString())
                };
                    var tokenSecret = _configuration.GetValue<string>("JwtSecret:Secret");

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret ?? throw new InvalidOperationException()));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var expires = DateTime.Now.AddHours(60);

                    var token = new JwtSecurityToken(
                            issuer: "https://localhost:5001",
                            audience: "https://localhost:5001",
                            claims: claims,
                            expires: expires,
                            signingCredentials: creds
                        );
       

                // end of token
        

        //return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        return Ok(new {result = user, token = new JwtSecurityTokenHandler().WriteToken(token)});
    }


    [HttpPost]
    [Route("signin")]
    public async Task<IActionResult> LogInUser([FromBody] LoginInterFace body)
    {

        if (body.email == null || body.password == null ){
            return BadRequest(new {message = "proplem with provided body data."});
        }

        var user = await _userService.GetUserByEmail(body.email);
        var decodedPassword = user?.DecryptPasswordBase64(user.password);
        if (user is null)
        {
            return NotFound();
        } else if (decodedPassword != body.password){
            return BadRequest(new {message = "given email Or password not correct"});
        } else {
            // create token
            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user._id ?? throw new InvalidOperationException()),
                    new Claim(ClaimTypes.Name, user.name ?? throw new InvalidOperationException()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user._id.ToString())
                };
                    var tokenSecret = _configuration.GetValue<string>("JwtSecret:Secret");

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret ?? throw new InvalidOperationException()));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var expires = DateTime.Now.AddHours(60);

                    var token = new JwtSecurityToken(
                            issuer: "https://localhost:5001",
                            audience: "https://localhost:5001",
                            claims: claims,
                            expires: expires,
                            signingCredentials: creds
                        );
       
                // end of token
         return Ok(new {result = user, token = new JwtSecurityTokenHandler().WriteToken(token)});

        }


    }


    [HttpGet]
    [Route("getUser/{id}")]
    public async Task<IActionResult> GetUserById([FromRoute] string id){
      try
       {
        var user = new User{};
        var userPosts = new List<Post>(); 
        // 
 
        user = await _userService.GetUserByID(id);
        userPosts = await _userService.GetUserPosts(id); 

        if (user is null){
            return NotFound(new {message = "user with given id is not found!."});
        } else if (userPosts is null){
            userPosts = new List<Post>();
        }

        // ruturn data
        return Ok(new {user = user, posts = userPosts});
        }
        catch 
        {
            return BadRequest(new {message = "some thing went worng!."});
        }
    }


// GET /getSug
// id from query
// json({users})
    [HttpGet]
    [Route("getSug")]
    public async Task<IActionResult> GetSugUsers([FromQuery] string id)
    {
        try
        {
         if (id == "undefined" ) return BadRequest( new {Message = "id is undefined ", Success = false} );
            // get list of the users that our user follow them
            // get list of thair followers & following
         var mainUser = await _userService.GetUserByID(id);
                
         if (mainUser is null) return NotFound( new {Message = "User Not Found", Success = false} );

         var FollowingLIST = mainUser.following;
         
         if (FollowingLIST == null) return NotFound( new {Message = "null follwoing list for user", Success = false} );

         var FoloUsersList = new List<User>{};

         foreach (var Uid in FollowingLIST)
         {
             var getUserFollowing = await _userService.GetUserByID(Uid);
             //Console.WriteLine(getUserFollowing?.UserName);
             if (getUserFollowing != null){
                FoloUsersList.Add(getUserFollowing); 
             }         
         }
        
        // start
        // use FollowingLIST as refrence
        var usersidesforsug = new List<string>{};
        var FinalUsers = new List<User>{};

        foreach (var us in FoloUsersList)
        {

            if (us.followers != null && mainUser._id != null) {
             foreach (var ids in us.followers)
             {
                if (usersidesforsug.Contains(ids)|| ids == mainUser._id.ToString()) continue;
                var gus = await _userService.GetUserByID(ids);
                if (gus != null)  FinalUsers.Add(gus);
                usersidesforsug.Add(ids);
             }
            }
            // following
            if (us.following != null  && mainUser._id != null) {
             foreach (var ids in us.following)
             {
                if (usersidesforsug.Contains(ids) || ids == mainUser._id.ToString()) continue;
                var gus = await _userService.GetUserByID(ids);
                if (gus != null)  FinalUsers.Add(gus);
                usersidesforsug.Add(ids);
             }
            }

        }

        // return data
        return Ok( new {
            Users = FinalUsers,
            Success = true,
            Message = "Successfully"
        });


        }
        catch (Exception ex)
        {
           return BadRequest( new {Message = ex.Message, Success = false} ); 
        }
    }




// PATCH /Update/:id
[HttpPatch]
[Route("Update/{id}"), Authorize]
public async Task<IActionResult> UpdateUser([FromRoute] string id , [FromBody] UpdateUserInterface body){
    var userIDToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (userIDToken?.ToString() != id){
        return Unauthorized(new {message = "You are not authrized to delete this account"});
    }
    
    if (body.name == null || body.imageUrl == null || body.bio == null ){
            return BadRequest(new {message = "proplem with provided body data."});
    }
    
    var user = new User{};
    user = await _userService.GetUserByID(id);

    if (user is null){
        return NotFound(new {message = "User With given id is not found."});
    }
    // add new data to user
    user.name = body.name;
    user.imageUrl = body.imageUrl;
    user.bio = body.bio;

    // update user
    var upUser = await _userService.UpdateUser(id, user);
    if (upUser is null){
        return NotFound(new {message = "Can Not Update the user."});
    }

    return Ok(new {user = user});

}










// PATCH /:id/following
// Authmiddlewear
        [HttpPatch]
        [Route("{id}/following"), Authorize]
        public async Task<IActionResult> Following([FromRoute] string id)
        {
            if (id == null  ){
                return BadRequest(new {message = "proplem with provided id data."});
            }
            
            try
            {
                var user2 = await _userService.GetUserByID(id);
                
                if (user2 is null || user2._id is null) return NotFound( new {Message = "User Not Found", Success = false} );

                var userIDToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userIDToken == null  ){
                    return BadRequest(new {message = "proplem with provided id data. of token"});
                }

                var user1 = await _userService.GetUserByID(userIDToken.ToString());

                if (user1 is null || user1._id is null) return NotFound( new {Message = "User Not Found", Success = false} );

                if( user1.following == null) {
                   user1.following = new List<string>{};
                }
                if (user2.followers == null)
                {
                    user2.followers = new List<string>{};
                }
                var fo =user1.following;
                var fo2 = user2.followers;
                if (fo.Contains(id)) {
                    fo.Remove(id);
                    user1.following = fo;
                    fo2.Remove(user1._id);
                    user2.followers = fo2;
                } else {
                    fo.Add(id);
                    user1.following = fo;
                    fo2.Add(user1._id);
                    user2.followers = fo2;  
                    // Call Notification start

                    var deat = user1.name + "  Start Following You";
                    var us = new UserIn{name = user1.name, avatar = user1.imageUrl};
                    var notification = new Notification
                    {
                        mainuid = user2._id,
                        targetid = user1._id,
                        deatils = deat,
                        user = us
                    };

                     await _notificationService.CreateNotification(notification);
                    // Call Nofification End                 
                }

               if (user1 == null || user2 == null || user1._id == null || user2._id == null ){
                  return NotFound(new {message = "Error With given user"});
               }

                var updateduser1 = await  _userService.UpdateUser(user1._id.ToString(), user1); 
                var updateduser2 = await  _userService.UpdateUser(user2._id.ToString(), user2); 

                return Ok( new {
                    user1 = user1,
                    user2 = user2,
                    Success = true,
                    Message = "Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest( new {Message = ex.Message, Success = false} );
            }

        }
        

  





    [HttpDelete]
    [Route("delete/{id}"), Authorize]
    public async Task<IActionResult> Delete([FromRoute] string id) {
        var userIDToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Console.WriteLine(userIDToken);
        Console.WriteLine(id);
        if (userIDToken?.ToString() != id){
            return Unauthorized(new {message = "You are not authrized to delete this account"});
        }
        await _userService.DeleteAsync(id);
        return Ok();
    }

}






