using LoginAndRegister.Dto;
using LoginAndRegister.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace LoginAndRegister.Controllers
{
    [ApiController]
    [Route("HealthCare")]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly ILogger<UsersController> log;  
        public UsersController(IUserRepository userRepo, ILogger<UsersController> log)
        {
            _userRepo = userRepo;
            this.log = log; 
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            log.LogInformation("Checking username and password");
            var loginResponse = await _userRepo.Login(model);
            if (loginResponse.user == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                return BadRequest(new { message = "Username or Password is incorrect" });
                log.LogInformation("Username and password are incorrect");
            }
            log.LogInformation("Logged In");
            return Ok(loginResponse);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            log.LogInformation("Received Registration request.");
            log.LogInformation("Checking whether username is unique or not.");
            bool isUserNameUnique = _userRepo.IsUniqueUser(model.UserName);
            if (!isUserNameUnique)
                return BadRequest(new { message = "This username already exists" });
            log.LogInformation("Trying to register the user");
            var user = await _userRepo.Register(model);
            if (user == null)
                return BadRequest(new { message = "Bad request" });
            log.LogInformation("User registered successfully");
            return Ok(user);
        }
    }
}
