namespace SaveApis.Core.Example.Application.Backend.GraphQL;

public class ExampleMutation
{
    public string CreateExample()
    {
        return "Example";
    }

    public string UpdateExample(int id)
    {
        return $"Example {id}";
    }
}