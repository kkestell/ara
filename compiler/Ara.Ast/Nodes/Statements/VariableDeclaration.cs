#region

using Ara.Ast.Errors;
using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.Parsing.Abstract;
using Type = Ara.Ast.Types.Abstract.Type;

#endregion

namespace Ara.Ast.Nodes.Statements;

public record VariableDeclaration(IParseNode Node, string Name, TypeRef TypeRef, Expression? Expression) : Statement(Node), ITyped
{
    private List<AstNode>? _children;

    public override IEnumerable<AstNode> Children
    {
        get
        {
            if (_children is not null)
                return _children;

            _children = new List<AstNode> { TypeRef };

            if (Expression is not null)
                _children.Add(Expression);

            return _children;
        }
    }
    
    public Type Type => TypeRef.ToType();
}
