using Ara.Ast.Errors;
using Ara.Ast.Nodes;
using Ara.Ast.Semantics.Types;

namespace Ara.Ast.Semantics;

public class TypeChecker : Visitor
{
    readonly Dictionary<string, FunctionDefinition> functionCache = new ();
    
    public TypeChecker(SourceFile sourceFile) : base(sourceFile)
    {
        foreach (var d in sourceFile.Definitions.Nodes)
        {
            if (d is not FunctionDefinition f)
                continue;

            functionCache.Add(f.Name, f);
        }
    }
    
    protected override void VisitNode(AstNode node)
    {
        switch (node)
        {
            case Call c:
                CheckCall(c);
                break;

            case If i:
                CheckIf(i);
                break;
            
            case Return r:
                CheckReturn(r);
                break;
        }
    }

    void CheckCall(Call c)
    {
        if (!functionCache.ContainsKey(c.Name))
            throw new SemanticException(c, "No such function.");

        var func = functionCache[c.Name];
        
        if (c.Arguments.Nodes.Count != func.Parameters.Nodes.Count())
            throw new SemanticException(c, "Wrong number of arguments.");

        foreach (var arg in c.Arguments.Nodes)
        {
            var p = func.Parameters.Nodes.SingleOrDefault(x => x.Name == arg.Name);
            
            if (p is null)
                throw new SemanticException(arg, $"Function {func.Name} has no such argument {arg.Name}");
            
            if (!p.Type.Equals(arg.Expression.Type))
                throw new SemanticException(arg, $"Argument type {arg.Expression.Type} doesn't match parameter type {p.Type}");
        }
    }

    static void CheckReturn(Return r)
    {
        var func = r.NearestAncestor<FunctionDefinition>();
        
        if (r.Expression.Type is null)
            throw new SemanticException(r.Expression, "Expression type could not be inferred.");
        
        if (!r.Expression.Type.Equals(func.Type))
            throw new ReturnTypeException(r);
    }

    static void CheckIf(If i)
    {
        if (i.Predicate.Type is null)
            throw new SemanticException(i.Predicate, "Expression type could not be inferred.");
                
        if (!i.Predicate.Type.Equals(new BooleanType()))
            throw new IfPredicateTypeException(i);
    }
}