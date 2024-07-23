using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Nombre no puede tener mas de 100 caracteres")]
        [Column(TypeName = "NVARCHAR(100)")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Apellido no puede tener mas de 100 caracteres")]
        [Column(TypeName = "NVARCHAR(100)")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Dirección de correo incorrecta")]
        [Column(TypeName = "NVARCHAR(100)")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Column(TypeName = "NVARCHAR(100)")]
        public string Password { get; set; }  

        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public ICollection<Booking> Bookings { get; set; }
        public User() { }
    }
}
