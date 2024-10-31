namespace SaveApis.Core.Domain.Settings;

public record MongoSettings(
    bool Srv,
    string Host,
    uint Port,
    string UserName,
    string Password,
    string AuthSource)
{
    public static implicit operator string(MongoSettings settings)
    {
        return settings.ToString();
    }

    public override string ToString()
    {
        return Srv
            ? $"mongodb+srv://{UserName}:{Password}@{Host}/?retryWrites=false&authSource={AuthSource}"
            : $"mongodb://{UserName}:{Password}@{Host}:{Port}/?retryWrites=false&authSource={AuthSource}";
    }
}