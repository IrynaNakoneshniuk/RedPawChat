using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using DataAccessRedPaw.UserAccessData;
using DBAccess.AppSetting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RedPaw.Models;

namespace RedPawChat.Server.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IUserDataAccess _dataAccess;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        
        public AccountController(IUserDataAccess dataAccess, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _dataAccess = dataAccess;
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
        [Authorize(Policy = "AccessChatResources")]
        //POST : /api/User/Register
        public async Task<Object> Register(User model)
        {
            if(model!=null)
            {
                var applicationUser = new User()
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    Password = model.Password,
                    CreatedAt = DateTime.UtcNow,
                    NickName = model.NickName,
                    ImageData = model.ImageData,
                };

                await _dataAccess.Registration(applicationUser);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(string email,string password)
        {
            //var user = await _dataAccess.UserAuthentication(email, password);
         
            var user = await _signInManager.PasswordSignInAsync(email, password, true, lockoutOnFailure: false);
            if (user.Succeeded)
            {
                try
                {
                    var claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name,email),
                    new Claim(ClaimTypes.Upn,password),
                    new Claim(ClaimTypes.Role,"Admin")
                    };

                    var identity= new ClaimsIdentity(claims, "RedPawAuth"); 
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                    var authProperty = new AuthenticationProperties
                    {
                        IsPersistent = true,
                    };
                    await HttpContext.SignInAsync("RedPawAuth", claimsPrincipal, authProperty);

                    //var roles = await _userManager.GetRolesAsync(user);

                    return Ok(new { user });

                }
                catch (Exception exp)
                {
                    throw;
                }
            }
            else
                return BadRequest(new { message = "Username or password is incorrect." });
        }
    }
}