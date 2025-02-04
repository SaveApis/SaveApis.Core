using Example.Web.Domains.EfCore.Domain.VO;

namespace Example.Web.Domains.EfCore.Domain.Entities;

public class TestEntity
{
    private TestEntity(Id id, DateTime createdAt, DateTime? updatedAt, Name firstName, Name lastName)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        FirstName = firstName;
        LastName = lastName;
    }

    public Id Id { get; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }

    public Name FirstName { get; private set; }
    public Name LastName { get; private set; }

    public TestEntity WithFirstName(Name firstName)
    {
        FirstName = firstName;
        UpdatedAt = DateTime.UtcNow;

        return this;
    }
    public TestEntity WithLastName(Name lastName)
    {
        LastName = lastName;
        UpdatedAt = DateTime.UtcNow;

        return this;
    }

    public static TestEntity Create(Name firstName, Name lastName)
    {
        return new TestEntity(Id.From(Guid.NewGuid()), DateTime.UtcNow, null, firstName, lastName);
    }
}
