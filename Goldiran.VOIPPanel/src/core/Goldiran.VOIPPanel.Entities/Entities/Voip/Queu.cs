using Voip.Framework.Domain;

namespace Goldiran.VOIPPanel.ReadModel.Entities;
public class Queu : BaseQueryEntity<int>
{
    public int Code { get; set; }
    public string Name { get; set; }
    public int Count { get; set; }
    public string IPAddress { get; set; }=string.Empty;
    public string User { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public bool IsSLA { get;  set; }
    public bool IsFCR { get; set; }

}
