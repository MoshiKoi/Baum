using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading;

namespace NotifyGenerator
{
    public record struct PropertyInfo(string NamespaceName, string ClassName, string PropertyName, string PropertyType);

    [Generator]
    public class NotifyGenerator : IIncrementalGenerator
    {
        public const string NotifyAttributeSource = @"
namespace NotifyGenerator
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class NotifyAttribute : System.Attribute
    {
    }
}";

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(static ctx => ctx.AddSource(
                "NotifyAttribute.g.cs",
                NotifyAttributeSource));

            var propertiesProvider = context.SyntaxProvider
                .ForAttributeWithMetadataName(
                    "NotifyGenerator.NotifyAttribute",
                    predicate: static (s, _) => true,
                    transform: static (ctx, token) => Transform(ctx, token))
                .Where(x => x != null);

            context.RegisterSourceOutput(propertiesProvider, Execute);
        }

        static PropertyInfo? Transform(GeneratorAttributeSyntaxContext ctx, CancellationToken token)
        {
            var symbol = ctx.TargetSymbol;
            var className = symbol.ContainingType.Name;
            var namespaceName = symbol.ContainingNamespace.ToDisplayString(
                SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted));
            var propertyType = ((PropertyDeclarationSyntax)ctx.TargetNode).Type.ToFullString();
            return new PropertyInfo(
                    ClassName: className,
                    NamespaceName: namespaceName,
                    PropertyName: symbol.Name,
                    PropertyType: propertyType);
        }

        static void Execute(SourceProductionContext context, PropertyInfo? prop)
        {
            if (prop == null) { return; }

            PropertyInfo value = prop.Value;

            context.AddSource(
                $"NotifyGenerator.{value.NamespaceName}.{value.ClassName}.{value.PropertyName}.g.cs",
                @$"namespace {value.NamespaceName} {{
    partial class {value.ClassName} {{
        public partial {value.PropertyType} {value.PropertyName} {{
            get => field;
            set => SetProperty(ref field, value);
        }}
    }}
}}");
        }
    }
}
