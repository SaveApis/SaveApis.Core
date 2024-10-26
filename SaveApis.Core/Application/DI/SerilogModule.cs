using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace SaveApis.Core.Application.DI;

public class SerilogModule(IConfiguration configuration) : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();
        collection.AddSerilog(config =>
        {
            var logLevel = configuration["SAVEAPIS_LOG_LEVEL"] ?? "Information";
            config = Enum.Parse<LogEventLevel>(logLevel) switch
            {
                LogEventLevel.Debug => config.MinimumLevel.Debug(),
                LogEventLevel.Verbose => config.MinimumLevel.Verbose(),
                LogEventLevel.Information => config.MinimumLevel.Information(),
                LogEventLevel.Warning => config.MinimumLevel.Warning(),
                LogEventLevel.Error => config.MinimumLevel.Error(),
                LogEventLevel.Fatal => config.MinimumLevel.Fatal(),
                _ => config.MinimumLevel.Information()
            };
            config
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