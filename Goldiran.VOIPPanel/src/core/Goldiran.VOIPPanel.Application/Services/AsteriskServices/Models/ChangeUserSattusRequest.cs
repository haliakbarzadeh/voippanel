using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.ReadModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;

public class ChangeUserSattusRequest
{
   public List<Queu>? QueueList { get; set; } = new List<Queu>();
   public string Exten {  get; set; }
   public OperationType OperationType {  get; set; }
}
