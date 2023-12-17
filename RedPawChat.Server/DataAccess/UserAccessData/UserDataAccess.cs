using DBAccess.DBAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using RedPaw.Models;
using RedPawChat.Server.DataAccess.Models.DTO;
using System;
using System.Security.Claims;

namespace DataAccessRedPaw.UserAccessData
{
    // This class provides data access methods related to user registration.
    public class UserDataAccess : IUserDataAccess
    {
        private readonly ISqlDataAccess _dataAccess;

        public UserDataAccess(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }


        public async Task<User?> FindUserById(Guid id)
        {
            var userResult = await _dataAccess.LoadData<User,dynamic>("spFindByIdAsync",new {Id= id });

            return userResult.FirstOrDefault();
        }

        public async Task DeleteUserById(User user)
        {
            await _dataAccess.SaveData<dynamic>("spDeleteUser",new {Id=user.Id});
        }

        // Registers a new user in the system.
        public async Task Registration(User user)
        {
            // Use data access layer to save user registration data using the spRegistration_User stored procedure.
            // The dynamic parameter contains user details mapped from the User object.
            await _dataAccess.SaveData<dynamic>("spRegistration_User", new
            {
                NickName = user.NickName,
                Email = user.Email,
                Password = user.Password,
                UserName = user.UserName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                CreatedAt = user.CreatedAt,
                ImageData = user.ImageData,
                NormalizedUserName = user.UserName?.ToUpper(),
                NormalizedEmail = user.Email.ToUpper(),
                PasswordHash = user.PasswordHash,
                TwoFactorEnabled = user.TwoFactorEnabled,
                SecurityStamp=user.SecurityStamp,
                ConcurrencyStamp = user.ConcurrencyStamp,
                EmailConfirmed=user.EmailConfirmed,
                AccessFailedCount=user.AccessFailedCount,
                LockoutEnabled=user.LockoutEnabled,

            });
        }

        // Authenticates a user based on the provided email and password.
        // Returns the authenticated user, if any.
        public async Task<User?> UserAuthentication(string email, string password)
        {
            var user = await _dataAccess.LoadData<User, dynamic>(
                "spAuthentication_User",
                new { Email = email, Password = password }
            );

            // Return the first user found (or null if none).
            return user.FirstOrDefault();
        }

        public async Task<User?> SignInUser(string email, string password)
        {
          var user=  await UserAuthentication(email, password);

            if (user != null)
            {
                await GetConversationInfo(user);
            }

            return user;
        }


        public async Task<User?> FindUserByEmail(string email)
        {
            var user = await _dataAccess.LoadData<User, dynamic>(
                "spFindUserByEmail",
                new { Email = email }
            );

            // Return the first user found (or null if none).
            return user.FirstOrDefault();
        }

        // Updates account information for a user.
        public async Task UpdateAccount(User user)
        {
            // Use data access layer to save updated account information using the spApdate_account stored procedure.
            // The dynamic parameter contains user details mapped from the User object.
            await _dataAccess.SaveData<dynamic>("spUpdate_account", new
            {
                Id = user.Id,
                NickName = user.NickName,
                UserName = user.UserName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Password=user.Password,
                PasswordHash= user.PasswordHash,
                SecurityStamp= user.SecurityStamp,
                ConcurrencyStamp=user.ConcurrencyStamp,
            });
        }

        // Changes the password for a user.
        public async Task ChangePassword(string password, string id)
        {
            // Use data access layer to save the updated password using the spChangePassword_User stored procedure.
            // The dynamic parameter contains user ID and the new password.
            await _dataAccess.SaveData<dynamic>("spChangePassword_User", new
            {
                Id = id,
                Password = password,
            });
        }

        // Changes the profile photo for a user.
        public async Task ChangePhotoProfile(string id, byte[] newPhoto)
        {
            // Use data access layer to save the updated profile photo using the spChangePassword_User stored procedure.
            // The dynamic parameter contains user ID and the new profile photo data.
            await _dataAccess.SaveData<dynamic>("spChangePhoto_User", new
            {
                Id = id,
                ImageData = newPhoto
            });
        }

        // Retrieves contact information for a user.
        // This method uses the data access layer to fetch contact information for a given user using the spGetAllData stored procedure.
        public async Task GetUserContact(User user)
        {
            await _dataAccess.GetContactInfo("spGetAllData", user);
        }

        // Retrieves conversation information for a user.
        // This method utilizes the data access layer to fetch conversation information for a given user using the spGetConversations stored procedure.
        public async Task GetConversationInfo(User user)
        {
            await _dataAccess.GetConversationsInfo("spGetConversations", user);
        }

