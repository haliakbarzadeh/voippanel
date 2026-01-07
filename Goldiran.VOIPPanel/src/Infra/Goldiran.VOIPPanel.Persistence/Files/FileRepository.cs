using Voip.Framework.EFCore.Common;
using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories;
using Goldiran.VOIPPanel.Domain.AggregatesModel.LoginHistories.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files.Contracts;
using Goldiran.VOIPPanel.Persistence;
using File = Goldiran.VOIPPanel.Domain.AggregatesModel.Files.File;

namespace Goldiran.VOIPPanel.Persistence.Files;

public class FileRepository : BaseRepository<Guid, File>, IFileRepository
{
    public FileRepository(VOIPPanelContext context) : base(context)
    {
    }

}
