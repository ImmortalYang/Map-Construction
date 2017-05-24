using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_application1.Models.MapUnitViewModels
{
    public class MapUnitIndexViewModel
    {
        public ICollection<City> Cities { get; set; }
        public ICollection<Road> Roads { get; set; }
        public ICollection<Pass> Passes { get; set; }
    }
}
