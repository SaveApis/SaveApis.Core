using Vogen;

namespace Example.Web.Domains.EfCore.Domain.VO;

[ValueObject<string>(conversions: Conversions.EfCoreValueConverter | Conversions.NewtonsoftJson)]
public partial class Name
{
    public static readonly Name Empty = new Name(string.Empty);

    private static Validation Validate(string input)
    {
        return string.IsNullOrWhiteSpace(input)
            ? Validation.Invalid("Name cannot be empty!")
            : input.Length > 50
                ? Validation.Invalid("Name cannot be longer than 50 characters!")
                : Validation.Ok;
    }
    private static string NormalizeInput(string input)
    {
        return input;
    }
}
