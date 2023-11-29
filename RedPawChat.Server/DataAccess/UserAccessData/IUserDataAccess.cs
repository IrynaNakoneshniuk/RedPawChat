using RedPaw.Models;

namespace DataAccessRedPaw.UserAccessData
{
    public interface IUserDataAccess
    {
        Task AddUserToBlackList(Guid userId, User user);
        Task AddUserToContacts(Guid userId, User user);
        Task ChangePassword(string password, string id);
        Task ChangePhotoProfile(string id, byte[] newPhoto);
        Task CreateMessage(Messages message);
        Task DeleteFromContacts(Guid userId, User user);
        Task DeleteMessages(Guid messageId);
        Task<IEnumerable<User>?> FindContactByNickName(string nickName);
        Task GetConversationInfo(User user);
        Task GetUserContact(User user);
        Task Registration(User user);
        Task UpdateAccount(User user);
        Task<User?> UserAuthentication(string email, string password);
    }
}