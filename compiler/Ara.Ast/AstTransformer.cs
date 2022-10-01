using Ara.Ast.Errors;
using Ara.Ast.Nodes;
using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Expressions.Values;
using Ara.Parsing;

namespace Ara.Ast;

public static class AstTransformer
{    
    public static SourceFile Transform(Tree parseTree)
    {
        return (SourceFile)Visit(parseTree.Root);
    }

    static AstNode Visit(Node node)
    {
        var children = node.NamedChildren.Select(Visit).ToList();

        AstNode astNode = node.Type switch
        {
            "argument"                       => Argument(node, children),
            "argument_list"                  => ArgumentList(node, children),
            "assignment_statement"           => Assignment(node, children),
            "array_assignment_statement"     => ArrayAssignment(node, children),
            "array_index"                    => ArrayIndex(node, children),
            "binary_expression"              => BinaryExpression(node, children),
            "block"                          => Block(node, children),
            "bool"                           => Bool(node),
            "function_definition_list"       => FunctionDefinitionList(node, children),
            "for_statement"                  => For(node, children),
            "float"                          => Float(node),
            "function_call_expression"       => Call(node, children),
            "function_definition"            => FunctionDefinition(node, children),
            "identifier"                     => Identifier(node),
            "if_statement"                   => If(node, children),
            "if_else_statement"              => IfElse(node, children),
            "integer"                        => Integer(node),
            "parameter"                      => Parameter(node, children),
            "parameter_list"                 => ParameterList(node, children),
            "return_statement"               => Return(node, children),
            "source_file"                    => SourceFile(node, children),
            "statement_list"                 => StatementList(node, children),
            "single_value_type"              => SingleValueType(node, children),
            "array_type"                     => ArrayType(node, children),
            "unary_expression"               => UnaryExpression(node, children),
            "variable_declaration_statement" => VariableDeclaration(node, children),
            "variable_reference"             => VariableReference(node, children),
            
            _ => throw new SyntaxException(node)
        };

        foreach (var child in children)
        {
            child.Parent = astNode;
        }

        return astNode;
    }

    static Argument Argument(Node n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value, (Expression)c[1]);

    static NodeList<Argument> ArgumentList(Node n, IEnumerable<AstNode> c) =>
        new(n, c.Select(x => (Argument)x).ToList());

    static Assignment Assignment(Node n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value, (Expression)c[1]);

    static ArrayAssignment ArrayAssignment(Node n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value, (Expression)c[1], (Expression)c[2]);
    
    static ArrayIndex ArrayIndex(Node n, IReadOnlyList<AstNode> c) =>
        new(n, (VariableReference)c[0], (Expression)c[1]);

    static BinaryExpression BinaryExpression(Node n, IReadOnlyList<AstNode> c)
    {
        var op = n.ChildByFieldName("op")!.Span.ToString() switch
        {
            "*"  => BinaryOperator.Multiply,
            "/"  => BinaryOperator.Divide,
            "+"  => BinaryOperator.Add,
            "-"  => BinaryOperator.Subtract,
            "==" => BinaryOperator.Equality,
            "!=" => BinaryOperator.Inequality,
            
            _ => throw new NotImplementedException("Binary operator not implemented")
        };
        
        return new BinaryExpression(n, (Expression)c[0], (Expression)c[1], op);
    }

    static Block Block(Node n, IReadOnlyList<AstNode> c) =>
        new(n, (NodeList<Statement>)c[0]);
    
    static NodeList<FunctionDefinition> FunctionDefinitionList(Node n, IEnumerable<AstNode> c) =>
        new(n, c.Select(x => (FunctionDefinition)x).ToList());

    static For For(Node n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value, (Expression)c[1], (Expression)c[2], (Block)c[3]);
    
    static Call Call(Node n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value, (NodeList<Argument>)c[1]);

    static FunctionDefinition FunctionDefinition(Node n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value, (NodeList<Parameter>)c[1], (TypeRef)c[2], (Block)c[3]);

    static Identifier Identifier(Node n) =>
        new(n, n.Span.ToString());

    static If If(Node n, IReadOnlyList<AstNode> c) =>
        new(n, (Expression)c[0], (Block)c[1]);
    
    static IfElse IfElse(Node n, IReadOnlyList<AstNode> c) =>
        new(n, (Expression)c[0], (Block)c[1], (Block)c[2]);

    static BooleanValue Bool(Node n) => 
        new(n, bool.Parse(n.Span.ToString()));

    static IntegerValue Integer(Node n) => 
        new(n, int.Parse(n.Span.ToString()));

    static FloatValue Float(Node n) => 
        new(n, float.Parse(n.Span.ToString()));

    static Parameter Parameter(Node n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value, (TypeRef)c[1]);

    static NodeList<Parameter> ParameterList(Node n, IEnumerable<AstNode> c) =>
        new(n, c.Select(x => (Parameter)x).ToList());

    static Return Return(Node n, IReadOnlyList<AstNode> c) =>
        new(n, (Expression)c[0]);

    static SourceFile SourceFile(Node n, IReadOnlyList<AstNode> c) =>
        new(n, (NodeList<FunctionDefinition>)c[0]);

    static NodeList<Statement> StatementList(Node n, IEnumerable<AstNode> c) =>
        new(n, c.Select(x => (Statement)x).ToList());

    static SingleValueTypeRef SingleValueType(Node n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value);
    
    static ArrayTypeRef ArrayType(Node n, IReadOnlyList<AstNode> c) =>
        new(n, (TypeRef)c[0], (IntegerValue)c[1]);

    static UnaryExpression UnaryExpression(Node n, IReadOnlyList<AstNode> c)
    {
        var op = n.ChildByFieldName("op")!.Span.ToString() switch
        {
            "-"  => UnaryOperator.Negate,
            "!"  => UnaryOperator.Not,
            
            _ => throw new NotImplementedException("Unary operator not implemented")
        };

        return new UnaryExpression(n, (Expression)c[0], op);
    }

    static VariableDeclaration VariableDeclaration(Node n, IReadOnlyList<AstNode> c)
    {
        var name = ((Identifier)c[0]).Value;
        
        switch (c.Count)
        {
            case 3:
            {
                var type = (TypeRef)c[1];
                var value = (Expression?)c[2];
            
                return new VariableDeclaration(n, name, type, value);
            }
            case 2 when c[1] is TypeRef type:
                return new VariableDeclaration(n, name, type, null);
            case 2 when c[1] is Expression value:
                return new VariableDeclaration(n, name, null, value);
            default:
                throw new NotSupportedException();
        }
    }

    static VariableReference VariableReference(Node n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value);
}
