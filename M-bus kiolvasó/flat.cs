using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace M_bus_kiolvasó
{
    public static class flat
    {
        public static List<string> collectFlats()
        {
            string file = File.ReadAllText(Application.StartupPath + "\\forras.csv");
            var flats = file.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            for (int i=0; i < flats.Count; i++)
            {
                flats[i] = flats[i].Substring(0, 3);
            }
            return flats;

        }

        public static List<string> collectMeterNumbers()
        {
            string file = File.ReadAllText(Application.StartupPath + "\\forras.csv");
            var numbers = file.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            for (int i = 0; i < numbers.Count; i++)
            {
                numbers[i] = numbers[i].Substring(4);
            }
            return numbers;
        }
    }

}
