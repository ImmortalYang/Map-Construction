using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace asp_application1.Models
{
    public class City : MapUnit
    {
        [Required]
        public string Name { get; set; }
    }
}
