using Goldiran.VOIPPanel.Application.Common.Enums;

namespace Goldiran.VOIPPanel.Application.Common.Services.ExportToExcell.Models
{
    public class ExportExcellModel
    {
        public string Request { get; set; } = "{}";
        public string? customColumns { get; set; }
        //public List<string> customColumns { get; set; }
        public FormNameEnum FormName { get; set; } 
    }
}
