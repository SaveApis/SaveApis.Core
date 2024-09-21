using SaveApis.Core.Application.Models.ValueObject;

namespace SaveApis.Core.Application.Exceptions;

public class CacheObjectNotFoundException(CacheKey key) : Exception($"Cache object with key {key} not found.");