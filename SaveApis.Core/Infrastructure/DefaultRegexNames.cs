using System.Text.RegularExpressions;

namespace SaveApis.Core.Infrastructure;

public partial class DefaultRegexNames
{
    [GeneratedRegex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
    public static partial Regex Email();
}