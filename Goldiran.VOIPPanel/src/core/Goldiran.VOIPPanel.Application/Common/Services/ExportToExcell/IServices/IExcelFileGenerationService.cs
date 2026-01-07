using Goldiran.VOIPPanel.Application.Common.Services.ExportToExcell.Models;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace Goldiran.VOIPPanel.Application.Common.Services.ExportToExcell.IServices
{
    public interface IExcelFileGenerationService
    {
        ExcelFileResponse GenerateExcelFile(DataTable data, List<string> columns,string fileName);
    }
}
