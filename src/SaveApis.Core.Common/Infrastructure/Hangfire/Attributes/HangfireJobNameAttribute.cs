using Hangfire;
using Hangfire.Common;
using Hangfire.Dashboard;

namespace SaveApis.Core.Common.Infrastructure.Hangfire.Attributes;

public sealed class HangfireJobNameAttribute(string displayName) : JobDisplayNameAttribute(displayName)
{
    public override string Format(DashboardContext context, Job job)
    {
        var formatted = base.Format(context, job);

        var prefix = GeneratePrefix("Integrations", job);
        prefix ??= GeneratePrefix("Domains", job);
        prefix ??= "Core";

        return $"[{prefix}] {formatted}";
    }

    private static string? GeneratePrefix(string part, Job job)
    {
        var namespaceParts = job.Type.Namespace?.Split('.') ?? [];
        var index = Array.IndexOf(namespaceParts, part);

        if (index == -1 || index == namespaceParts.Length - 1)
        {
            return null;
        }

        return namespaceParts[index + 1];
    }
}
