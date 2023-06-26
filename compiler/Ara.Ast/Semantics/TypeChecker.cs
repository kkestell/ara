#region

using Ara.Ast.Errors;
using Ara.Ast.Nodes;
using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Statements;
using Ara.Ast.Types;

#endregion

namespace Ara.Ast.Semantics;

public class TypeChecker : Visitor
{
    private readonly Dictionary<string, ExternalFunctionDeclaration> _externalFunctionDeclarations = new ();
    private readonly Dictionary<string, FunctionDefinition> _functionDefinitions = new ();
    
    public TypeChecker(SourceFile sourceFile) : base(sourceFile)
    {
        if (sourceFile.ExternalFunctionDeclarations is not null)
        {
            foreach (var f in sourceFile.ExternalFunctionDeclarations.Nodes)
            {
                _externalFunctionDeclarations.Add(f.Name, f);
            }
        }

        foreach (var d in sourceFile.Definitions.Nodes)
        {
            switch (d)
            {
                case FunctionDefinition f:
                    _functionDefinitions.Add(f.Name, f);
                    break;
                case StructDefinition s:
                    // TODO
                    break;
            }
        }
    }
    
    protected override void VisitNode(AstNode node)
    {
        switch (node)
        {
            case Assignment a:
                Assignment(a);
                break;
            
            case CallExpression c:
                Call(c);
                break;

            case If i:
                If(i);
                break;
            
            case IfElse i:
                IfElse(i);
                break;
            
            case Return r:
                Return(r);
                break;
            
            case VariableDeclaration v:
                VariableDeclaration(v);
                break;
        }
    }

    private void Call(CallExpression c)
    {
        if (!_functionDefinitions.ContainsKey(c.Name) && !_externalFunctionDeclarations.ContainsKey(c.Name))
            throw new SemanticException(c, "No such function.");

        var parameters = _functionDefinitions.TryGetValue(c.Name, out var definition) ? definition.Parameters : _externalFunctionDeclarations[c.Name].Parameters;

        if (c.Arguments.Nodes.Count != parameters.Nodes.Count)
            throw new SemanticException(c, "Wrong number of arguments.");

        for (var i = 0; i < c.Arguments.Nodes.Count; i++)
        {
            var a = c.Arguments.Nodes[i];
            var p = parameters.Nodes[i];
            
            if (!p.Type.Equals(a.Expression.Type))
                throw new SemanticException(a, $"Argument type {a.Expression.Type} doesn't match parameter type {p.Type}");
        }
    }
    
    private static void Assignment(Assignment a)
    {
        var blk = a.NearestAncestor<Block>();
        var v = blk.FindDeclaration(a.Name);
 
        if (!v.Type.Equals(a.Expression.Type))
            throw new SemanticException(a, $"Variable type {v.Type} doesn't match expression type {a.Expression.Type}");
    }

    private static void Return(Return r)
    {
        var func = r.NearestAncestor<FunctionDefinition>();

        if (!r.Expression.Type.Equals(func.Type))
            throw new ReturnTypeException(r);
    }

    private static void If(If i)
    {
        if (!i.Predicate.Type.Equals(new BooleanType()))
            throw new PredicateTypeException(i.Predicate);
    }

    private static void IfElse(IfElse i)
    {
        if (!i.Predicate.Type.Equals(new BooleanType()))
            throw new PredicateTypeException(i.Predicate);
    }
    
    private static void VariableDeclaration(VariableDeclaration d)
    {
        if (d.Expression is null)
            return;
        
        if (!d.Type.Equals(d.Expression.Type))
            throw new VariableTypeException(d);
    }
}