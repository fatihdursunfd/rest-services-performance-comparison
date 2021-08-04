using System;

namespace WebApi
{
    public class UserPostView
    {
        public int UserId { get; set; }     
        public string Name { get; set; }
        public string SurName { get; set; }
        public DateTime Birthday { get; set; }
        public Address address { get; set; }
    }
}