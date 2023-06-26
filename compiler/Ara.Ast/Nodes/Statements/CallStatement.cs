#region

using Ara.Ast.Errors;
using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

#endregion

namespace Ara.Ast.Nodes.Statements;

public record CallStatement(IParseNode Node, string Name, NodeList<Argument> Arguments) : Statement(Node)
{
    public override IEnumerable<AstNode> Children { get; } = new List<AstNode> { Arguments };
}