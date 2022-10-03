using LoginAndRegister.Dto;
using LoginAndRegister.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace LoginAndRegister.Controllers
{
    [ApiController]
    [Route("HealthCare")]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userPepo;
        public UsersController(IUserRepository userPepo)
        {
            _userPepo = userPepo;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _userPepo.Login(model);
            if (loginResponse.user == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                return BadRequest(new { message = "Username or Password is incorrect" });
            }
            return Ok(loginResponse);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            bool isUserNameUnique = _userPepo.IsUniqueUser(model.UserName);
            if (!isUserNameUnique)
                return BadRequest(new { message = "This username already exists" });
            var user = await _userPepo.Register(model);
            if (user == null)
                return BadRequest(new { message = "Bad request" });
            return Ok(user);
        }
    }
}
