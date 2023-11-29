using DBAccess.DBAccess;
using RedPaw.Models;

namespace DataAccessRedPaw.UserAccessData
{
    // This class provides data access methods related to admin operations on users.
    public class AdminDataAccess : IAdminDataAccess
    {
        private readonly ISqlDataAccess _dataAccess;
        public AdminDataAccess(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        // Retrieves all users using a stored procedure.
        public async Task<IEnumerable<User>> GetAllUser()
        {
            // Use data access layer to load data from the database.
            // The dynamic parameter is an empty object as the stored procedure spGetAllUsersAdmin doesn't require parameters.
            return await _dataAccess.LoadData<User, dynamic>("spGetAllUsersAdmin", new { });
        }

        // Bans a user based on the provided user ID.
        public async Task BanUser(string id)
        {
            // Use data access layer to save data using the spBan_User stored procedure.
            // The dynamic parameter contains the user ID.
            await _dataAccess.SaveData<dynamic>("spBan_User", new { Id = id });
        }

        // Unblocks a user based on the provided user ID.
        public async Task UnblockUser(string id)
        {
            // Use data access layer to save data using the spUnblock_User stored procedure.
            // The dynamic parameter contains the user ID.
            await _dataAccess.SaveData<dynamic>("spUnblock_User", new { Id = id });
        }

        // Updates the role of a user based on the provided user ID.
        public async Task UpdateRoleUser(string id)
        {
            // Use data access layer to save data using the spUpdateRole_User stored procedure.
            // The dynamic parameter contains the user ID.
            await _dataAccess.SaveData<dynamic>("spUpdateRole_User", new { Id = id });
        }
    }
}
