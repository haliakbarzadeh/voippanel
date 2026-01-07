using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Text;


namespace Goldiran.VOIPPanel.Application.Common.Services.ExportToExcell.IServices
{
    public interface IPackageExportUtility
    {
        IWorkbook WriteExcelWithNPOI<T>(List<List<T>> data, List<string> sheetNamesList, List<string> customColumns, string extension);
    }
}
