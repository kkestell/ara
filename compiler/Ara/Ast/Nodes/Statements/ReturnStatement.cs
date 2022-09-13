using Ara.Ast.Nodes.Expressions;
using Ara.Parsing;

namespace Ara.Ast.Nodes.Statements;

public class ReturnStatement : Statement
{
    public ReturnStatement(Node node, Expression expression) : base(node)
    {
        Expression = expression;
    }
    
    public Expression Expression { get; }
}
