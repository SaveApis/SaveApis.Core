using Riok.Mapperly.Abstractions;
using SaveApis.Example.Domains.EfCore.Application.Models.Dto;
using SaveApis.Example.Domains.EfCore.Application.Models.Entities;
using SaveApis.Example.Domains.EfCore.Application.Models.VO;

namespace SaveApis.Example.Domains.EfCore.Application.Mapper;

[Mapper]
public partial class TestEntityMapper
{
    [MapProperty(nameof(TestEntity.Id), nameof(TestGetDto.Id), Use = nameof(IdToGuid))]
    public partial TestGetDto EntityToDto(TestEntity entity);

    private static Guid IdToGuid(TestId id)
    {
        return id.Value;
    }
}
