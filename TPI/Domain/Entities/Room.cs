using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "float")] //Para SQL Server
        //[Column(TypeName = "REAL")] Para SQLite
        public float Price { get; set; }

        [Required]
        [Range(0, 5, ErrorMessage = "Score must be between 0 and 5.")]
        [Column(TypeName = "float")] //Para SQL Server
        //[Column(TypeName = "REAL")] Para SQLite
        public float Score { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Service description cannot be longer than 100 characters.")]
        [Column(TypeName = "nvarchar(100)")]
        public string Service { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Category cannot be longer than 50 characters.")]
        [Column(TypeName = "nvarchar(50)")]
        public string Category { get; set; }
    }
}
