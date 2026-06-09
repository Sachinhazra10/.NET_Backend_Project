using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UserManagementApi.Models;

namespace UserManagementApi.Services
{
    public class UserService : IUserService
    {
        private readonly ConcurrentDictionary<int, User> _users = new();
        private int _nextId = 1;
        private readonly object _idLock = new();

        public UserService()
        {
            // Seed some default users
            Create(new User { Username = "alice", Email = "alice@example.com", Age = 28, Role = "Admin" });
            Create(new User { Username = "bob", Email = "bob@example.com", Age = 32, Role = "User" });
            Create(new User { Username = "charlie", Email = "charlie@example.com", Age = 24, Role = "Manager" });
        }

        public IEnumerable<User> GetAll()
        {
            return _users.Values.OrderBy(u => u.Id);
        }

        public User? GetById(int id)
        {
            _users.TryGetValue(id, out var user);
            return user;
        }

        public User Create(User user)
        {
            lock (_idLock)
            {
                user.Id = _nextId++;
            }
            user.CreatedAt = DateTime.UtcNow;
            _users[user.Id] = user;
            return user;
        }

        public bool Update(int id, User user)
        {
            if (!_users.ContainsKey(id))
            {
                return false;
            }

            user.Id = id; // Ensure ID remains consistent
            _users[id] = user;
            return true;
        }

        public bool Delete(int id)
        {
            return _users.TryRemove(id, out _);
        }
    }
}
