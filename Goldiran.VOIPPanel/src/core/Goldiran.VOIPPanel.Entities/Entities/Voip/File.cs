using Voip.Framework.Domain;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Entities;

public class File: BaseQueryEntity<Guid>
{
    public FileOwnerType FileOwnerTypeId { get;  set; }
    public Guid? FileOwnerId { get;  set; }
    public string FileName { get;  set; }
    public string Name { get;  set; }
    public long Length { get;  set; }
    public string ContentType { get;  set; }
    public FileContent FileContent { get; set; }

    //public Guid? SessionId { get; set; }
}
