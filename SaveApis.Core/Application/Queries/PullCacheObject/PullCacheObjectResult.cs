using SaveApis.Core.Application.Models.Base;
using SaveApis.Core.Application.Models.Dtos;

namespace SaveApis.Core.Application.Queries.PullCacheObject;

public class PullCacheObjectResult : BaseResult<CacheObjectDto>
{
    public PullCacheObjectResult(CacheObjectDto result) : base(result)
    {
    }

    public PullCacheObjectResult(params Exception[] exceptions) : base(exceptions)
    {
    }
}