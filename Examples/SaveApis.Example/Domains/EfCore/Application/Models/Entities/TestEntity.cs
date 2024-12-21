using SaveApis.Example.Domains.EfCore.Application.Models.VO;

namespace SaveApis.Example.Domains.EfCore.Application.Models.Entities;

public class TestEntity
{
    private TestEntity(TestId id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public TestId Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    public static TestEntity Create(string name, string description)
    {
        return new TestEntity(TestId.From(Guid.NewGuid()), name, description);
    }
}
