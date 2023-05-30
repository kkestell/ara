#region

using Ara.Ast.Nodes;
using Ara.Ast.Nodes.Abstract;

#endregion

namespace Ara.Ast.Semantics;

public abstract class Visitor
{
    private readonly SourceFile _sourceFile;
    
    protected Visitor(SourceFile sourceFile)
    {
        _sourceFile = sourceFile;
    }

    public void Visit()
    {
        Visit(_sourceFile);
    }

    private void Visit(AstNode node)
    {
        foreach (var child in node.Children)
        {
            Visit(child);
        }
        
        VisitNode(node);
    }
    
    protected abstract void VisitNode(AstNode node);
}
