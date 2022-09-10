using System;
using System.Text.Json.Serialization;

namespace PideYa.Shared.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public ICollection<Order> Orders { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }
    }
}

