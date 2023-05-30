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

        foreach (var f in sourceFile.FunctionDefinitions.Nodes)
        {
            _functionDefinitions.Add(f.Name, f);
        }
    }
    
    protected override void VisitNode(AstNode node)
    {
        switch (node)
        {
            case Call c:
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
        }
    }

    private void Call(Call c)
    {
        if (!_functionDefinitions.ContainsKey(c.Name) && !_externalFunctionDeclarations.ContainsKey(c.Name))
            throw new SemanticException(c, "No such function.");

        var parameters = _functionDefinitions.TryGetValue(c.Name, out var definition) ? definition.Parameters : _externalFunctionDeclarations[c.Name].Parameters;

        if (c.Arguments.Nodes.Count != parameters.Nodes.Count)
            throw new SemanticException(c, "Wrong number of arguments.");

        foreach (var arg in c.Arguments.Nodes)
        {
            var p = parameters.Nodes.SingleOrDefault(x => x.Name == arg.Name);
            
            if (p is null)
                throw new SemanticException(arg, $"Function {c.Name} has no such argument {arg.Name}");
            
            if (!p.Type.Equals(arg.Expression.Type))
                throw new SemanticException(arg, $"Argument type {arg.Expression.Type} doesn't match parameter type {p.Type}");
        }
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
}