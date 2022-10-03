using LoginAndRegister.Dto;
using LoginAndRegister.Modals;
using LoginAndRegister.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoginAndRegister.Repository
{
    public class UserRepository:IUserRepository
    {
        private ToDoContext _toDoContext;
        private string secretkey;
        public UserRepository(ToDoContext toDoContext, IConfiguration configuration)
        {
            _toDoContext = toDoContext;
            secretkey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string username)
        {
            var user = _toDoContext.LocalUsers.SingleOrDefault(u => u.UserName == username);
            if (user == null)
                return true;
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _toDoContext.LocalUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName && u.Password == loginRequestDto.Password);
            if (user == null)
            {
                return new LoginResponseDto()
                {
                    Token = "",
                    user = null
                };
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretkey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                Token = tokenHandler.WriteToken(token),
                user = user,
            };
            loginResponseDto.user.Password = "";
            return loginResponseDto;
        }

        public async Task<LocalUser> Register(RegistrationRequestDto registrationRequestDto)
        {
            LocalUser user = new LocalUser()
            {
                UserName = registrationRequestDto.UserName,
                Password = registrationRequestDto.Password,
                Name = registrationRequestDto.Name,
                Role = registrationRequestDto.Role
            };
            await _toDoContext.LocalUsers.AddAsync(user);
            await _toDoContext.SaveChangesAsync();
            user.Password = "";
            return user;
        }
    }
}
