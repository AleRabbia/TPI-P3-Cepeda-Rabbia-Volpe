using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Application.Models.Requests
{
    public class AuthenticationRequest
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        [RegularExpression("^(Visitor|Customer|Admin)$", ErrorMessage = "UserType must be 'Visitor', 'Customer', or 'Admin'.")]
        public string? UserType { get; set; }
    }
}
