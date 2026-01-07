using Goldiran.VOIPPanel.Application.Common.Services.ExportToExcell.IServices;
using Goldiran.VOIPPanel.Application.Common.Services.ExportToExcell.Models;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;


namespace Goldiran.VOIPPanel.Application.Common.Services.ExportToExcell.Services
{
    public class ExcelFileGenerationService : IExcelFileGenerationService
    {
        public ExcelFileResponse GenerateExcelFile(DataTable data,List<string> columns, string fileName) 
        {
            var workBook=GetIworkBook(data, columns);
            return GetFile(workBook, fileName);
        }

        private IWorkbook GetIworkBook(DataTable data, List<string> columns)
        {
            IWorkbook workbook = new XSSFWorkbook(); ;


            ISheet sheet1 = workbook.CreateSheet("Sheet 1");

            //make a header row
            IRow row1 = sheet1.CreateRow(0);

            for (int j = 0; j < columns.Count; j++)
            {

                ICell cell = row1.CreateCell(j);
                string columnName = columns[j];
                cell.SetCellValue(columnName);
            }

            for (int i = 0; i < data.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(i + 1);
                for (int j = 0; j < data.Columns.Count; j++)
                {

                    ICell cell = row.CreateCell(j);
                    string columnName = data.Columns[j].ToString();
                    cell.SetCellValue(data.Rows[i][columnName].ToString());
                    cell.CellStyle.WrapText = true; // NOT WORKING
                }
            }

            return workbook;
        }

        private ExcelFileResponse GetFile(IWorkbook workbook, string fileName)
        {

            MemoryStream tempStream = new MemoryStream();
            workbook.Write(tempStream, false);

            var byteArray = tempStream.ToArray();
            MemoryStream stream = new MemoryStream();
            stream.Write(byteArray, 0, byteArray.Length);
            stream.Seek(0, SeekOrigin.Begin);

            var contentType = workbook.GetType() == typeof(XSSFWorkbook) ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/vnd.ms-excel";

            return new ExcelFileResponse()
            {
                FileName = fileName,
                ContentType = contentType,
                Stream= stream
            };

        }

    }
}
