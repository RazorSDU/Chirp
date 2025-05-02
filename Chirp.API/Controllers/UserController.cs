using Chirp.Core.Domain.Interfaces.Services;
using Chirp.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
        {
            var user = await _userService.CreateUserAsync(createUserDto);
            return CreatedAtAction(nameof(GetUser), new { Id = user.Id }, user);
        }
    }
}