using RedPaw.Models;

namespace DBAccess.DBAccess
{
    public interface ISqlDataAccess
    {
        Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters);
        Task SaveData<T>(string storedProcedure, T parametrs);
        Task GetContactInfo(string storedProcedure, User user);
        Task GetConversationsInfo(string storedProcedure, User user);
        Task<int?> GetScalarValue(string storedProcedure, User user, string roleName);
    }
}