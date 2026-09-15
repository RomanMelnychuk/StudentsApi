using StudentsApi.Models;

namespace StudentsApi.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
