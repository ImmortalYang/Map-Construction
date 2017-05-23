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

    [Flags]
    public enum Orientation
    {
        None = 0,
        Left = 1,
        Right = 2,
        Top = 4,
        Bottom = 8,
        TopLeft = Top | Left,
        TopRight = Top | Right,
        BottomLeft = Bottom | Left,
        BottomRight = Bottom | Right,
        Horizontal = Left | Right,
        Vertical = Top | Bottom,
        All = Left | Right | Top | Bottom
    }

}
