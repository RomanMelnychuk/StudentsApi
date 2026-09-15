using Microsoft.Extensions.Configuration;
using StudentsApi.DTOs;
using StudentsApi.Models;
using StudentsApi.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsApi.Tests
{
    public class AuthServiceTests
    {
        [Fact]
        public async Task RegisterAsync_WhenEmailExists_ReturnsFail()
        {
            // Arrange
            var fakeRepo = new FakeUserRepository { EmailExistsResult = true };
            var service = new AuthService(fakeRepo, null!);

            var dto = new RegisterDto { Email = "test@test.com", Password = "1234" };

            // Act
            var result = await service.RegisterAsync(dto);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task RegisterAsync_WhenEmailIsFree_ReturnsSuccess()
        {
            // Arrange
            var fakeRepo = new FakeUserRepository { EmailExistsResult = false };
            var service = new AuthService(fakeRepo, null!);

            var dto = new RegisterDto { Email = "test@test.com", Password = "1234" };

            // Act
            var result = await service.RegisterAsync(dto);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task LoginAsync_WhenUserNotFound_ReturnsFail()
        {
            var fakeRepo = new FakeUserRepository { UserToReturn = null };
            var service = new AuthService(fakeRepo, null!);

            var dto = new LoginDto { Email = "test@test.com", Password = "1234" };

            var result = await service.LoginAsync(dto);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task LoginAsync_WhenPasswordIsWrong_ReturnsFail()
        {
            var user = new User
            {
                Id = 1,
                Email = "test@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("correct_password"),
                Role = UserRole.User
            };
            var fakeRepo = new FakeUserRepository { UserToReturn = user };
            var service = new AuthService(fakeRepo, null!);

            var dto = new LoginDto { Email = "test@test.com", Password = "wrong_password" };

            var result = await service.LoginAsync(dto);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task LoginAsync_WhenPasswordIsCorrect_ReturnsToken()
        {
            var user = new User
            {
                Id = 1,
                Email = "test@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("correct_password"),
                Role = UserRole.User
            };
            var fakeRepo = new FakeUserRepository { UserToReturn = user };
            
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Key"] = "test_key_at_least_32_characters_long_12345",
                    ["Jwt:Issuer"] = "TestIssuer",
                    ["Jwt:Audience"] = "TestAudience"
                })
                .Build();

            var service = new AuthService(fakeRepo, configuration);

            var dto = new LoginDto { Email = "test@test.com", Password = "correct_password" };

            var result = await service.LoginAsync(dto);

            Assert.True(result.Success);
            Assert.False(string.IsNullOrEmpty(result.Data!.Token));
        }
    }
}
