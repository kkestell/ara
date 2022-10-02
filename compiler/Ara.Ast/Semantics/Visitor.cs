using Ara.Ast.Nodes;
using Ara.Ast.Nodes.Abstract;

namespace Ara.Ast.Semantics;

public abstract class Visitor
{
    readonly SourceFile sourceFile;
    
    protected Visitor(SourceFile sourceFile)
    {
        this.sourceFile = sourceFile;
    }

    public void Visit()
    {
        Visit(sourceFile);
    }

    void Visit(AstNode node)
    {
        foreach (var child in node.Children)
        {
            Visit(child);
        }
        
        VisitNode(node);
    }
    
    protected abstract void VisitNode(AstNode node);
}
