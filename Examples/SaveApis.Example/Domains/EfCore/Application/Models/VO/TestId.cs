using Vogen;

namespace SaveApis.Example.Domains.EfCore.Application.Models.VO;

[ValueObject<Guid>(Conversions.EfCoreValueConverter)]
public partial record TestId
{
    private static Validation Validate(Guid input)
    {
        return input == Guid.Empty ? Validation.Invalid("Guid should not be empty!") : Validation.Ok;
    }

    private static Guid NormalizeInput(Guid input)
    {
        return input;
    }
}
