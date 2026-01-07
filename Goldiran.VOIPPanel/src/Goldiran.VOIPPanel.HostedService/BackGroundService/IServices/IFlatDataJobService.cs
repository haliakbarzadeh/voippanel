using Goldiran.VOIPPanel.HostedService.BackGroundService.Models.Request;
using Goldiran.VOIPPanel.HostedService.BackGroundService.Models.Response;

namespace Goldiran.VOIPPanel.HostedService.BackGroundService.IServices;

public interface IFlatDataJobService
{
    public Task<GetFlatDataJobLastResponse> GetFlatDataJobLast(GetFlatDataJobLastRequest request, string token);
    public Task<long> CreateFlatDataJob(CreateFaltDataJobRequest request, string token);

}
