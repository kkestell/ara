using Ara.Ast.Nodes.Statements;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public class Block : AstNode
{
    public Block(Node node, IEnumerable<Statement> statements) : base(node)
    {
        Statements = statements;
    }
    
    public IEnumerable<Statement> Statements { get; }
}
