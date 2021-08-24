using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi;

public interface IUserRepository
{
    List<User> GetAllUsers();
    User GetUserById(int id);
    void AddUser(User item);
    bool UpdateUser(int id, User item);
    bool DeleteUserById(int id);
}

