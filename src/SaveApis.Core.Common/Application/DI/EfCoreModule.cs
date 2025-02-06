using System.Reflection;
using Autofac;
using Microsoft.EntityFrameworkCore.Design;
using SaveApis.Core.Common.Infrastructure.DI;
using SaveApis.Core.Common.Infrastructure.Persistence.Sql;

namespace SaveApis.Core.Common.Application.DI;

public class EfCoreModule(IEnumerable<Assembly> assemblies) : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(assemblies.ToArray())
            .Where(type => type.GetInterfaces().Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == typeof(IDesignTimeDbContextFactory<>)))
            .AsImplementedInterfaces()
            .As<IDesignTimeDbContextFactory<BaseDbContext>>();
    }
}
