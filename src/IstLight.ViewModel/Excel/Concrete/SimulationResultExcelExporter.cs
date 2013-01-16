// Copyright 2012 Jakub Niemyjski
//
// This file is part of IstLight.
//
// IstLight is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// IstLight is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with IstLight.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using IstLight.ViewModels;
using IstLight.ViewModels.ResultSections;
using OfficeOpenXml;

namespace IstLight.Excel.Concrete
{
    public class SimulationResultExcelExporter : DialogExcelExporter<SimulationResultViewModel>
    {
        private const string headerStyleName = "HeaderStyle";
        private string[] rowStyleName = new string[] { "RowStyle","AlternateRowStyle" };

        public SimulationResultExcelExporter()
        {

        }

        void SetStyles(ExcelPackage efile)
        {
            var namedStyle = efile.Workbook.Styles.CreateNamedStyle(headerStyleName);
            namedStyle.Style.Fill.Gradient.Type = OfficeOpenXml.Style.ExcelFillGradientType.Linear;
            namedStyle.Style.Fill.Gradient.Color1.SetColor(System.Drawing.Color.FromArgb(200, 210, 221));
            namedStyle.Style.Fill.Gradient.Color2.SetColor(System.Drawing.Color.FromArgb(220, 230, 241));
            namedStyle.Style.Font.Bold = true;
            namedStyle.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            namedStyle = efile.Workbook.Styles.CreateNamedStyle(rowStyleName[0]);

            namedStyle = efile.Workbook.Styles.CreateNamedStyle(rowStyleName[1]);
            namedStyle.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            namedStyle.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(220, 230, 241));

        }

        void ExportEquity(EquityViewModel data, ExcelPackage efile)
        {
            var sheet = efile.Workbook.Worksheets.Add(data.Header);

            var headerDescriptions = new string[]
            {
                "Bar",
                "Date",
                "Cash",
                "Interest",
                "Portfolio",
                "Transactions",
                "Commisions",
                "Transaction equity",
                "Total equity"
            };
            foreach (var h in headerDescriptions.Select((x, i) => new { Text = x, Id = i }))
            {
                var cell = sheet.Cells[1, h.Id + 1];
                cell.Value = h.Text;
                cell.StyleName = headerStyleName;
            }

            int lastCI = headerDescriptions.Length;
            foreach (var r in data.Rows.Select((x, i) => new { Id = i+2, Data = x }))
            {
                int cI = 1;
                sheet.Cells[r.Id, cI, r.Id, lastCI].StyleName = rowStyleName[r.Id % 2];

                sheet.Cells[r.Id, cI++].Value = r.Data.Bar;
                sheet.Cells[r.Id, cI].Style.Numberformat.Format = "yyyy-mm-dd";
                sheet.Cells[r.Id, cI++].Value = r.Data.Date;
                sheet.Cells[r.Id, cI++].Value = Math.Round(r.Data.Cash, 2);
                sheet.Cells[r.Id, cI++].Value = Math.Round(r.Data.Interest, 4);
                sheet.Cells[r.Id, cI++].Value = Math.Round(r.Data.Portfolio, 2);
                sheet.Cells[r.Id, cI++].Value = r.Data.TransactionCount;
                sheet.Cells[r.Id, cI++].Value = Math.Round(r.Data.Commissions, 2);
                sheet.Cells[r.Id, cI++].Value = Math.Round(r.Data.TransactionTotal, 2);
                sheet.Cells[r.Id, cI].Value = Math.Round(r.Data.Total, 2);
            }

            base.SetAutoWidth(sheet);
        }

        void ExportSummary(SummarySectionViewModel data, ExcelPackage efile)
        {
            
        }

        void ExportOutput(OutputViewModel data, ExcelPackage efile)
        {
            
        }

        #region DialogExcelExporter
        protected override ExcelPackage ExportData(SimulationResultViewModel data, ExcelPackage efile)
        {
            SetStyles(efile);
            ExportEquity(data.Sections.OfType<EquityViewModel>().First(), efile);
            ExportSummary(data.Sections.OfType<SummarySectionViewModel>().First(), efile);
            ExportOutput(data.Sections.OfType<OutputViewModel>().First(), efile);
            return efile;
        }
        protected override bool CanExport(SimulationResultViewModel data)
        {
            return data.Sections.OfType<SummarySectionViewModel>().First().Groups.All(g => g.LoadState == AsyncState.Completed);
        }
        protected override void CanExportChangedAction(SimulationResultViewModel data, Action canExportChangedTrigger)
        {
            foreach (var g in data.Sections.OfType<SummarySectionViewModel>().First().Groups)
                g.PropertyChanged += (s,e) => { if(e.PropertyName=="LoadState")canExportChangedTrigger(); };
        }
        #endregion // DialogExcelExporter
    }
}
