using Goldiran.VOIPPanel.HostedService.BackGroundService.Models;
using Goldiran.VOIPPanel.HostedService.BackGroundService.Models.Request;
using Goldiran.VOIPPanel.HostedService.BackGroundService.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Goldiran.VOIPPanel.HostedService.BackGroundService.IServices;

public interface IContactDetailsService
{
    //public Task<PaginatedList<GetContactDetailsResponse>> GetContactDetails(GetContactDetailsRequest request, string token);
    //public Task<long> CreatreFlatContactDetailList(CreateFlatContactDetailListRequest request, string token);
    public Task<CreateFlatContactDetailListJobResponse> CreateFlatContactDetailJob(CreateFlatContactDetailListJobRequest request, string token);


}
