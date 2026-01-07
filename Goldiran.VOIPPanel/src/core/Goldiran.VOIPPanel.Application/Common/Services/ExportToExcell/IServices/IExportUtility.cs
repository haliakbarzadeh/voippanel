using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Text;


namespace Goldiran.VOIPPanel.Application.Common.Services.ExportToExcell.IServices
{
    public interface IExportUtility
    {
        IWorkbook WriteExcelWithNPOI<T>(List<T> data, List<string> customColumns, string extension);
    }
}
