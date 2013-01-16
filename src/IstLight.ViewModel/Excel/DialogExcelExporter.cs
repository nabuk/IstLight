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
using System.IO;
using System.Linq;
using System.Windows.Input;
using OfficeOpenXml;
using winForms = System.Windows.Forms;

namespace IstLight.Excel
{
    public abstract class DialogExcelExporter<T> : IExcelExporter<T>
    {
        public ICommand GetExportCommand(T data)
        {
            var exportCommand = new DelegateCommand(() =>
                {
                    var dlg = new winForms.SaveFileDialog
                    {
                        Filter = "Excel files (*.xlsx)|*.xlsx",
                        AddExtension = true
                    };
                    if (dlg.ShowDialog() == winForms.DialogResult.OK)
                    {
                        try
                        {
                            ExportData(data, new ExcelPackage()).SaveAs(new FileInfo(dlg.FileName));
                        }
                        catch (Exception ex)
                        {
                            winForms.MessageBox.Show(ex.Message, "Error", winForms.MessageBoxButtons.OK, winForms.MessageBoxIcon.Error);
                        }
                    }
                },
                () => CanExport(data));

            CanExportChangedAction(data, exportCommand.RaiseCanExecuteChanged);

            return exportCommand;
        }

        protected abstract ExcelPackage ExportData(T data, ExcelPackage efile);
        protected abstract bool CanExport(T data);
        protected virtual void CanExportChangedAction(T data, Action canExportChangedTrigger) { }

        protected void SetAutoWidth(ExcelWorksheet sheet)
        {
            foreach (int columnI in Enumerable.Range(sheet.Dimension.Start.Column, (sheet.Dimension.End.Column - sheet.Dimension.Start.Column) + 1))
            {
                var cells = sheet.Cells[sheet.Dimension.Start.Row, columnI, sheet.Dimension.End.Row, columnI];

                if (cells.Any())
                {
                    int maxLength = cells.Max(cell => (cell.Value ?? "x").ToString()
                        .Split('\n')
                        .Aggregate((s1,s2) => s1.Length > s2.Length ? s1 : s2)
                        .Length);

                    sheet.Column(columnI).Width = maxLength + 2;
                }
            }
        }
    }
}
