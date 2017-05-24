using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace asp_application1.Models
{
    public class Pass: MapUnit
    {
        [Required]
        [Range(0, 1000, ErrorMessage = "Please enter a value between 0 and 1000.")]
        public int Duration { get; set; }
    }
}
