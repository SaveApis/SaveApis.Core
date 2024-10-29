namespace SaveApis.Core.Domain.Models.ValueObject;

public record MongoDatabase(string Value)
{
    public static MongoDatabase Create(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        return new MongoDatabase(value);
    }

    public static implicit operator string(MongoDatabase database)
    {
        return database.ToString();
    }

    public override string ToString()
    {
        return Value;
    }
}