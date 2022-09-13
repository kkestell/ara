using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Expressions.Atoms;
using Ara.Parsing;

namespace Ara.Ast.Nodes.Statements;

public class VariableDeclarationStatement : Statement
{
    public VariableDeclarationStatement(Node node, Identifier name, Type_ type, Expression expression) : base(node)
    {
        Name = name;
        Type = type;
        Expression = expression;
    }
    
    public Identifier Name { get; }
    
    public Type_ Type { get; }
    
    public Expression Expression { get; }
}
