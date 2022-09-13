using Ara.Parsing;

namespace Ara.Ast.Nodes;

public class SourceFile : AstNode
{
    public SourceFile(Node node, ModuleDeclaration moduleDeclaration, IEnumerable<Definition> definitions) : base(node)
    {
        ModuleDeclaration = moduleDeclaration;
        Definitions = definitions;
    }
    
    public ModuleDeclaration ModuleDeclaration { get; }
    
    public IEnumerable<Definition> Definitions { get; }
}
