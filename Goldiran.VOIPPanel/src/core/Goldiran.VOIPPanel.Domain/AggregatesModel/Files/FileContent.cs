using Voip.Framework.Domain;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.Files;

public class FileContent : Entity<Guid>
{
    public byte[] Content { get; private set; }

    public FileContent( byte[] content)
    {
        Id = Guid.NewGuid();
        Content = content;
    }
}