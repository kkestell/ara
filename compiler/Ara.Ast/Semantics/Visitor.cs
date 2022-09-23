using Ara.Ast.Nodes;

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
        foreach (var propertyInfo in node.GetType().GetProperties())
        {
            if (propertyInfo.Name.StartsWith("_"))
                continue;
            
            if (typeof(IEnumerable<AstNode>).IsAssignableFrom(propertyInfo.PropertyType))
            {
                foreach (var n in (IEnumerable<AstNode?>)propertyInfo.GetValue(node)!)
                {
                    if (n is not null)
                        Visit(n);
                }
            }
            else if (typeof(AstNode).IsAssignableFrom(propertyInfo.PropertyType))
            {
                var n = (AstNode?)propertyInfo.GetValue(node);
                
                if (n is not null)
                    Visit(n);
            }
        }
        
        VisitNode(node);
    }
    
    protected abstract void VisitNode(AstNode node);
}