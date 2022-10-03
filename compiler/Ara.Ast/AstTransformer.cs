using Ara.Ast.Errors;
using Ara.Ast.Nodes;
using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Ast.Nodes.Expressions.Values;
using Ara.Ast.Nodes.Statements;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.Parsing;

namespace Ara.Ast;

public static class AstTransformer
{    
    public static SourceFile Transform(Tree parseTree)
    {
        return (SourceFile)Visit(parseTree.Root);
    }

    static AstNode Visit(ParseNode parseNode)
    {
        var children = parseNode.NamedChildren.Select(Visit).ToList();

        AstNode astNode = parseNode.Type switch
        {
            "argument"                       => Argument(parseNode, children),
            "argument_list"                  => ArgumentList(parseNode, children),
            "assignment_statement"           => Assignment(parseNode, children),
            "array_assignment_statement"     => ArrayAssignment(parseNode, children),
            "array_index"                    => ArrayIndex(parseNode, children),
            "binary_expression"              => BinaryExpression(parseNode, children),
            "block"                          => Block(parseNode, children),
            "bool"                           => Bool(parseNode),
            "function_definition_list"       => FunctionDefinitionList(parseNode, children),
            "for_statement"                  => For(parseNode, children),
            "float"                          => Float(parseNode),
            "function_call_expression"       => Call(parseNode, children),
            "function_definition"            => FunctionDefinition(parseNode, children),
            "identifier"                     => Identifier(parseNode),
            "if_statement"                   => If(parseNode, children),
            "if_else_statement"              => IfElse(parseNode, children),
            "integer"                        => Integer(parseNode),
            "parameter"                      => Parameter(parseNode, children),
            "parameter_list"                 => ParameterList(parseNode, children),
            "return_statement"               => Return(parseNode, children),
            "source_file"                    => SourceFile(parseNode, children),
            "statement_list"                 => StatementList(parseNode, children),
            "single_value_type"              => SingleValueType(parseNode, children),
            "array_type"                     => ArrayType(parseNode, children),
            "unary_expression"               => UnaryExpression(parseNode, children),
            "variable_declaration_statement" => VariableDeclaration(parseNode, children),
            "variable_reference"             => VariableReference(parseNode, children),
            
            _ => throw new SyntaxException(parseNode)
        };

        foreach (var child in children)
        {
            child.Parent = astNode;
        }

        return astNode;
    }

    static Argument Argument(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value, (Expression)c[1]);

    static NodeList<Argument> ArgumentList(ParseNode n, IEnumerable<AstNode> c) =>
        new(n, c.Select(x => (Argument)x).ToList());

    static Assignment Assignment(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value, (Expression)c[1]);

    static ArrayAssignment ArrayAssignment(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value, (Expression)c[1], (Expression)c[2]);
    
    static ArrayIndex ArrayIndex(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, (VariableReference)c[0], (Expression)c[1]);

    static BinaryExpression BinaryExpression(ParseNode n, IReadOnlyList<AstNode> c)
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

    static Block Block(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, (NodeList<Statement>)c[0]);
    
    static NodeList<FunctionDefinition> FunctionDefinitionList(ParseNode n, IEnumerable<AstNode> c) =>
        new(n, c.Select(x => (FunctionDefinition)x).ToList());

    static For For(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value, (Expression)c[1], (Expression)c[2], (Block)c[3]);
    
    static Call Call(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value, (NodeList<Argument>)c[1]);

    static FunctionDefinition FunctionDefinition(ParseNode n, IReadOnlyList<AstNode> c)
    {
        return c[1] is TypeRef 
            ? new FunctionDefinition(n, ((Identifier)c[0]).Value, (TypeRef)c[1], (NodeList<Parameter>)c[2], (Block)c[3]) 
            : new FunctionDefinition(n, ((Identifier)c[0]).Value, null, (NodeList<Parameter>)c[1], (Block)c[2]);
    }
    
    static Identifier Identifier(ParseNode n) =>
        new(n, n.Span.ToString());

    static If If(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, (Expression)c[0], (Block)c[1]);
    
    static IfElse IfElse(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, (Expression)c[0], (Block)c[1], (Block)c[2]);

    static BooleanValue Bool(ParseNode n) => 
        new(n, bool.Parse(n.Span.ToString()));

    static IntegerValue Integer(ParseNode n) => 
        new(n, int.Parse(n.Span.ToString()));

    static FloatValue Float(ParseNode n) => 
        new(n, float.Parse(n.Span.ToString()));

    static Parameter Parameter(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value, (TypeRef)c[1]);

    static NodeList<Parameter> ParameterList(ParseNode n, IEnumerable<AstNode> c) =>
        new(n, c.Select(x => (Parameter)x).ToList());

    static Return Return(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, (Expression)c[0]);

    static SourceFile SourceFile(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, (NodeList<FunctionDefinition>)c[0]);

    static NodeList<Statement> StatementList(ParseNode n, IEnumerable<AstNode> c) =>
        new(n, c.Select(x => (Statement)x).ToList());

    static SingleValueTypeRef SingleValueType(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value);
    
    static ArrayTypeRef ArrayType(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, (TypeRef)c[0], (IntegerValue)c[1]);

    static UnaryExpression UnaryExpression(ParseNode n, IReadOnlyList<AstNode> c)
    {
        var op = n.ChildByFieldName("op")!.Span.ToString() switch
        {
            "-"  => UnaryOperator.Negate,
            "!"  => UnaryOperator.Not,
            
            _ => throw new NotImplementedException("Unary operator not implemented")
        };

        return new UnaryExpression(n, (Expression)c[0], op);
    }

    static VariableDeclaration VariableDeclaration(ParseNode n, IReadOnlyList<AstNode> c)
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

    static VariableReference VariableReference(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value);
}
