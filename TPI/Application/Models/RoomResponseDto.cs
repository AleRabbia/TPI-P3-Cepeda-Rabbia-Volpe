using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Models
{
    public class RoomResponseDto
    {
        public int Id { get; set; }
        public float Price { get; set; }
        public float Score { get; set; }
        public string Service { get; set; }
        public string Category { get; set; }
        public int Occupation { get; set; }
    }

}
