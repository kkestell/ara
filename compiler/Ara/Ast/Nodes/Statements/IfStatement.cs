using Ara.Ast.Nodes.Expressions;
using Ara.Parsing;

namespace Ara.Ast.Nodes.Statements;

public class IfStatement : Statement
{
    public IfStatement(Node node, Expression predicate, Block then) : base(node)
    {
        Predicate = predicate;
        Then = then;
    }
    
    public Expression Predicate { get; }
    
    public Block Then { get; }
}
