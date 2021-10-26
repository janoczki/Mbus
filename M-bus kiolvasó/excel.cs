using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace M_bus_kiolvasó
{
    public static class excel
    {
        public static void CreateExcel(List<string> flats, List<string> numbers, List<decimal> meterValues, string fileName)
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlApp.DisplayAlerts = false;
            if (xlApp == null)
            {
                MessageBox.Show("Excel is not properly installed!!");
                return;
            }

            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells.Range["A1", "E1"].Font.Bold = true;
            xlWorkSheet.Cells.Range["A1", "A216"].Font.Bold = true;
            
            xlWorkSheet.Cells[1, 1] = DateTime.Now.ToString();
            xlWorkSheet.Cells[1, 2] = "Mérőóra száma";
            xlWorkSheet.Cells[1, 3] = "Hőmennyiség (Wh)";
            xlWorkSheet.Cells[1, 4] = "Hideg víz (m3)";
            xlWorkSheet.Cells[1, 5] = "Meleg víz (m3)";
            
            int i = 1;
            foreach (string flat in flats)
            {
                xlWorkSheet.Cells[i + 1, 1] = flat + " számú lakás";
                xlWorkSheet.Cells[i + 1, 2] = numbers[flats.IndexOf(flat)];
                xlWorkSheet.Cells[i + 1, 3] = meterValues[(i - 1) * 3 + 0];
                xlWorkSheet.Cells[i + 1, 4] = meterValues[(i - 1) * 3 + 1];
                xlWorkSheet.Cells[i + 1, 5] = meterValues[(i - 1) * 3 + 2];
                i++;
            }
            xlWorkSheet.Cells.Range["A1", "D1"].Columns.AutoFit();

            xlWorkBook.SaveAs(Application.StartupPath + "\\" + fileName + ".xlsx", Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook, misValue,
            misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);

            xlApp.Quit();

            if (xlWorkBook != null) { Marshal.ReleaseComObject(xlWorkBook); } //release each workbook like this
            if (xlWorkSheet != null) { Marshal.ReleaseComObject(xlWorkSheet); } //release each worksheet like this
            if (xlApp != null) { Marshal.ReleaseComObject(xlApp); } //release the Excel application
            xlWorkBook = null; //set each memory reference to null.
            xlWorkSheet = null;
            xlApp = null;
            GC.Collect();

        }
    }
}
