using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebApi.Controllers
{

    [ApiController]
    [Route("[controller]s")]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository ;
        }

        /*
        [HttpGet]
        public List<UserView> GetUsers()
        {
            var result =  _userRepository.GetAllUsers();
            return result;
        }
        */

        [HttpGet("{id}")]
        public UserGetView GetUserById(int id)
        {
            var result =  _userRepository.GetUserById(id);
            
            var getview = new UserGetView{
                UserId = result.UserId,
                Name = result.Name,
                SurName = result.SurName,
                Birthday = result.Birthday,
                Age = DateTime.Today.Year - result.Birthday.Year,
                address = new Address{
                    State = result.address.State,
                    PostalCode = result.address.PostalCode
                    }
                };
            return getview;
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] UserPostView newuser)
        {

            var user = new User{
                Id = ObjectId.GenerateNewId(),
                UserId = newuser.UserId,
                Name = newuser.Name,
                SurName = newuser.SurName,
                Birthday = newuser.Birthday,
                Age = DateTime.Today.Year - newuser.Birthday.Year,
                address = new Address{
                    State = newuser.address.State,
                    PostalCode = newuser.address.PostalCode
                    }
                };

            if(_userRepository.AddUser(user))
                return Ok();
                
            return BadRequest();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUserById(int id , [FromBody] UserPutView user)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUserById(int id)
        {
            if(_userRepository.DeleteUserById(id))
                return Ok();
            else
                return BadRequest();
        }



    }
}
