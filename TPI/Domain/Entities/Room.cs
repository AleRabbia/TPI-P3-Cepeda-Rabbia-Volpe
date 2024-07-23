using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Domain.Enums;

namespace Domain.Entities
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "REAL")]
        public float Price { get; set; }

        [Required]
        [Range(0, 5, ErrorMessage = "Score must be between 0 and 5.")]
        [Column(TypeName = "REAL")]
        public float Score { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        [Column(TypeName = "NVARCHAR(100)")]
        public string Service { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(50)")]
        public CategoryRoom Category { get; set; }

        [Required]
        public int Occupation { get; set; }

        public ICollection<Booking> Bookings { get; set; }

        public Room() { }
    }
}
