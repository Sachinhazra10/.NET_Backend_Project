using Microsoft.AspNetCore.Mvc;
using UserManagementApi.Models;
using UserManagementApi.Services;

namespace UserManagementApi.Controllers
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

        // GET: api/users
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            if (user == null)
            {
                return NotFound(new { error = $"User with ID {id} not found." });
            }
            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public IActionResult Create([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdUser = _userService.Create(user);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
        }

        // PUT: api/users/{id}
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = _userService.Update(id, user);
            if (!updated)
            {
                return NotFound(new { error = $"User with ID {id} not found." });
            }

            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var deleted = _userService.Delete(id);
            if (!deleted)
            {
                return NotFound(new { error = $"User with ID {id} not found." });
            }

            return NoContent();
        }
    }
}
