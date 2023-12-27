using DataAccessRedPaw.UserAccessData;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RedPaw.Models;
using RedPawChat.Server.DataAccess.Models.DTO;
using RedPawChat.Server.Filters;
using System.Security.Claims;
using static Dapper.SqlMapper;


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
        [Route("getall")]
        [Authorize]      
        public   IActionResult GetAll()
        {
            var cookies=Request.Cookies;
            
            return Ok(new {Message="Вдало"});
        }
       
        [HttpPost]
        [Route("registration")]
        //POST : /api/User/Register
        public async Task<IActionResult> Register([FromForm] RegistrationModel registrationModel)
        {
           
            var applicationUser = new User()
            {
                Email = registrationModel.Email,
                UserName = registrationModel.FirstName,
                LastName = registrationModel.LastName,
                MiddleName = registrationModel.MiddleName,
                Password = registrationModel.Password,
                CreatedAt = DateTime.UtcNow,
                NickName = registrationModel.Nickname,
                ImageData = ConvertIFormFileToByteArray(registrationModel.Photo),

            };

            if (applicationUser != null)
            {
                var result = await _userManager.CreateAsync(applicationUser, applicationUser.Password);

                if (result.Succeeded)
                {


                    //var claims = new List<Claim>()
                    //{
                    //    new Claim("Email",registrationModel.Email),
                    //    new Claim("Role","User")
                    //};

                    //var user = await _userDataAccess.FindUserByEmail(registrationModel.Email);
                    //await _userDataAccess.AddClaimsAsync(user, claims);
                    //var identity = new ClaimsIdentity(claims, "RedPawAuth");
                    //ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                    var resultSignIn = await _signInManager.PasswordSignInAsync(registrationModel.Email, registrationModel.Password, true, lockoutOnFailure: false);
                    //await HttpContext.SignInAsync("RedPawAuth", claimsPrincipal, new AuthenticationProperties { IsPersistent = true });
                    return Ok();
                }
                else
                {
                    return BadRequest(new {Error=result.Errors});
                }
            }
            else
            {
                return BadRequest(new { Error = "All fields must be filled" });
            }
        }

        [HttpPost]
        [Route("login")]
       
        public async Task<IActionResult> Login([FromBody] LoginInfo loginModel)
        {
            var resultSignIn = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, true, lockoutOnFailure: false);


            if (resultSignIn.Succeeded)
            {
                //var user = await _userDataAccess.FindUserByEmail(loginModel.Email);
                //var claims =  await _userManager.GetClaimsAsync(user);
                //var identity = new ClaimsIdentity(claims, "RedPawAuth");
                //ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
                //await HttpContext.SignInAsync("RedPawAuth", claimsPrincipal, new AuthenticationProperties { IsPersistent = true });

                return Ok();
            }
            else
                return BadRequest(new { message = "Username or password is cincorrect." });
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

        private byte[] ConvertIFormFileToByteArray(IFormFile file)
        {
            if (file != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }

            throw new NullReferenceException();
        }
    }

}
