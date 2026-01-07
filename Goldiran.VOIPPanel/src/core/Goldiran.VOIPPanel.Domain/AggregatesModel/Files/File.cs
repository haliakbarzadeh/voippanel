using Voip.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.Files;

public class File: AggregateRoot<Guid> 
{
    public FileOwnerType FileOwnerTypeId { get; private set; }
    public Guid? FileOwnerId { get; private set; }
    public string FileName { get; private set; }
    public string Name { get; private set; }
    public long Length { get; private set; }
    public string ContentType { get; private set; }
    public FileContent? FileContent { get; private set; }

    public File(FileOwnerType fileOwnerTypeId, Guid? fileOwnerId, string fileName, string name, long length, string contentType)
    {
        FileOwnerTypeId = fileOwnerTypeId;
        FileOwnerId = fileOwnerId;
        FileName = fileName;
        Name = name;
        Length = length;
        ContentType = contentType;
        Id = Guid.NewGuid();
    }
    public void SetFileContent(FileContent fileContent)
    {
        FileContent = fileContent;
    }

    public void SetFileOwnerId(Guid fileOwnerId)
    {
        FileOwnerId = fileOwnerId;
    }

}
