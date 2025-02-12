using Vogen;

namespace SaveApis.Core.Common.Domains.Common.Domain.VO;

[ValueObject<string>(conversions: Conversions.NewtonsoftJson | Conversions.EfCoreValueConverter)]
public partial class Name
{
    public static readonly Name Empty = new Name(string.Empty);

    private static Validation Validate(string input)
    {
        return string.IsNullOrWhiteSpace(input)
            ? Validation.Invalid("Name cannot be empty")
            : input.Length > 100
                ? Validation.Invalid("Name cannot be longer than 100 characters")
                : Validation.Ok;
    }

    private static string NormalizeInput(string input)
    {
        return input.Trim();
    }
}
