using System.Reflection;
using SaveApis.Core.Common.Infrastructure.Helpers;

namespace SaveApis.Core.Common.Application.Helpers;

public class AssemblyHelper : IAssemblyHelper
{
    private ICollection<Assembly> AssemblyStorage { get; } = [];

    public AssemblyHelper()
    {
        RegisterAssembly(Assembly.GetExecutingAssembly());
    }

    public void RegisterAssembly(Assembly assembly)
    {
        if (AssemblyStorage.Contains(assembly))
        {
            return;
        }

        AssemblyStorage.Add(assembly);
    }

    public void RegisterAssemblies(params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            RegisterAssembly(assembly);
        }
    }

    public IEnumerable<Assembly> GetAssemblies()
    {
        return AssemblyStorage;
    }

    public IEnumerable<Type> GetTypes()
    {
        return AssemblyStorage.SelectMany(assembly => assembly.GetTypes());
    }

    public IEnumerable<Type> GetTypesByAttribute<TAttribute>() where TAttribute : Attribute
    {
        return GetTypes().Where(type => type.GetCustomAttribute<TAttribute>() is not null);
    }
}
