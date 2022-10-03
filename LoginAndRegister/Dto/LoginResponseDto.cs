using LoginAndRegister.Modals;

namespace LoginAndRegister.Dto
{
    public class LoginResponseDto
    {
        public LocalUser user { get; set; }
        public string Token { get; set; }
    }
}
