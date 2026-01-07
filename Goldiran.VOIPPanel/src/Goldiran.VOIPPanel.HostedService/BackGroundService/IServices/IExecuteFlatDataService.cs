using Goldiran.VOIPPanel.HostedService.BackGroundService.Enums;

namespace Goldiran.VOIPPanel.HostedService.BackGroundService.IServices;

public interface IExecuteFlatDataService
{
    public Task ExecuteService(ReportType reportType, string token);

}
