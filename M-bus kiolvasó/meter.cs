using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M_bus_kiolvasó
{
    public class meter
    {
        public string heatingEnergy { get; set; }
        public string hotWater { get; set; }
        public string coolWater { get; set; }
        public static List<string> collectMeters()
        {
            var flats = flat.collectFlats();
            var meters = new List<string>();
            foreach (var flat in flats)
            {
                var meter = new meter();
                meter.heatingEnergy = flat + "_HE";
                meter.hotWater = flat + "_CW";
                meter.coolWater = flat + "_HW";
                meters.Add(meter.heatingEnergy);
                meters.Add(meter.hotWater);
                meters.Add(meter.coolWater);
            }
            return meters;
        }

    }
}
