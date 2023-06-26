#region

using Ara.Ast.Errors;
using Ara.Ast.Nodes;
using Ara.Ast.Nodes.Abstract;
using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Expressions.Abstract;
using Ara.Ast.Nodes.Expressions.Values;
using Ara.Ast.Nodes.Statements;
using Ara.Ast.Nodes.Statements.Abstract;
using Ara.Parsing;

#endregion

namespace Ara.Ast;

public static class AstTransformer
{
    public static SourceFile Transform(Tree parseTree)
    {
        PrintParseTree(parseTree.Root);
        
        var rootNode = Visit(parseTree.Root);

        return (SourceFile)rootNode;
    }
    
    private static void PrintParseTree(ParseNode parseNode, int indent = 0)
    {
        Console.WriteLine($"{new string(' ', indent)}{parseNode.Type} ({parseNode.Location})");
        foreach (var child in parseNode.NamedChildren)
        {
            PrintParseTree(child, indent + 2);
        }
    }

    private static AstNode Visit(ParseNode parseNode)
    {
        var children = parseNode.NamedChildren.Select(Visit).ToList();

        AstNode astNode = parseNode.Type switch
        {
            "array_type"                         => ArrayType(parseNode, children),
            "argument"                           => Argument(parseNode, children),
            "argument_list"                      => ArgumentList(parseNode, children),
            "assignment_statement"               => Assignment(parseNode, children),
            "array_assignment_statement"         => ArrayAssignment(parseNode, children),
            "array_index"                        => ArrayIndex(parseNode, children),
            "binary_expression"                  => BinaryExpression(parseNode, children),
            "block"                              => Block(parseNode, children),
            "bool"                               => Bool(parseNode),
            "function_call_expression"           => CallExpression(parseNode, children),
            "function_call_statement"            => CallStatement(parseNode, children),
            "external_function_declaration_list" => ExternalFunctionDeclarationList(parseNode, children),
            "external_function_declaration"      => ExternalFunctionDeclaration(parseNode, children),
            "for_statement"                      => For(parseNode, children),
            "float"                              => Float(parseNode),
            "definition_list"                    => DefinitionList(parseNode, children),
            "function_definition"                => FunctionDefinition(parseNode, children),
            "identifier"                         => Identifier(parseNode),
            "if_statement"                       => If(parseNode, children),
            "if_else_statement"                  => IfElse(parseNode, children),
            "integer"                            => Integer(parseNode),
            "parameter"                          => Parameter(parseNode, children),
            "parameter_list"                     => ParameterList(parseNode, children),
            "return_statement"                   => Return(parseNode, children),
            "source_file"                        => SourceFile(parseNode, children),
            "statement_list"                     => StatementList(parseNode, children),
            "struct_definition"                  => StructDefinition(parseNode, children),
            "struct_field_list"                  => StructFieldList(parseNode, children),
            "struct_field"                       => StructField(parseNode, children),
            "single_value_type"                  => SingleValueType(parseNode, children),
            "unary_expression"                   => UnaryExpression(parseNode, children),
            "variable_declaration_statement"     => VariableDeclaration(parseNode, children),
            "variable_reference"                 => VariableReference(parseNode, children),
            
            _ => throw new SyntaxException(parseNode)
        };

        foreach (var child in children)
        {
            child.Parent = astNode;
        }

        return astNode;
    }

    private static Argument Argument(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, (Expression)c[0]);

    private static NodeList<Argument> ArgumentList(ParseNode n, IEnumerable<AstNode> c) =>
        new(n, c.Select(x => (Argument)x).ToList());

    private static Assignment Assignment(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value, (Expression)c[1]);

    private static ArrayAssignment ArrayAssignment(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value, (Expression)c[1], (Expression)c[2]);

    private static ArrayIndex ArrayIndex(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, (VariableReference)c[0], (Expression)c[1]);

    private static BinaryExpression BinaryExpression(ParseNode n, IReadOnlyList<AstNode> c)
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

    private static Block Block(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, (NodeList<Statement>)c[0]);

    private static NodeList<ExternalFunctionDeclaration> ExternalFunctionDeclarationList(ParseNode n, IEnumerable<AstNode> c) =>
        new(n, c.Select(x => (ExternalFunctionDeclaration)x).ToList());

    private static ExternalFunctionDeclaration ExternalFunctionDeclaration(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[1]).Value, (NodeList<Parameter>)c[2], (TypeRef)c[0]);

    private static NodeList<AstNode> DefinitionList(ParseNode n, IEnumerable<AstNode> c) =>
        new(n, c.ToList());

    private static For For(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value, (Expression)c[1], (Expression)c[2], (Block)c[3]);

    private static CallExpression CallExpression(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value, (NodeList<Argument>)c[1]);
    
    private static CallStatement CallStatement(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value, (NodeList<Argument>)c[1]);

    private static FunctionDefinition FunctionDefinition(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[1]).Value, (NodeList<Parameter>)c[2], (TypeRef)c[0], (Block)c[3]);

    private static Identifier Identifier(ParseNode n) =>
        new(n, n.Span.ToString());

    private static If If(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, (Expression)c[0], (Block)c[1]);

    private static IfElse IfElse(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, (Expression)c[0], (Block)c[1], (Block)c[2]);

    private static BooleanValue Bool(ParseNode n) => 
        new(n, bool.Parse(n.Span.ToString()));

    private static IntegerValue Integer(ParseNode n) => 
        new(n, int.Parse(n.Span.ToString()));

    private static FloatValue Float(ParseNode n) => 
        new(n, float.Parse(n.Span.ToString()));

    private static Parameter Parameter(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[1]).Value, (TypeRef)c[0]);

    private static NodeList<Parameter> ParameterList(ParseNode n, IEnumerable<AstNode> c) =>
        new(n, c.Select(x => (Parameter)x).ToList());

    private static Return Return(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, (Expression)c[0]);

    private static SourceFile SourceFile(ParseNode n, IReadOnlyList<AstNode> c) =>
        c.Count == 2 
            ? new SourceFile(n, (NodeList<AstNode>)c[1], (NodeList<ExternalFunctionDeclaration>)c[0]) 
            : new SourceFile(n, (NodeList<AstNode>)c[0]);

    private static NodeList<Statement> StatementList(ParseNode n, IEnumerable<AstNode> c) =>
        new(n, c.Select(x => (Statement)x).ToList());
    
    private static StructDefinition StructDefinition(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value, (NodeList<StructField>)c[1]);
    
    private static NodeList<StructField> StructFieldList(ParseNode n, IEnumerable<AstNode> c) =>
        new(n, c.Select(x => (StructField)x).ToList());
    
    private static StructField StructField(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[1]).Value, (TypeRef)c[0]);

    private static SingleValueTypeRef SingleValueType(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value);

    private static ArrayTypeRef ArrayType(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, (TypeRef)c[0], (IntegerValue)c[1]);

    private static UnaryExpression UnaryExpression(ParseNode n, IReadOnlyList<AstNode> c)
    {
        var op = n.ChildByFieldName("op")!.Span.ToString() switch
        {
            "-"  => UnaryOperator.Negate,
            "!"  => UnaryOperator.Not,
            
            _ => throw new NotImplementedException("Unary operator not implemented")
        };

        return new UnaryExpression(n, (Expression)c[0], op);
    }

    private static VariableDeclaration VariableDeclaration(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[1]).Value, (TypeRef)c[0], (Expression)c[2]);

    private static VariableReference VariableReference(ParseNode n, IReadOnlyList<AstNode> c) =>
        new(n, ((Identifier)c[0]).Value);
}
