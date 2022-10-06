using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Parsing.Abstract;

namespace Ara.Ast.Nodes;

public record Argument(IParseNode Node, string Name, Expression Expression) : AstNode(Node)
{ 
    public override IEnumerable<AstNode> Children { get; } = new List<AstNode> { Expression };
}
