#region

using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.Parsing.Abstract;

#endregion

namespace Ara.Ast.Nodes.Statements;

public record Return(IParseNode Node, Expression Expression) : Statement(Node)
{
    public override IEnumerable<AstNode> Children { get; } = new List<AstNode> { Expression };
}
