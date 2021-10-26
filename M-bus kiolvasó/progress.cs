using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M_bus_kiolvasó
{
    public static class progress
    {
        //public static List<string> getMeterValues(List<string> namesToRead, List<string> whatWeHave)
        public static List<decimal> getMeterValues(List<string> namesToRead, List<string> whatWeHave)
        {

            List<decimal> values = new List<decimal>();
            foreach (var meterName in namesToRead)
            {
                bool found = false;
                foreach (string meter in whatWeHave)
                {
                    if (isMeterNameAvailable(meter, meterName))
                    {
                        var type = meter.Split('#')[0];
                        var instance = Convert.ToUInt16(meter.Split('#')[1]);
                        var name = meter.Split('#')[2];
                        // új cucc
                        var deviceIP = meter.Split('#')[3];
                        var devInstance = meter.Split('#')[4];
                        values.Add(Decimal.Parse(Bac.readValue(1, deviceIP, uint.Parse(devInstance), type, instance, "PV"), System.Globalization.NumberStyles.AllowExponent | System.Globalization.NumberStyles.AllowDecimalPoint));
                        // új cucc vége

                        //values.Add(Decimal.Parse(Bac.readValue(1, "192.168.16.156", 156, type, instance, "PV"), System.Globalization.NumberStyles.AllowExponent | System.Globalization.NumberStyles.AllowDecimalPoint));

                        found = true;
                        break;
                    }
                    
                }
                if (!found)
                {
                    values.Add(-999999.99m);

                }

            }

            return values;
        }

        public static Boolean isMeterNameAvailable(string meter, string meterName)
        {
            return meter.Contains(meterName);
        }

    }
}