        // Creates a new message in the system.
        // This method constructs parameters from the provided Messages object and uses the data access layer to save the message
        // using the spCreateMessage_User stored procedure.
        public async Task CreateMessage(Messages message)
        {
            var parameters = new
            {
                ConversationId = message.ConversationId,
                UserId = message.UserId,
                Text = message.Text,
                CreatedAt = DateTime.UtcNow,
            };

            await _dataAccess.SaveData<dynamic>("spCreateMessage_User", parameters);
        }

        // Deletes messages based on the provided messageId.
        // This method uses the data access layer to delete messages by messageId using the spDeleteMessage stored procedure.
        public async Task DeleteMessages(Guid messageId)
        {
            await _dataAccess.SaveData<dynamic>("spDeleteMessage", new { IdMessage = messageId });
        }

        // Adds a user to contacts.
        // This method utilizes the data access layer to add a user to contacts using the spAddUserToContacts stored procedure.
        public async Task AddUserToContacts(Guid userId, User user)
        {
            await _dataAccess.SaveData<dynamic>("spAddUserToContacts", new { UserId = userId, ContactId = user.Id });
        }

        // Adds a user to the blacklist.
        // This method uses the data access layer to add a user to the blacklist using the spAddUserToBlackList stored procedure.
        public async Task AddUserToBlackList(Guid userId, User user)
        {
            await _dataAccess.SaveData<dynamic>("spAddUserToBlackList", new { UserId = userId, ParticipantId = user.Id, CreatedAt = DateTime.UtcNow });
        }

        // Deletes a user from contacts.
        // This method leverages the data access layer to delete a user from contacts using the spDeleteFromContacts stored procedure.
        public async Task DeleteFromContacts(Guid userId, User user)
        {
            await _dataAccess.SaveData<dynamic>("spDeleteFromContacts", new { ContactId = user.Id, UserId = userId });
        }

        // Finds contacts by nickname.
        // This method uses the data access layer to find contacts by nickname using the spFindContact stored procedure.
        public async Task<IEnumerable<User>?> FindContactByNickName(string nickName)
        {
            return await _dataAccess.LoadData<User, dynamic>("spFindContact", new { NickName = nickName });
        }

        public async Task<IEnumerable<User?>> FindUserByName(string name)
        {
            return await _dataAccess.LoadData<User, dynamic>("spFindUserByName", new { NormalizeName = name.ToUpper() });
            
        }

        public async Task<int?> IsInRoleUser(User user, string roleName)
        {
            return await _dataAccess.GetScalarValue("pIsInRoleAsync", user, roleName);
        }

        public async Task<IEnumerable<string?>> GetUsersRole(User user)
        {
            return await _dataAccess.LoadData<string, dynamic>("spGetRolesAsync", new { Id = user.Id });
        }

        public async Task<IEnumerable<User?>> GetUsersInRoleAsync(string roleName)
        {
            return await _dataAccess.LoadData<User,dynamic>("spGetUsersInRoleAsync", new { RoleName = roleName.ToUpper()}); 
        }

        public async Task AddToRoleAsync(User user, string roleName)
        {
            await _dataAccess.SaveData("spAddToRole", new { Id =user.Id, RoleNameNormalize = roleName.ToUpper()});
        }

        public async Task RemoveFromRole(User user, string roleName)
        {
            await _dataAccess.SaveData("spRemoveFromRole", new { Id = user.Id, RoleNameNormalize = roleName.ToUpper() });
        }

        public async Task<string?> GetSecurityStamp(User user)
        {
           var res= await _dataAccess.LoadData<string?, dynamic>("spGetSecurityStamp", new { IdUser = user.Id });
            return res.FirstOrDefault();
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync(Guid userId)
        {
            var listClaims= await _dataAccess.LoadData<UserClaimDto, dynamic>("spGetUserClaims", new { UserId = userId });

            return listClaims.Select(r => new Claim(r.ClaimType, r.ClaimValue)).ToList();
        }

        public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims)
        {
             await _dataAccess.SaveClaimsListAtDb("spAddUserClaim",user.Id,claims);   
        }

        public async Task UpdateUserClaim(User user, Claim claim, Claim newClaim)
        {
            await _dataAccess.SaveData<dynamic>("spUpdateUserClaim", new
            {
                UserId = user.Id,
                NewClaimType = newClaim.Type,
                NewClaimValue = newClaim.Value,
                ClaimType = claim.Value,
                ClaimValue = claim.Value
            });
        }
    }
}
