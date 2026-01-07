using Voip.Framework.Domain.Extensions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.ReadModel.Dto;

public class RasadTeamOperationDto
{
    public OperationType OperationTypeId { get; set; }
    public string OperationType { get { return OperationTypeId != 0 ? OperationTypeId.Description() : string.Empty; } }
    public int Count {  get; set; }
}
