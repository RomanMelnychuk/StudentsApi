using Microsoft.IdentityModel.Tokens;
using StudentsApi.Common;
using StudentsApi.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using StudentsApi.Repositories;
using StudentsApi.Models;


namespace StudentsApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<ServiceResult<User>> RegisterAsync(RegisterDto dto)
        {
            var emailExists = await _userRepository.EmailExistsAsync(dto.Email);
            if (emailExists) return ServiceResult<User>.Fail("Користувач з таким email вже існує");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = passwordHash,
                Role = UserRole.User
            };

            await _userRepository.AddAsync(user);


            return ServiceResult<User>.Ok(user);
        }

        public async Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null) return ServiceResult<AuthResponseDto>.Fail("Невірний email або пароль");

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            
            if (!isPasswordValid) return ServiceResult<AuthResponseDto>.Fail("Невірний email або пароль");

            var token = GenerateToken(user);
            
            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role
            };

            var authResponseDto = new AuthResponseDto
            {
                Token = token,
                User = userDto
            };

            return ServiceResult<AuthResponseDto>.Ok(authResponseDto);
        }

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
