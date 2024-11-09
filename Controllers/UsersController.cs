using Microsoft.AspNetCore.Mvc;
using RaceConditionSample.Interface;
using RaceConditionSample.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RaceConditionSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserListItem>>> Get()
        {
            return Ok(await _userService.GetUsersAsync());
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserListItem>> Get(int id)
        {
            var model = await _userService.GetUserByIdAsync(id);
            return model == null ? NotFound() : Ok(model);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserDto userDto)
        {
            return await _userService.CreateUserAsync(userDto) ? Ok() : BadRequest();
        }

        // POST api/<UsersController>
        [HttpPost("lock")]
        public async Task<ActionResult> PostLock([FromBody] UserDto userDto)
        {
            return await _userService.CreateUserAsyncKeyedLock(userDto) ? Ok() : BadRequest();
        }
    }
}
