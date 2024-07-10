using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string CompleteName { get => Name + " " + LastName; }
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }
        public string Role { get; set; }

        public static UserDto Create(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Birthdate = user.Birthdate,
                Role = user.Role.ToString()
            };
        }
    }

}
