using System.Text.RegularExpressions;

namespace SaveApis.Core.Infrastructure;

public partial class DefaultRegexNames
{
    [GeneratedRegex(@"^[\w\.\-]+@[\w\-]+\.[a-zA-Z]{2,}$")]
    public static partial Regex Email();
}