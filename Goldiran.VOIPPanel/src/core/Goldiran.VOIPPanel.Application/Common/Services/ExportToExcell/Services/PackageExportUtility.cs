using Voip.Framework.Common.Exceptions;
using Goldiran.VOIPPanel.Application.Common.Services.ExportToExcell.IServices;
using Goldiran.VOIPPanel.Domain.Common.Attributes;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Voip.Framework.Common.Extensions;


namespace Saramad.Core.ApplicationService.Common.Services.ExportToExcell.Services
{
    public class PackageExportUtility : IPackageExportUtility
    {
        public IWorkbook WriteExcelWithNPOI<T>(List<List<T>> data, List<string> sheetNamesList, List<string> customColumns=null, string extension = "xlsx")
        {
            // Get DataTable

            // Instantiate Wokrbook
            IWorkbook workbook;
            if (extension == "xlsx")
            {
                workbook = new XSSFWorkbook();
            }
            else if (extension == "xls")
            {
                workbook = new HSSFWorkbook();
            }
            else
            {
                throw new Exception("The format '" + extension + "' is not supported.");
            }

            int i = 0;
            foreach (var item in data)
            {
                WriteExcelSheetWithNPOI(item, sheetNamesList[i], workbook, customColumns);
                i++;
            }


            return workbook;
        }

        private void WriteExcelSheetWithNPOI<T>(List<T> data, string sheetName,IWorkbook workbook, List<string> customColumns = null)
        {
            DataTable dt = ConvertListToDataTable(data, customColumns);
            ISheet sheet1 = workbook.CreateSheet(sheetName.Length>31?sheetName.Substring(0,31):sheetName);

            //make a header row
            IRow row1 = sheet1.CreateRow(0);

            for (int j = 0; j < dt.Columns.Count; j++)
            {

                ICell cell = row1.CreateCell(j);
                string columnName = dt.Columns[j].ToString();
                cell.SetCellValue(columnName);
            }

            //loops through data
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    ICell cell = row.CreateCell(j);
                    string columnName = dt.Columns[j].ToString();
                    cell.SetCellValue(dt.Rows[i][columnName].ToString());
                    cell.CellStyle.WrapText = true; // NOT WORKING
                }
            }
        }


        private DataTable ConvertListToDataTable<T>(IList<T> data, List<string> customColumns)
        {
            var type=typeof(T);
            DataTable table = new DataTable();
            var props=type.GetProperties().Where(c=>customColumns!=null? customColumns.Contains(c.Name):true).ToList();
            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttributes(typeof(DtoAttributes), true);
                if (!attr.IsNullOrEmpty())
                {
                    
                    if(attr[0] is DtoAttributes a && a.IsDisplayed)
                    {
                        table.Columns.Add(a.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                    }
                }
                else if(prop.PropertyType is not IList)
                {
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }

            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (var prop in props)
                {
                    var attr = prop.GetCustomAttributes(typeof(DtoAttributes), true);
                    if (!attr.IsNullOrEmpty())
                    {

                        if (attr[0] is DtoAttributes a && a.IsDisplayed)
                        {
                            if(prop.PropertyType.FullName.Contains("DateTime"))
                            {
                                row[a.Name] = (object)(((DateTime)prop.GetValue(item)).ConvertDateTimeToJalaliDateTime()) ?? DBNull.Value;
                            }
                            else
                            {
                                row[a.Name] = prop.GetValue(item) ?? DBNull.Value;
                            }

                        }
                    }
                    else if (prop.PropertyType is not IList)
                    {
                        if (prop.PropertyType.FullName.Contains("DateTime"))
                        {
                            row[prop.Name] = (object)(((DateTime)prop.GetValue(item)).ConvertDateTimeToJalaliDateTime()) ?? DBNull.Value;
                        }
                        else
                        {
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                        }
                        //row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    }
                }
                    
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
