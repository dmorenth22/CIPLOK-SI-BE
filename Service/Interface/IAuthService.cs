using CIPLOK_SI_BE.DTO;
using CIPLOK_SI_BE.Models;

namespace CIPLOK_SI_BE.Service.Interface
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> LoginAsync(LoginDTO data);
    }
}
