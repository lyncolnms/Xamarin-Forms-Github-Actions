using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DefaultNamespace {
    public class Rules {
        [DiagnosticAnalyzer(LanguageNames.CSharp)]
        public class Rules : DiagnosticAnalyzer
        {
            public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
                CustomRule001
            );

            private static readonly DiagnosticDescriptor CustomRule001 = new DiagnosticDescriptor(
                "CustomRule001",
                "Use var instead of explicit type",
                "Consider using 'var' instead of an explicit type",
                "CustomRules",
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true
            );

            public override void Initialize(AnalysisContext context)
            {
                context.RegisterSyntaxNodeAction(AnalyzeVariableDeclaration, SyntaxKind.VariableDeclaration);
            }

            private static void AnalyzeVariableDeclaration(SyntaxNodeAnalysisContext context)
            {
                var variableDeclaration = (VariableDeclarationSyntax)context.Node;
                var type = variableDeclaration.Type;

                if (type.IsVar)
                {
                    return;
                }

                var diagnostic = Diagnostic.Create(CustomRule001, type.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}