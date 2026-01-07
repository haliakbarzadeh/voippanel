using NPOI.SS.UserModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.XSSF.UserModel;
using Goldiran.VOIPPanel.Application.Common.Services.ExportToExcell.IServices;
using Goldiran.VOIPPanel.Application.Common.Services.ExportToExcell.Models;
using Goldiran.VOIPPanel.Application.Common.Enums;
using Voip.Framework.Common.Extensions;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Microsoft.AspNetCore.Authorization;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;

namespace Goldiran.VOIPPanel.Api.Controllers;

[Route("api/v1/[controller]/[action]")]
public class ExcellExportController : ApiControllerBase
{
    private IExportUtility _exportUtility;
    public ExcellExportController(IExportUtility exportUtility)
    {
        _exportUtility = exportUtility;
    }

    //[AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ExcellReport([FromQuery] ExportExcellModel query)
    {

        var workbook = await GenerateExcellFile(query);
        var file = GetFile(workbook, query.FormName);
        return file;
    }

    private async Task<IWorkbook> GenerateExcellFile(ExportExcellModel query)
    {
        IWorkbook excelloutput = null;

        dynamic obj = null;
        dynamic items = null;
        switch (query.FormName)
        {
            case FormNameEnum.UserOperations:
                var getOperationsQuery = JsonConvert.DeserializeObject<GetOperationsQuery>(query.Request);
                getOperationsQuery.ExcellReport = true;
                items = await Mediator.Send(getOperationsQuery);

                break;
            case FormNameEnum.ContactDetails:
                //var tempp = JsonConvert.DeserializeObject<GetTempContactDetailsQuery>(query.Request);
                //var getContactDetailsQuery = new GetContactDetailsQuery()
                //{
                //    ContactReportType = tempp.ContactReportType,
                //    DisablePaging = tempp.DisablePaging,
                //    FromDate = tempp.FromDate,
                //    ToDate = tempp.ToDate,
                //    FromTime = tempp.FromTime,
                //    ToTime = tempp.ToTime,
                //    OrderBy = tempp.OrderBy,
                //    PageNumber = tempp.PageNumber,
                //    PageSize = tempp.PageSize,
                //    Phone = tempp.Phone,
                //    OperationReportType = tempp.OperationReportType,
                //    Search = tempp.Search,
                //    Agents = tempp.Agents,
                //    TypeCalls = !tempp.TypeCalls.IsNullOrEmpty() ? string.Join(',', tempp.TypeCalls) : null,

                //};
                var getContactDetailsQuery = JsonConvert.DeserializeObject<GetContactDetailsQuery>(query.Request);
                getContactDetailsQuery.ExcellReport = true;
                items = await Mediator.Send(getContactDetailsQuery);
                break;
            case FormNameEnum.AutoDial:
                //query.Request = query.Request.Replace("\"typeCalls\":[],", "").Replace("\"Agents\":[],", "");
                //var temp = JsonConvert.DeserializeObject<GetTempContactDetailsQuery>(query.Request);
                //var getAutoDialsQuery = new GetContactDetailsQuery()
                //{
                //    ContactReportType = temp.ContactReportType,
                //    DisablePaging = temp.DisablePaging,
                //    FromDate = temp.FromDate,
                //    ToDate = temp.ToDate,
                //    FromTime = temp.FromTime,
                //    ToTime = temp.ToTime,
                //    OrderBy = temp.OrderBy,
                //    PageNumber = temp.PageNumber,
                //    PageSize = temp.PageSize,
                //    Phone = temp.Phone,
                //    OperationReportType = temp.OperationReportType,
                //    Search = temp.Search,
                //    Agents = temp.Agents,
                //    TypeCalls = !temp.TypeCalls.IsNullOrEmpty() ? string.Join(',', temp.TypeCalls) : null,

                //};

                var getAutoDialsQuery = JsonConvert.DeserializeObject<GetContactDetailsQuery>(query.Request);
                getAutoDialsQuery.ExcellReport = true;
                getAutoDialsQuery.ContactReportType = ContactReportType.AutoDial;
                items = await Mediator.Send(getAutoDialsQuery);
                break;
            case FormNameEnum.SafeCall:
                var getSecureCallsQuery = JsonConvert.DeserializeObject<GetSecureCallsQuery>(query.Request);
                getSecureCallsQuery.ExcellReport = true;
                items = await Mediator.Send(getSecureCallsQuery);
                break;
            case FormNameEnum.AskCustomer:
                var getAskCustomersQuery = JsonConvert.DeserializeObject<GetAskCustomersQuery>(query.Request);
                getAskCustomersQuery.ExcellReport = true;
                items = await Mediator.Send(getAskCustomersQuery);
                break;
            case FormNameEnum.QueueLog:
                var getQueueLogsQuery = JsonConvert.DeserializeObject<GetQueueLogsQuery>(query.Request);
                getQueueLogsQuery.ExcellReport = true;
                items = await Mediator.Send(getQueueLogsQuery);
                break;
            case FormNameEnum.NewAutoDial:
                var newAutoDialQuery = JsonConvert.DeserializeObject<GetAutoDetailsQuery>(query.Request);
                newAutoDialQuery.ExcellReport = true;
                items = await Mediator.Send(newAutoDialQuery);
                break;
            case FormNameEnum.AnsweringMachine:
                var answeringMachineQuery = JsonConvert.DeserializeObject<GetQueueLogsQuery>(query.Request);
                answeringMachineQuery.ExcellReport = true;
                answeringMachineQuery.Queues = new List<int>() { 20 };
                items = await Mediator.Send(answeringMachineQuery);
                break;
            case FormNameEnum.AnsweringMachineManagement:
                var answeringMachineManageQuery = JsonConvert.DeserializeObject<GetAnsweringMachinesQuery>(query.Request);
                answeringMachineManageQuery.ExcellReport = true;
                items = await Mediator.Send(answeringMachineManageQuery);
                break;
            case FormNameEnum.SoftPhoneEvents:
                var softphoneEventsQuery = JsonConvert.DeserializeObject<GetSoftPhoneEventsQuery>(query.Request);
                softphoneEventsQuery.ExcellReport = true;
                items = await Mediator.Send(softphoneEventsQuery);
                break;
            default:
                break;
        }
        //obj.ExcellReport = true;

        //dynamic items = await Mediator.Send(obj);
        //var obj= JsonConvert.DeserializeObject<GetOperationsQuery>(query.Request);
        //dynamic items = await Mediator.Send(obj);
        //
        var customColumns = !string.IsNullOrEmpty(query.customColumns) ? query.customColumns.Split(';').ToList() : null;
        if (customColumns != null)
        {
            for (int i = 0; i < customColumns.Count; i++)
            {
                customColumns[i] = Char.ToUpper(customColumns[i][0]) + customColumns[i].Substring(1, customColumns[i].Length - 1);
            }
        }

        //
        excelloutput = _exportUtility.WriteExcelWithNPOI(items.Items, customColumns, "xlsx");

        return excelloutput;

    }

    private FileStreamResult GetFile(IWorkbook workbook, FormNameEnum formName)
    {
        var date = DateTime.Now;
        var fileName = formName.Description() + $"_{date.Year}{date.Month}{date.Day}{date.Hour}{date.Minute}{date.Second}";
        fileName += ((workbook.GetType() == typeof(XSSFWorkbook)) ? ".xlsx" : "xls");
        string contentType = "";

        MemoryStream tempStream = null;
        MemoryStream stream = null;

        tempStream = new MemoryStream();
        workbook.Write(tempStream, false);
        // 2. Convert the tempStream to byteArray and copy to another stream
        var byteArray = tempStream.ToArray();
        stream = new MemoryStream();
        stream.Write(byteArray, 0, byteArray.Length);
        stream.Seek(0, SeekOrigin.Begin);
        // 3. Set file content type
        contentType = workbook.GetType() == typeof(XSSFWorkbook) ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/vnd.ms-excel";
        // 4. Return file
        return File(stream, contentType, fileName);

    }



}