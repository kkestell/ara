#region

using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

#endregion

namespace Ara.Ast.Nodes.Expressions.Abstract;

public abstract record Expression(IParseNode Node) : AstNode(Node), ITyped
{
    public abstract Type Type { get; }
}
