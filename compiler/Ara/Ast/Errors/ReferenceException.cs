using System.Text;
using Ara.Ast.Nodes;

namespace Ara.Ast.Errors;

public class ReferenceException : CompilerException
{
    readonly AstNode astNode;

    public ReferenceException(AstNode astNode) : base(astNode.Node)
    {
        this.astNode = astNode;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        var name = astNode switch
        {
            VariableReference r => r.Name.Value,
            Call c => c.Name.Value,
            _ => throw new Exception($"Unsupported node type {astNode.GetType()}")
        };

        sb.AppendLine("ReferenceException");
        sb.AppendLine(Location.ToString());
        sb.AppendLine(Location.Context);
        sb.AppendLine(Indented($"{name} is not defined"));
        
        return sb.ToString();
    }
}