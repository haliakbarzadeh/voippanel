using Goldiran.VOIPPanel.HostedService.BackGroundService.Enums;

namespace Goldiran.VOIPPanel.HostedService.BackGroundService.Models.Request;

public class CreateFlatContactDetailListRequest
{
    public IList<CreateFlatContactDetailCommand> CreateFlatContactDetailCommands { get; set; } = new List<CreateFlatContactDetailCommand>();
    public class CreateFlatContactDetailCommand
    {
        public string LinkedId { get; set; }
        public string DstChannel { get; set; }
        public string Status { get; set; }
        public string Disposition { get; set; }
        public DateTime Date { get; set; }
        public string Dcontext { get; set; }
        public string? Source { get; set; }
        public string? Dest { get; set; }
        public int? Billsecond { get; set; }
        public int? Duration { get; set; }
        public int? Waiting { get; set; }
        public string? Filepath { get; set; }
        public string? Recordingfile { get; set; }
        public ReportType ReportType { get; set; }

    }
}
