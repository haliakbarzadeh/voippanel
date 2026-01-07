using Goldiran.VOIPPanel.HostedService.BackGroundService.Enums;
using Goldiran.VOIPPanel.HostedService.BackGroundService.IServices;
using Goldiran.VOIPPanel.HostedService.BackGroundService.Models.Request;

namespace Goldiran.VOIPPanel.HostedService.BackGroundService.Services;

public class ExecuteFlatDataService : IExecuteFlatDataService
{
    private readonly IContactDetailsService _contactDetailsService;
    private readonly IFlatDataJobService _flatDataJobService;

    public ExecuteFlatDataService(IContactDetailsService contactDetailsService, IFlatDataJobService flatDataJobService)
    {
        _flatDataJobService = flatDataJobService;
        _contactDetailsService = contactDetailsService;
    }
    public async Task ExecuteService(ReportType reportType, string token)
    {
        int pageSize = 100;
        //if (reportType == ReportType.AutoDial)
        //    pageSize = 300;
        //else
        //    pageSize = 100;

        CreateFlatContactDetailListJobRequest contactDetailRequest = null;
        DateTime dateTime = DateTime.Now;

        var lastJob = await _flatDataJobService.GetFlatDataJobLast(new GetFlatDataJobLastRequest() { ReportType = reportType },token);
        if (lastJob == null)
        {
            contactDetailRequest = new CreateFlatContactDetailListJobRequest() { FromDate = new DateTime(2025, 3, 21), FromTime = new TimeSpan(0, 0, 0), ToDate = new DateTime(2025, 3, 21), ToTime = new TimeSpan(0, 30, 0), IsJob = true, IsRestricted = false, ContactReportType = (ContactReportType)((int)reportType), OrderBy = 1, PageNumber = 1, PageSize = pageSize };
            //contactDetailRequest = new CreateFlatContactDetailListJobRequest() { FromDate = new DateTime(2025, 4, 15), FromTime = new TimeSpan(9, 0, 0), ToDate = new DateTime(2025, 4, 15), ToTime = new TimeSpan(9, 30, 0), IsJob = true, IsRestricted = false, ContactReportType = (ContactReportType)((int)reportType), OrderBy = 1, PageNumber = 1, PageSize = 100 };
            //contactDetailRequest = new CreateFlatContactDetailListJobRequest() { FromDate = new DateTime(2025, 3, 21), FromTime = new TimeSpan(20, 30, 0), IsJob = true, IsRestricted = false, ContactReportType = (ContactReportType)((int)reportType), OrderBy = 1, PageNumber = 1, PageSize = 1000 };

        }
        else
        {
            if((dateTime-lastJob.LastDate).TotalMinutes<=30)
                return;

            DateTime lastDate =new DateTime(lastJob.LastDate.Date.Year, lastJob.LastDate.Date.Month, lastJob.LastDate.Date.Day, lastJob.LastDate.Hour, lastJob.LastDate.Minute, lastJob.LastDate.Second, lastJob.LastDate.Millisecond) ;
            lastDate=DateTime.Now.AddMinutes(-15);
            //contactDetailRequest = new CreateFlatContactDetailListJobRequest() { FromDate = lastJob.LastDate.Date, FromTime = lastJob.LastDate.TimeOfDay, ToDate = lastDate.Date, ToTime =lastDate.TimeOfDay, IsJob = true, IsRestricted = false, ContactReportType = (ContactReportType)((int)reportType), OrderBy = 1, PageNumber = 1, PageSize = 10000 };
            contactDetailRequest = new CreateFlatContactDetailListJobRequest() { FromDate = lastJob.LastDate.Date, FromTime = lastJob.LastDate.TimeOfDay,ToDate=lastDate.Date,ToTime=lastDate.TimeOfDay,IsJob = true, IsRestricted = false, ContactReportType = (ContactReportType)((int)reportType), OrderBy = 1, PageNumber = 1, PageSize = pageSize };

        }

        var contactDetail=await _contactDetailsService.CreateFlatContactDetailJob(contactDetailRequest,token);

        //long result = 0;
        //if (contactDetail != null && contactDetail.Date.Year != 2000)
        //    result = await _flatDataJobService.CreateFlatDataJob(new CreateFaltDataJobRequest() { LastDate = contactDetail.Date, ReportType = reportType, Status = true, Count = contactDetail.Count }, token);
        //else if (contactDetail != null && contactDetail.Date.Year == 2000 )
        //    result = await _flatDataJobService.CreateFlatDataJob(new CreateFaltDataJobRequest() { LastDate = lastJob != null ? lastJob.LastDate.AddMinutes(30) : new DateTime(2025, 3, 21, 0, 30, 0), ReportType = reportType, Status = true }, token);
    }
}
