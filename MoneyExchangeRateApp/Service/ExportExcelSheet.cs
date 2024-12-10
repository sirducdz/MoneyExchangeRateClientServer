using DataLayer.ModelDTOs;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MoneyExchangeRateApp.Service
{
    public class ExportExcelSheet
    {
        public ExportExcelSheet(ListView listViewData, DateTime dt)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible = true;
            Worksheet worksheet1 = excel.Workbooks.Add(Missing.Value).Sheets[1];
            worksheet1.Range["A1:C1"].Merge();
            worksheet1.Range["A2:C2"].Merge();
            string title = "Rate Currency History Data List";
            worksheet1.Range["A1:C1"].Value = title;
            worksheet1.Range["A1:C1"].Font.Bold = true;
            worksheet1.Range["A2:C2"].Value = dt.ToShortDateString();
            GridView gridView = listViewData.View as GridView;
            for (int i = 0; i < gridView.Columns.Count; i++)
            {
                Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)worksheet1.Cells[4, i + 1];
                myRange.Font.Bold = true;
                string header = gridView.Columns[i].Header.ToString();
                worksheet1.Columns[i + 1].ColumnWidth = header.Length + 5;
                myRange.Value2 = header;
            }
            for (int i = 0; i < listViewData.Items.Count; i++)
            {
                var item = listViewData.Items[i] as RateHistoryDTO;
                worksheet1.Cells[i + 5, 1].Value = item.ExchangeRateId;
                worksheet1.Cells[i + 5, 2].Value = item.CurrencySourceName;
                worksheet1.Cells[i + 5, 3].Value = item.CurrencyTargetName;
                worksheet1.Cells[i + 5, 4].Value = item.Date;
                worksheet1.Cells[i + 5, 5].Value = item.SourceCurrencyPrice;
                worksheet1.Cells[i + 5, 6].Value = item.TargetCurrencyPrice;
            }
        }
    }
}
