using Goldiran.VOIPPanel.Application.Features.Rasads.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;

public class ChanSpyRequest
{
    public string IP { get; set; } = string.Empty;

    public string Extension {  get; set; }=string.Empty;
    public string Caller {  get; set; }=string.Empty ;
    public SpyType SpyType { get; set; }
}
