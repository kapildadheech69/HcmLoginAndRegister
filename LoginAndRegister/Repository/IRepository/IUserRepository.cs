using LoginAndRegister.Dto;
using LoginAndRegister.Modals;

namespace LoginAndRegister.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<LocalUser> Register(RegistrationRequestDto registrationRequestDto);
    }
}
