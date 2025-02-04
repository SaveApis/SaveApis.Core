using System.Globalization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Core.Common.Infrastructure.DI;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace SaveApis.Core.Common.Application.DI;

public sealed class SerilogModule : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddSerilog((_, configuration) =>
        {
            configuration.MinimumLevel.Verbose();
            configuration.Enrich.FromLogContext();
            configuration.Enrich.WithCorrelationId();

            configuration.WriteTo.Console(LogEventLevel.Information, formatProvider: CultureInfo.InvariantCulture, theme: AnsiConsoleTheme.Code);
            configuration.WriteTo.File("logs/log.txt", LogEventLevel.Information, formatProvider: CultureInfo.InvariantCulture, buffered: true, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7);
        });

        builder.Populate(collection);
    }
}
