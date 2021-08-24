using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
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
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
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
            GetByIdValidator gv = new GetByIdValidator();
            gv.UserId = id;

            UserGetByIdValidator validator = new UserGetByIdValidator();
            validator.ValidateAndThrow(gv);

            var result =  _userRepository.GetUserById(id);
            
            var getview = _mapper.Map<UserGetView>(result);

            return getview;
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] UserPostView newuser)
        {

            // validation
            UserPostValidator validator = new UserPostValidator();
            validator.ValidateAndThrow(newuser);

                // mapping
            var user = _mapper.Map<User>(newuser);
            user.Age = DateTime.Today.Year - newuser.Birthday.Year;


            _userRepository.AddUser(user);
            return Ok();
            
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUserById(int id , [FromBody] UserPutView user)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUserById(int id)
        {
            DeleteByIdValidator gv = new DeleteByIdValidator();
            gv.UserId = id;

            UserDeleteByIdValidator validator = new UserDeleteByIdValidator();
            validator.ValidateAndThrow(gv);

            if(_userRepository.DeleteUserById(id))
                return Ok();
            else
                return BadRequest();
        }



    }
}
