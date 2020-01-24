using System.Threading.Tasks;
using BGBLL.Interfaces.Persistence;
using BGDomain.DTOs;
using BGDomain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BGAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserManager : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var ret = await _userRepository.GetAll();
            return Ok(ret);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<User> GetUser(int id)
        {
            return await _userRepository.GetById(id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            await _userRepository.Add(new User()
            {
                Name = user.Name,
                BirthDate = user.BirthDate,
                Email = user.Email,
                Hobbies = user.Hobbies,
            });
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            var userToUpdate = await _userRepository.GetById(id);
            if (userToUpdate == null)
            {
                return BadRequest();
            }

            await _userRepository.Update(new User()
            {
                Id = id,
                Name = user.Name,
                BirthDate = user.BirthDate,
                Email = user.Email,
                Hobbies = user.Hobbies,
            });
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userToRemove = await _userRepository.GetById(id);
            if (userToRemove == null)
            {
                return BadRequest();
            }

            await _userRepository.Remove(userToRemove);
            return Ok();
        }
    }
}