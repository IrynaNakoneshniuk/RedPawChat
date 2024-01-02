using DataAccessRedPaw.UserAccessData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedPaw.Models;
using RedPawChat.Server.DataAccess.Models.DTO;

namespace RedPawChat.Server.Controllers
{
    [Route("api/profile")]
    [ApiController]
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
           User ? user= await _dataAccess.FindUserById(new Guid(id));
           await _dataAccess.GetConversationInfo(user);

            var conversations = user.Conversations;
            UserDTO userDTO = new UserDTO();
            userDTO.Email = user.Email;
            userDTO.IsOnline = 1;
            userDTO.Photo = user.ImageData;
            userDTO.Name = user.UserName;
            userDTO.Nick = user.NickName;

            foreach (Conversations conversation in conversations)
            {
                ConversationDTO conversationDTO = new ConversationDTO();
                foreach (Messages message in conversation.Messages)
                {
                    MessagesDTO messagesDTO = new MessagesDTO();
                    messagesDTO.UserId = message.UserId;
                    messagesDTO.Text = message.Text;
                    var userInfo = await _dataAccess.FindUserById(message.UserId);
                    messagesDTO.UserName = userInfo.UserName;
                    messagesDTO.Photo = userInfo.ImageData;
                    messagesDTO.ConversationId = conversation.Id;
                    messagesDTO.CreatedAt = message.CreatedAt;
                    messagesDTO.Id=message.Id;

                    conversationDTO.Messages.Add(messagesDTO);
                   
                }
                conversationDTO.Title = conversation.Title;
                conversationDTO.Id = conversation.Id;

               userDTO.Conversation.Add(conversationDTO);
            }

            return Ok(userDTO);
        }
    }
}
