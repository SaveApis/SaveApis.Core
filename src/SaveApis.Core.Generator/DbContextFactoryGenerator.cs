using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SaveApis.Core.Generator;

[Generator]
public class DbContextFactoryGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider.CreateSyntaxProvider((syntaxNode, _) => Filter(syntaxNode), (syntaxContext, _) => Transform(syntaxContext));

        context.RegisterSourceOutput(provider, Generate);
    }

    private static ClassDeclarationSyntax Transform(GeneratorSyntaxContext syntaxContext)
    {
        return syntaxContext.Node switch
        {
            ClassDeclarationSyntax classDeclaration => classDeclaration,
            _ => throw new InvalidOperationException(),
        };
    }

    private static bool Filter(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not ClassDeclarationSyntax classDeclaration)
        {
            return false;
        }

        if (!classDeclaration.Identifier.Text.EndsWith("DbContext", StringComparison.InvariantCultureIgnoreCase))
        {
            return false;
        }

        if (classDeclaration.BaseList is null)
        {
            return false;
        }

        var baseType = classDeclaration.BaseList.Types.FirstOrDefault()?.Type;
        if (baseType is null)
        {
            return false;
        }

        return baseType.ToString() == "BaseDbContext";
    }

    private static void Generate(SourceProductionContext productionContext, ClassDeclarationSyntax syntax)
    {
        var @namespace = syntax.FirstAncestorOrSelf<FileScopedNamespaceDeclarationSyntax>()!.Name.ToString();

        var code = $$"""
                     using Microsoft.Extensions.Configuration;
                     using SaveApis.Core.Common.Infrastructure.Persistence.Sql.Factories;

                     namespace {{@namespace}};

                     public class {{syntax.Identifier}}Factory(IConfiguration configuration) : BaseDesignTimeDbContextFactory<{{syntax.Identifier}}>(configuration)
                     {
                         public {{syntax.Identifier}}Factory() : this(new ConfigurationBuilder().AddInMemoryCollection().Build())
                         {
                         }
                     }
                     """;

        productionContext.AddSource($"{syntax.Identifier}Factory.g.cs", code);
    }
}
