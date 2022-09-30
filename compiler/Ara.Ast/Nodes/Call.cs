using Ara.Parsing;
using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.Ast.Nodes;

public record Call(Node Node, string Name, NodeList<Argument> Arguments) : Expression(Node)
{
    public override List<AstNode> Children { get; } = new List<AstNode> { Arguments };
    
    public override Type Type { get; set; }
}
