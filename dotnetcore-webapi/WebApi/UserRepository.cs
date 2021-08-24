using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using WebApi;

public class UserRepository : IUserRepository
{
    private readonly UserContext _context = null;

    public UserRepository(IOptions<Settings> settings)
    {
        _context = new UserContext(settings);
    }

    public void AddUser(User item)
    {
        _context.Users.InsertOne(item);
    }
    public List<User> GetAllUsers()
    {
        var result = _context.Users.Find(new BsonDocument()).ToList();
        return result;
    }

    public User GetUserById(int id)
    {
        var filter = Builders<User>.Filter.Eq("UserId", id);
        var user = _context.Users.Find(filter).FirstOrDefault();
            
        return user;
    }

    public bool DeleteUserById(int id)
    {
        var filter = Builders<User>.Filter.Eq("UserId", id);
        var user = _context.Users.Find(filter).FirstOrDefault();

        if(user == null)
            return false;

        else{
            _context.Users.DeleteOne(filter);
            return true;
        }
    }

    public bool UpdateUser(int id, User updateduser)
    {   
        throw new System.NotImplementedException();
    }
}