using DataAccessRedPaw.UserAccessData;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using RedPaw.Models;
using RedPawChat.Server.Filters;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace RedPawChat.Server.Controllers
{
    [Route("api/account")]
    [ApiController]
   
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _userManager= userManager;  
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok();
        }
       
        [HttpPost]
        [Route("Register")]
        [SignInResultFilter]
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

            //await _userManager.UpdateSecurityStampAsync(applicationUser);   

            if (result.Succeeded)
            {
                //await _signInManager.PasswordSignInAsync(email, password, true, lockoutOnFailure: false);
                await _signInManager.SignInAsync(applicationUser, isPersistent: true);
            }
            return Ok();
        }

        [HttpPost]
        [Route("Login")]
        [SignInResultFilter]
        public async Task<IActionResult> Login(string email,string password)
        { 
            var resultSignIn = await _signInManager.PasswordSignInAsync(email, password, true, lockoutOnFailure: false);

            if (resultSignIn.Succeeded)
            {
                return Ok();
            }
            else
                return BadRequest(new { message = "Username or password is incorrect." });
        }

        [HttpPost("changepassword")]
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
               

                //if (!result)
                //{
                //    return BadRequest("Invalid current password.");
                //}


                var changePasswordResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

                if (changePasswordResult.Succeeded)
                {
                    //await _userManager.UpdateSecurityStampAsync(user);
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
