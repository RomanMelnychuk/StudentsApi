using System;
using System.Collections.Generic;
using System.Text;
using StudentsApi.Models;
using StudentsApi.Repositories;

namespace StudentsApi.Tests
{
    public class FakeUserRepository : IUserRepository
    {
        public bool EmailExistsResult { get; set; }
        public User? UserToReturn { get; set; }

        public Task<bool> EmailExistsAsync(string email)
            => Task.FromResult(EmailExistsResult);

        public Task<User?> GetByEmailAsync(string email)
            => Task.FromResult(UserToReturn);

        public Task AddAsync(User user)
            => Task.CompletedTask;

    }
}
