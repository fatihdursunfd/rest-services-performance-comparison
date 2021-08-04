using System;

namespace WebApi
{
    public class UserGetView
    {
        public int UserId { get; set; }     
        public string Name { get; set; }
        public string SurName { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
        public Address address { get; set; }
    }
}