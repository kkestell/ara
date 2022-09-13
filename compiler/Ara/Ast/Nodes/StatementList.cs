using Ara.Ast.Nodes.Statements;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public class StatementList : AstNode
{
    public StatementList(Node node, IEnumerable<Statement> statements) : base(node)
    {
        Statements = statements;
    }
    
    public IEnumerable<Statement> Statements { get; }
}
