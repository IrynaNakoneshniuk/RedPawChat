using DataAccessRedPaw.UserAccessData;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RedPaw.Models;
using RedPawChat.Server.DataAccess.Models;
using RedPawChat.Server.Filters;
using System.Net.WebSockets;
using System.Security.Claims;


namespace RedPawChat.Server.Controllers
{
    [Route("api/account")]
    [ApiController]
   
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserDataAccess _userDataAccess;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IUserDataAccess userDataAccess)
        {
            _signInManager = signInManager;
            _userManager= userManager;
            _userDataAccess= userDataAccess;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var cookies=Request.Cookies;
            
            return Ok();
        }
       
        [HttpPost]
        [Route("Register")]
        //POST : /api/User/Register
        public async Task<Object> Register(string email, string userName, string lastName,string middleName,string password,
            string nickName)
        {
            var applicationUser = new User()
            {
                Email = email,
                UserName = userName,
                LastName = lastName,
                MiddleName = middleName,
                Password = password,
                CreatedAt = DateTime.UtcNow,
                NickName = nickName,
                ImageData = null,

            };

            var result = await _userManager.CreateAsync(applicationUser,applicationUser.Password);

            if (result.Succeeded)
            {
               

                var claims = new List<Claim>()
                {
                    new Claim("Email", email),
                    new Claim("Role","User")
                };

                var user= await _userDataAccess.FindUserByEmail(email);
                await _userDataAccess.AddClaimsAsync(user, claims);
                var identity = new ClaimsIdentity(claims, "RedPawAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                var resultSignIn=await _signInManager.PasswordSignInAsync(email,password, true, lockoutOnFailure: false);
                await HttpContext.SignInAsync("RedPawAuth", claimsPrincipal, new AuthenticationProperties { IsPersistent = true });
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Login")]
      
        public async Task<IActionResult> Login([FromBody] LoginInfo loginModel)
        {
            var resultSignIn = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, true, lockoutOnFailure: false);

            if (resultSignIn.Succeeded)
            {
                var user = await _userDataAccess.FindUserByEmail(loginModel.Email);
                var claims = await _userDataAccess.GetClaimsAsync(user.Id);
                var identity = new ClaimsIdentity(claims,"RedPawAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("RedPawAuth", claimsPrincipal, new AuthenticationProperties { IsPersistent = false });

                return Ok();
            }
            else
                return BadRequest(new { message = "Username or password is incorrect." });
        }

        [HttpPost("СhangePassword")]
        [SignInResultFilter]
        public async Task<IActionResult> ChangePassword(string email, string currentPassword,string newPassword)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(email);

                if (user == null)
                {
                    return BadRequest("Invalid username.");
                }

                var result = await _userManager.CheckPasswordAsync(user, currentPassword);


                if (!result)
                {
                    return BadRequest("Invalid current password.");
                }

                user.Password = newPassword;
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

                if (changePasswordResult.Succeeded)
                {
                    return Ok("Password changed successfully.");
                }
                else
                {
                    return BadRequest("Failed to change password.");
                }
            }

            return BadRequest(ModelState);
        }
    }

}
