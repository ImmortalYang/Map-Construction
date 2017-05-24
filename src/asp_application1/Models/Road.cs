using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace asp_application1.Models
{
    public class Road: MapUnit
    {
        [Required]
        public Orientation Orientation { get; set; }
    }

    public enum Orientation
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Horizontal,
        Vertical,
        All
    }

}
