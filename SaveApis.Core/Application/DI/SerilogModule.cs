using System.Globalization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Core.Infrastructure.DI;
using Serilog;
using Serilog.Events;

namespace SaveApis.Core.Application.DI;

public class SerilogModule(IConfiguration configuration) : BaseModule(configuration)
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddSerilog(loggerConfiguration =>
        {
            loggerConfiguration.MinimumLevel.Verbose();
            loggerConfiguration.Enrich.FromLogContext();
            loggerConfiguration.WriteTo.Console(LogEventLevel.Debug, formatProvider: CultureInfo.InvariantCulture);
            loggerConfiguration.WriteTo.File("logs/log.txt", formatProvider: CultureInfo.InvariantCulture,
                rollingInterval: RollingInterval.Day);
        });

        builder.Populate(collection);
    }
}
