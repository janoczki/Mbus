using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.BACnet;

namespace M_bus_kiolvasó
{
    public partial class Form1 : Form
    {
        //List<string> meterNamesToRead;
        List<string> addressesToRead = new List<string>();
        List<string> meterValues = new List<string>();


        List<string> meterNamesToRead;
        string metersInDevice1;
        string metersInDevice2;
        string metersInDevice3;
        public Form1()
        {
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            Bac.startActivity("10.1.1.51");
            meterNamesToRead = meter.collectMeters();

            metersInDevice1 = collectAvailableMeters();

            timer1.Interval = 60000;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Hour == 1 && DateTime.Now.Minute == 0)
            {
                olvas(collectAvailableMeters());
            }
        }

        public string collectAvailableMeters()
        {
            label1.BackColor = Color.LightGray;
            label2.BackColor = Color.LightGray;
            metersInDevice1 = Bac.readAllAI(1, "10.1.1.205", 1205);
            metersInDevice2 = Bac.readAllAI(1, "10.1.1.206", 1206);
            metersInDevice3 = Bac.readAllAI(1, "10.1.1.207", 1207);
            var metersInDevice = metersInDevice1 + ";" + metersInDevice2 + ";" + metersInDevice3;

            label1.BackColor = Color.Green;
            return metersInDevice;
            
        }
        public void olvas(string metersInDevice)
        {
            var metersWeHave = metersInDevice.Split(';').ToList();
            var values = progress.getMeterValues(meterNamesToRead, metersWeHave);
            writeMetersToExcel(values);
            values.Clear();
        }

        private void writeMetersToExcel(List<decimal> values)
        {
            string filename = DateTime.Now.ToString().Replace(".", "").Replace(":", "");
            excel.CreateExcel(flat.collectFlats(), flat.collectMeterNumbers(), values, filename);
            label2.BackColor = Color.Green;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            olvas(collectAvailableMeters());
        }
    }
}
