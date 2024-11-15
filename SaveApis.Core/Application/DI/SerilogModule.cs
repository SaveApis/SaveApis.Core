using Autofac;
using Autofac.Extensions.DependencyInjection;
using Elastic.CommonSchema.Serilog;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Core.Infrastructure.DI;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace SaveApis.Core.Application.DI;

public class SerilogModule(IConfiguration configuration) : BaseModule(configuration)
{
    protected override void Register(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();
        collection.AddSerilog(config =>
        {
            var elasticName = Configuration["ELASTICSEARCH_NAME"] ?? "saveapis";
            var elasticUri = Configuration["ELASTICSEARCH_URL"] ?? "http://localhost:9200";

            var options =
                new ElasticsearchSinkOptions(new DistributedTransport(new TransportConfiguration(new Uri(elasticUri))))
                {
                    DataStream = new DataStreamName($"saveapis-{elasticName}"),
                    BootstrapMethod = BootstrapMethod.Failure,
                    TextFormatting = new EcsTextFormatterConfiguration(),
                    MinimumLevel = LogEventLevel.Verbose
                };


            config
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Error)
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                    theme: AnsiConsoleTheme.Code, applyThemeToRedirectedOutput: true,
                    restrictedToMinimumLevel: LogEventLevel.Information)
                .WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Day,
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.Elasticsearch(options);
        });

        builder.Populate(collection);
    }
}