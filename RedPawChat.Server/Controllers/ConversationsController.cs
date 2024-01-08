using DataAccessRedPaw.UserAccessData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedPaw.Models;
using RedPawChat.Server.DataAccess.Models.DTO;

namespace RedPawChat.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConversationsController : ControllerBase
    {
        private readonly IUserDataAccess _userDataAccess;

        public ConversationsController(IUserDataAccess userDataAccess)
        {
            _userDataAccess = userDataAccess;
        }

        [HttpPost]
        [Route("createnewconversation")]
       public async Task<IActionResult> CreateNewConversation([FromBody] string parameters)
        {
            if (parameters == null)
                return NotFound();

            var result = parameters.Split(',');

            if (result.Length == 2)
            {
                Guid idUser = Guid.Parse(result[0]);
                Guid idContact = Guid.Parse(result[1]);

                Guid idNewConversation = await _userDataAccess.CreateConversation(idUser, idContact);

                return Ok(idNewConversation);
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpDelete]
        [Route("removeconversation")]
        public async Task<IActionResult> RemoveConversation([FromBody]string conversationId)
        {
            if(new Guid(conversationId) == Guid.Empty) return BadRequest();

            await _userDataAccess.RemoveConversation(new Guid(conversationId));

            return Ok();

        }

        [HttpPost]
        [Route("addmembers")]

        public async Task<IActionResult> AddMembersConversation([FromBody] string parameters)
        {
            if (parameters == null)
                return NotFound();

            var result = parameters.Split(',');

            if (result.Length == 2)
            {
                Guid idConversation = Guid.Parse(result[0]);
                Guid idContact = Guid.Parse(result[1]);

                await _userDataAccess.AddMembers(idConversation, idContact);

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("updateconversation/{id?}/{conversationid?}")]
        public async Task<IActionResult> UpdateConversation(string id,string conversationId)
        {
                Guid idConversation = Guid.Parse(conversationId);
                Guid idContact = Guid.Parse(id);

               if(idConversation == Guid.Empty||idContact==Guid.Empty) return BadRequest();    
  
                var conversation= await _userDataAccess.GetConversationById(idConversation, idContact);
                if(conversation==null) return NotFound();

                var conversationDTO = await MapConversationsAsync(conversation);
                if (conversationDTO == null) return NotFound();

                return Ok(conversationDTO);
        }

        private async Task<List<MessagesDTO>> MapMessagesAsync(IEnumerable<Messages> messages)
        {
            var messagesDTOs = new List<MessagesDTO>();

            foreach (var message in messages)
            {
                var userInfo = await _userDataAccess.FindUserById(message.UserId);

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
            List<MembersConversationDTO> members = new List<MembersConversationDTO>();

            var users = await _userDataAccess.GetConversationMembers(idConversation);

            foreach (var member in users)
            {
                MembersConversationDTO user = new MembersConversationDTO()
                {
                    Id = member.Id,
                    UserName = member.UserName,
                    Photo = member.ImageData,
                    IsBlocked = member.IsBlocked,
                    ConversationId = idConversation
                };

                members.Add(user);
            }

            return members;
        }

        private async Task<ConversationDTO> MapConversationsAsync(Conversations conversation)
        {

           var conversationDTO = new ConversationDTO
           {
               Id = conversation.Id,
               Title = conversation.Title,
               Messages = await MapMessagesAsync(conversation.Messages),
               Members = await MapMembersGroup(conversation.Id)
           };

            return conversationDTO;
        }

        [HttpPost]
        [Route("addmessage")]

        public async Task<IActionResult> AddMessage([FromBody] MessagesDTO message)
        {
            var m = message;
            if (message != null)
            {
                await _userDataAccess.CreateMessage(new Messages()
                {
                    ConversationId = message.ConversationId,
                    Text = message.Text,
                    CreatedAt = DateTime.UtcNow,
                    UserId = message.UserId,
                    DeletedAt = null,
                    Id= message.Id
                });

                return Ok();
            }
            else
            {
                return BadRequest();
            }
            
        }
    }
}
