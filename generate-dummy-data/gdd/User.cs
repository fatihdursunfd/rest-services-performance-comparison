using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace gdd
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