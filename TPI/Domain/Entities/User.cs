using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public abstract class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Last name cannot be longer than 100 characters.")]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Column(TypeName = "nvarchar(100)")]
        public string Password { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string UserName { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string UserType { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
    }
}
