using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace asp_application1.Models
{
    public class MapUnit
    {
        public int ID { get; set; }
        public string ApplicationUserId { get; set; }

        [Required]
        public int X { get; set; }

        [Required]
        public int Y { get; set; }

        public double Cost { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
