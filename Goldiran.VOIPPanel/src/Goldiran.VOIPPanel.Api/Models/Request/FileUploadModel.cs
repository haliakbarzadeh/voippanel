using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;

namespace Goldiran.VOIPPanel.Api.Models;

public class FileUploadModel
{
    public IFormFile File { get; set; }
    public FileOwnerType FileOwnerTypeId { get; set; }
    public Guid? FileOwnerId { get; set; }
}
