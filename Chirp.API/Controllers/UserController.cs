using System.Security.Claims;
using Chirp.Core.Domain.Interfaces.Services;
using Chirp.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
        {
            var user = await _userService.CreateUserAsync(createUserDto);
            return CreatedAtAction(nameof(GetUserById), new { Id = user.Id }, user);
        }

        // PUT: api/users/{username}
        [Authorize]
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserDto updateUserDto)
        {
            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(loggedInUserId, out var id) || userId != id)
            {
                return Forbid("You can only perform this action on your own account.");
            }

            await _userService.UpdateUserAsync(userId, updateUserDto);
            return NoContent();
        }

        // DELETE: api/users/{username}
        [Authorize]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(loggedInUserId, out var id) || userId != id)
            {
                return Forbid("You can only perform this action on your own account.");
            }

            await _userService.DeleteUserAsync(userId);
            return NoContent();
        }
    }
}