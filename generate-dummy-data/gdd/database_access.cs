using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace gdd
{
    public class database_access
    {
        public IMongoCollection<User> collection { get; set; }
        public void ConnectMongoDb()
        {
 
            var client = new MongoClient(
                "mongodb+srv://fatihdursunfd:Fatih.6116@cluster0.xu9mo.mongodb.net/test?retryWrites=true&w=majority"
            );
            var database = client.GetDatabase("fd");
            collection = database.GetCollection<User>("users");
        }
        
        public void AddUser(User user)
        {
            collection.InsertOne(user);
        }
        public void AddUsers(List<User> users)
        {
            collection.InsertMany(users);
        }
        

        
    }
}