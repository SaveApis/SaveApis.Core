using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace SaveApis.Core.Application.DI;

public class SerilogModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();
        collection.AddSerilog(configuration =>
        {
            var logLevel = Environment.GetEnvironmentVariable("SAVEAPIS_LOG_LEVEL") ?? "Information";
            configuration = Enum.Parse<LogEventLevel>(logLevel) switch
            {
                LogEventLevel.Debug => configuration.MinimumLevel.Debug(),
                LogEventLevel.Verbose => configuration.MinimumLevel.Verbose(),
                LogEventLevel.Information => configuration.MinimumLevel.Information(),
                LogEventLevel.Warning => configuration.MinimumLevel.Warning(),
                LogEventLevel.Error => configuration.MinimumLevel.Error(),
                LogEventLevel.Fatal => configuration.MinimumLevel.Fatal(),
                _ => configuration.MinimumLevel.Information()
            };
            configuration
                .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                    theme: AnsiConsoleTheme.Code, applyThemeToRedirectedOutput: true)
                .WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Day,
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}");
        });

        builder.Populate(collection);
    }
}