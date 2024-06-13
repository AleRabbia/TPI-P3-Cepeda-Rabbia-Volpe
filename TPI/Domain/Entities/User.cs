using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Last name cannot be longer than 100 characters.")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

    //    [Required]
    //    [EnumDataType(typeof(Role))]
    //    public Role Role { get; set; }
    //}

    //public enum Role
    //{
    //    Admin,
    //    Visitor,
    //    Customer

    //}
}
