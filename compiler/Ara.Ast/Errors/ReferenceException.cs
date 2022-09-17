using Ara.Ast.Nodes;

namespace Ara.Ast.Errors;

public class ReferenceException : CompilerException
{
    public ReferenceException(AstNode astNode) : base(astNode.Node)
    {
        var name = astNode switch
        {
            VariableReference r => r.Name.Value,
            Call c => c.Name.Value,
            _ => throw new Exception($"Unsupported node type {astNode.GetType()}")
        };
        Description = $"The name {name} is not defined.";
    }
}