using Voip.Framework.Domain;

namespace Goldiran.VOIPPanel.ReadModel.Entities;

public class FileContent : BaseQueryEntity<Guid>
{
    public byte[] Content { get;  set; }

}