using StudentsApi.Common;
using StudentsApi.DTOs;
using StudentsApi.Models;

namespace StudentsApi.Services
{
    public interface IAuthService
    {
        Task<ServiceResult<User>> RegisterAsync(RegisterDto dto);
        Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto dto);
    }
}
