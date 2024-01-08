using RedPaw.Models;
using System.Security.Claims;

namespace DataAccessRedPaw.UserAccessData
{
    public interface IUserDataAccess
    {
        Task AddUserToBlackList(Guid userId, User user);
        Task AddUserToContacts(Guid userId, Guid contactId);
        Task ChangePassword(string password, string id);
        Task ChangePhotoProfile(string id, byte[] newPhoto);
        Task CreateMessage(Messages message);
        Task DeleteFromContacts(Guid userId, Guid contactId);
        Task DeleteMessages(Guid messageId);
        Task<IEnumerable<User>?> FindContactByNickName(string nickName);
        Task GetConversationInfo(User user);
        Task GetUserContact(User user);
        Task Registration(User user);
        Task UpdateAccount(User user);
        Task<User?> UserAuthentication(string email, string password);
        Task DeleteUserById(User user);
        Task<User?> FindUserById(Guid id);
        Task<User?> FindUserByEmail(string email);
        Task<IEnumerable<User?>> FindUserByName(string name);
        Task<int?> IsInRoleUser(User user, string roleName);
        Task<IEnumerable<string?>> GetUsersRole(User user);
        Task<IEnumerable<User?>> GetUsersInRoleAsync(string roleName);
        Task AddToRoleAsync(User user, string roleName);
        Task RemoveFromRole(User user, string roleName);
        Task<User?> SignInUser(string email, string password);
        Task<string?> GetSecurityStamp(User user);
        Task<IEnumerable<Claim>> GetClaimsAsync(Guid userId);
        Task AddClaimsAsync(User user, IEnumerable<Claim> claims);
        Task<IEnumerable<User>> GetContactsInfo(Guid id);
        Task UpdateUserClaim(User user, Claim claim, Claim newClaim);

        Task<Guid> CreateConversation(Guid userId, Guid contactId);

        Task RemoveConversation(Guid conversationId);

        Task<IEnumerable<User>> GetConversationMembers(Guid conversationId);
        Task AddMembers(Guid conversationId, Guid contactid);
        Task<Conversations> GetConversationById(Guid conversationId, Guid userId);
    }
}