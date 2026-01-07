using Voip.Framework.EFCore.Common;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Tokens;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Tokens.Contracts;
using Goldiran.VOIPPanel.Persistence;

namespace Goldiran.VOIPPanel.Persistence.Tokens;

public class TokenRepository : BaseRepository<long, Token>, ITokenRepository
{
    public TokenRepository(VOIPPanelContext context) : base(context)
    {
    }

}
