using DataAccessRedPaw.UserAccessData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedPaw.Models;
using RedPawChat.Server.DataAccess.Models.DTO;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace RedPawChat.Server.Controllers
{
    [Route("api/profile")]
    [ApiController]
    [Authorize]

    public class ProfileController : Controller
    {
        private IUserDataAccess _dataAccess;
        private IAdminDataAccess _adminDataAccess;

        public ProfileController(IUserDataAccess dataAccess, IAdminDataAccess adminDataAccess)
        {
            _dataAccess = dataAccess;
            _adminDataAccess = adminDataAccess;
        }

        [HttpGet]
        [Route("getconversations/{id?}")]
        public async Task<IActionResult> GetConversations(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest();

                User? user = await _dataAccess.FindUserById(new Guid(id));
                if (user == null)
                    return NotFound();

                await _dataAccess.GetConversationInfo(user);

                var userDTO = await MapUserToUserDTOAsync(user);

                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private async Task<UserDTO> MapUserToUserDTOAsync(User user)
        {
            var userDTO = new UserDTO
            {
                Id= user.Id,    
                Email = user.Email,
                IsOnline = 1,
                Photo = user.ImageData,
                Name = user.UserName,
                Nick = user.NickName,
                Conversation = await MapConversationsAsync(user.Conversations),
            };

            return userDTO;
        }

        private async Task<List<ConversationDTO>> MapConversationsAsync(IEnumerable<Conversations> conversations)
        {
            var conversationDTOs = new List<ConversationDTO>();

            foreach (var conversation in conversations)
            {
                var conversationDTO = new ConversationDTO
                {
                    Id = conversation.Id,
                    Title = conversation.Title,
                    Messages = await MapMessagesAsync(conversation.Messages),
                    Members= await MapMembersGroup(conversation.Id)
                };

                conversationDTOs.Add(conversationDTO);
            }

            return conversationDTOs;
        }

        private async Task<List<MessagesDTO>> MapMessagesAsync(IEnumerable<Messages> messages)
        {
            var messagesDTOs = new List<MessagesDTO>();

            foreach (var message in messages)
            {
                var userInfo = await _dataAccess.FindUserById(message.UserId);

                var messagesDTO = new MessagesDTO
                {
                    Id = message.Id,
                    UserId = message.UserId,
                    Text = message.Text,
                    UserName = userInfo?.UserName,
                    Photo = userInfo?.ImageData,
                    ConversationId = message.ConversationId,
                    CreatedAt = message.CreatedAt
                };

                messagesDTOs.Add(messagesDTO);
            }

            return messagesDTOs;
        }

        private async Task<List<MembersConversationDTO>> MapMembersGroup(Guid idConversation)
        {
           List < MembersConversationDTO > members= new List<MembersConversationDTO>();

            var users = await _dataAccess.GetConversationMembers(idConversation);

            foreach (var member in users)
            {
                MembersConversationDTO user = new MembersConversationDTO()
                {
                    Id = member.Id,
                    UserName = member.UserName,
                    Photo = member.ImageData,
                    IsBlocked = member.IsBlocked,
                    ConversationId=idConversation
                };

                members.Add(user);
            }

            return members; 
        }

      
    }
}
