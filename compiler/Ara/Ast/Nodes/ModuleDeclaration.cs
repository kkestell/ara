using Ara.Ast.Nodes.Expressions.Atoms;
using Ara.Parsing;

namespace Ara.Ast.Nodes;

public class ModuleDeclaration : AstNode
{
    public ModuleDeclaration(Node node, Identifier name) : base(node)
    {
        Name = name;
    }
    
    public Identifier Name { get; }
}
