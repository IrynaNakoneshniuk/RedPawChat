using DataAccessRedPaw.UserAccessData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedPawChat.Server.DataAccess.Models.DTO;

namespace RedPawChat.Server.Controllers
{
    [Route("api/contacts")]
    [ApiController]

    public class ContactsController : Controller
    {
        private IUserDataAccess _userDataAccess;

        public ContactsController(IUserDataAccess userDataAccess)
        {
            _userDataAccess = userDataAccess;
        }

        [HttpGet]
        [Route("getcontacts/{id?}")]

       public async Task<IActionResult> GetContacts(Guid id)
        {
            if (id == null)
                return NotFound();
            
            var contactsList= await _userDataAccess.GetContactsInfo(id);

            List<ContactsDTO> contacts = new List<ContactsDTO>();   

            foreach(var contact in contactsList)
            {
                ContactsDTO contactDto = new ContactsDTO()
                {
                    Id = contact.Id,
                    Name=contact.UserName,
                    Photo=contact.ImageData,
                    Email=contact.Email,
                    IsBlock=contact.IsBlocked,
                    IsOnline=contact.IsActive,
                    Nick=contact.NickName
                };
                   
                contacts.Add(contactDto);
            }

            return Ok(contacts);    

        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> Search([FromBody]string query)
        {
            if(query == null) 
                return NotFound();

            var users= await _userDataAccess.FindContactByNickName(query);

            if(users == null)
            {
                return Ok(new { Message = "Not found" });
            }

            List<ContactsDTO> contactsDTOs = new List<ContactsDTO>();

            foreach(var user in users)
            {
                ContactsDTO contactsDTO = new ContactsDTO()
                {
                    Id = user.Id,
                    Name = user.UserName,
                    Nick=user.NickName,
                    Photo=user.ImageData,
                    IsOnline=user.IsActive,
                    Email=user.Email,
                };

                contactsDTOs.Add(contactsDTO);
            }
            
            return Ok(contactsDTOs);  
        }

        [HttpPost]
        [Route("addcontact")]
        public async Task<IActionResult> AddContact([FromBody] string parameters)
        {
            if (parameters == null)
                return NotFound();

            var result= parameters.Split(',');

            if(result.Length == 2)
            {
                Guid idUser = Guid.Parse(result[0]);
                Guid idContact = Guid.Parse(result[1]);

                await _userDataAccess.AddUserToContacts(idUser, idContact);

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("deletecontact")]
        public async Task<IActionResult> DeleteContact([FromBody] string parameters)
        {
            if (parameters == null)
                return NotFound(new {Error="Empty parameters"});

            var result = parameters.Split(',');

            if (result.Length == 2)
            {
                Guid idUser = Guid.Parse(result[0]);
                Guid idContact = Guid.Parse(result[1]);

                await _userDataAccess.DeleteFromContacts(idUser, idContact);

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
