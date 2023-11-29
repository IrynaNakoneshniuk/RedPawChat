using RedPaw.Models;

namespace DataAccessRedPaw.UserAccessData
{
    public interface IAdminDataAccess
    {
        Task BanUser(string id);
        Task<IEnumerable<User>> GetAllUser();
        Task UnblockUser(string id);
        Task UpdateRoleUser(string id);
    }
}