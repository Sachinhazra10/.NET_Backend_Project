using System.Collections.Generic;
using UserManagementApi.Models;

namespace UserManagementApi.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User? GetById(int id);
        User Create(User user);
        bool Update(int id, User user);
        bool Delete(int id);
    }
}
