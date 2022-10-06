using Ara.Ast.Errors;
using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Ast.Nodes.Statements;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Nodes.Expressions;

public record VariableReference(IParseNode Node, string Name) : Expression(Node)
{
    public override IEnumerable<AstNode> Children { get; } = new List<AstNode>();

    public override Type Type
    {
        get
        {
            var b = NearestAncestor<Block>();
            while (true)
            {
                if (b is null)
                    throw new ReferenceException(this);

                if (b.Scope.ContainsKey(Name))
                {
                    return b.Scope[Name].Type;
                }

                b = b.NearestAncestorOrDefault<Block>();
            }
        }
    }
}
