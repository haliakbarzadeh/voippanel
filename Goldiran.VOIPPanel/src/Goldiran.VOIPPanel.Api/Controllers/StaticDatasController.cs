
using Goldiran.VOIPPanel.Api.Controllers;
using Goldiran.VOIPPanel.Application.Common.Enums;
using Goldiran.VOIPPanel.Domain.AggregatesModel.AnsweringMachines.Enums;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations.Enums;
using Goldiran.VOIPPanel.Domain.AggregatesModel.SoftPhoneEvents.Enums;
using Goldiran.VOIPPanel.Domain.Common.Enums;
using Goldiran.VOIPPanel.ReadModel.Enums;
using Microsoft.AspNetCore.Mvc;
using Voip.Framework.Common.Extensions;

namespace Saramad.EndPoints.Api.Controllers;

public class StaticDatasController : ApiControllerBase
{


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<StaticData>>> List()
    {
        var result = new List<StaticData>
        {
            StaticEntity<PositionType>.GetStaticData(),
            StaticEntity<OperationReportType>.GetStaticData(),
            StaticEntity<FormNameEnum>.GetStaticData(),
            StaticEntity<CallType>.GetStaticData(),
            StaticEntity<HourType>.GetStaticData(),
            StaticEntity<SecureReportType>.GetStaticData(),
            StaticEntity<WallboardReportType>.GetStaticData(),
            StaticEntity<ShiftType>.GetStaticData(),
            StaticEntity<QueueLogEventType>.GetStaticData(),
            StaticEntity<SecureReportType>.GetStaticData(),
            StaticEntity<AMStatusType>.GetStaticData(),
            StaticEntity<SoftPhoneEventType>.GetStaticData(),
            StaticEntity<UserType>.GetStaticData()
        };

        return result.DistinctBy(e => e.Name).OrderBy(e => e.Name).ToList();
    }
    
}


public class StaticData
{
    public string Name { get; set; }
    public List<StaticValue> Values { get; set; } = new List<StaticValue>();
}
public class StaticValue
{
    public int Id { get; set; }
    public string Key { get; set; }
    public string Title { get; set; }

}

public class StaticEntity<TEnum> where TEnum : Enum
{


    public static StaticData GetStaticData()
    {
        var staticData = new StaticData();
        var type = typeof(TEnum);
        staticData.Name = type.Name;

        var keys = Enum.GetValues(typeof(TEnum));
        var values = Enum.GetValuesAsUnderlyingType(typeof(TEnum));

        int index = 0;
        foreach (var key in keys)
        {

            var _id = (TEnum)values.GetValue(index);
            var _int = (int)values.GetValue(index);

            var _key = key.ToString();

            staticData.Values.Add(new StaticValue
            {
                Id = _int,
                Key = _key,
                Title = _id.Description(),
            });

            index++;
        }

        return staticData;
    }


}
