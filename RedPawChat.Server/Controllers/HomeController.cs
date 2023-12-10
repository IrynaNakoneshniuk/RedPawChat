using DataAccessRedPaw.UserAccessData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedPaw.Models;

namespace RedPawChat.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "AccessChatResources")]
    public class HomeController : Controller
    {
        private IUserDataAccess _dataAccess;
        private IAdminDataAccess _adminDataAccess;

        public HomeController(IUserDataAccess dataAccess, IAdminDataAccess adminDataAccess)
        {
            _dataAccess = dataAccess;
            _adminDataAccess = adminDataAccess;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> Index()
        {
            return await _adminDataAccess.GetAllUser();
        }
    }
}
