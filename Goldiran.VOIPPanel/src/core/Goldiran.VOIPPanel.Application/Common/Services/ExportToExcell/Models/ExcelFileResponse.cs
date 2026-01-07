using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Common.Services.ExportToExcell.Models
{
    public class ExcelFileResponse
    {
        public MemoryStream Stream { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
