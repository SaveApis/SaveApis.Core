using SaveApis.Core.Application.Models.ValueObject;

namespace SaveApis.Core.Application.Models.Dtos;

public record CacheObjectDto(CacheKey Key, object Value);