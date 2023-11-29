using DataAccessRedPaw.UserAccessData;
using Microsoft.AspNetCore.Mvc;
using RedPaw.Models;

namespace RedPawChat.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private IUserDataAccess _dataAccess;
        private IAdminDataAccess _adminDataAccess;

        HomeController(IUserDataAccess dataAccess, IAdminDataAccess adminDataAccess)
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
