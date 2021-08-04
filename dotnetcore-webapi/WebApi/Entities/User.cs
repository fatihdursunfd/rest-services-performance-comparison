using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int UserId { get; set; }     
        public string Name { get; set; }
        public string SurName { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
        public Address address { get; set; }

    }
}