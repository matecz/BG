using System;
using bgAPI.Entities.Base;

namespace BGDomain.Entities
{
    public class User : EntityBase
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Hobbies { get; set; }
    }
}