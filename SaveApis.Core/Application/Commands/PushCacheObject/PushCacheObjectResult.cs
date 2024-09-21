using SaveApis.Core.Application.Models.Base;
using SaveApis.Core.Application.Models.Dtos;

namespace SaveApis.Core.Application.Commands.PushCacheObject;

public class PushCacheObjectResult : BaseResult<CacheObjectDto>
{
    public PushCacheObjectResult(CacheObjectDto result) : base(result)
    {
    }

    public PushCacheObjectResult(params Exception[] exceptions) : base(exceptions)
    {
    }
}