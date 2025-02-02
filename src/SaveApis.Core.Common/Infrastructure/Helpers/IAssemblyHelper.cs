using System.Reflection;

namespace SaveApis.Core.Common.Infrastructure.Helpers;

/// <summary>
/// Interface for AssemblyHelper
/// </summary>
public interface IAssemblyHelper
{
    /// <summary>
    /// Method to register assembly to the shared assembly storage
    /// </summary>
    /// <param name="assembly">Instance of the assembly</param>
    void RegisterAssembly(Assembly assembly);

    /// <summary>
    /// Method to register assemblies to the shared assembly storage
    /// </summary>
    /// <param name="assemblies">List of assemblies</param>
    void RegisterAssemblies(params Assembly[] assemblies);

    /// <summary>
    /// Method to get all the assemblies from the shared assembly storage
    /// </summary>
    /// <returns>List of assemblies</returns>
    IEnumerable<Assembly> GetAssemblies();

    /// <summary>
    /// Method to get all the types from the shared assembly storage
    /// </summary>
    /// <returns>List of types</returns>
    IEnumerable<Type> GetTypes();

    /// <summary>
    /// Method to get all the types from the shared assembly storage which has the specified attribute
    /// </summary>
    /// <typeparam name="TAttribute">Type of the attribute</typeparam>
    /// <returns>List of types</returns>
    IEnumerable<Type> GetTypesByAttribute<TAttribute>() where TAttribute : Attribute;
}
