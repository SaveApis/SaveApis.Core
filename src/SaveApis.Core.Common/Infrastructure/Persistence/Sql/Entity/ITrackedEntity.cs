
using SaveApis.Core.Common.Domains.Common.Domain.VO;

namespace SaveApis.Core.Common.Infrastructure.Persistence.Sql.Entity;

public interface ITrackedEntity
{
    Id Id { get; }

    ICollection<Tuple<string, string?, string?>> GetChanges();
}
