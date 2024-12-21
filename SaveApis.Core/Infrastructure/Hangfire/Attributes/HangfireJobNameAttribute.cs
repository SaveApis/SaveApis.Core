using Hangfire;
using Hangfire.Common;
using Hangfire.Dashboard;

namespace SaveApis.Core.Infrastructure.Hangfire.Attributes;

public class HangfireJobNameAttribute(string displayName) : JobDisplayNameAttribute(displayName)
{
    public override string Format(DashboardContext context, Job job)
    {
        var format = base.Format(context, job);

        var prefix = GeneratePrefix(job);

        return $"[{prefix}] {format}";
    }

    private static string GeneratePrefix(Job job)
    {
        // If job namespace contains `Domains` then it#s a domain job and return the domain name which is the part AFTER `Domains.`
        var namespaceParts = job.Type.Namespace?.Split('.') ?? [];
        var domainIndex = Array.IndexOf(namespaceParts, "Domains");
        if (domainIndex >= 0 && domainIndex < namespaceParts.Length - 1)
        {
            return namespaceParts[domainIndex + 1];
        }

        // If job namespace contains `Integrations` then it's an integration job and return the integration name which is the part AFTER `Integrations.`
        var integrationIndex = Array.IndexOf(namespaceParts, "Integrations");
        if (integrationIndex >= 0 && integrationIndex < namespaceParts.Length - 1)
        {
            return namespaceParts[integrationIndex + 1];
        }

        return "Core";
    }
}
