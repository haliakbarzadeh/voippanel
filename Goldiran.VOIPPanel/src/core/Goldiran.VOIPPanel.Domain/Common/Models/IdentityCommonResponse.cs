using System.Collections.Generic;

namespace Goldiran.VOIPPanel.Domain.Common.Models;
public class IdentityCommonResponse
{
    public bool Succeeded { get; set; }
    public List<string> Errors { get; set; }
}
