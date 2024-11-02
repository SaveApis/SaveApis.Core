namespace SaveApis.Core.Example.Domain.Models.Sql;

public class SqlEntity
{
    private SqlEntity(Guid id, DateTime createdAt, DateTime updatedAt, string firstName, string lastName, int age)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        FirstName = firstName;
        LastName = lastName;
        Age = age;
    }

    private SqlEntity()
    {
    }

    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public int Age { get; private set; }

    public static SqlEntity Create(string firstName, string lastName, int age)
    {
        return new SqlEntity(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, firstName, lastName, age);
    }
}