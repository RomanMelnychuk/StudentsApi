using StudentsApi.DTOs;
using StudentsApi.Models;

namespace StudentsApi.Repositories
{
    public interface IUserRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
    }
}
