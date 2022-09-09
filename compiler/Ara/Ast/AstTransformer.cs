using Ara.Ast.Nodes;
using Ara.Ast.Nodes.Expressions;
using Ara.Ast.Nodes.Expressions.Atoms;
using Ara.Ast.Nodes.Statements;
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

        return node.Type switch
        {
            "argument"                       => Argument(node, children),
            "argument_list"                  => ArgumentList(node, children),
            "binary_expression"              => BinaryExpression(node, children),
            "block"                          => Block(node, children),
            "bool"                           => Bool(node),
            "definition_list"                => DefinitionList(node, children),
            "float"                          => Float(node),
            "function_call_expression"       => FunctionCallExpression(node, children),
            "function_definition"            => FunctionDefinition(node, children),
            "identifier"                     => Identifier(node),
            "if_statement"                   => IfStatement(node, children),
            "integer"                        => Integer(node),
            "module_declaration"             => ModuleDeclaration(node, children),
            "parameter"                      => Parameter(node, children),
            "parameter_list"                 => ParameterList(node, children),
            "return_statement"               => ReturnStatement(node, children),
            "source_file"                    => SourceFile(node, children),
            "statement_list"                 => StatementList(node, children),
            "string"                         => String_(node),
            "type"                           => Type(node),
            "variable_reference"             => VariableReference(node, children),
            "unary_expression"               => UnaryExpression(node, children),
            "variable_declaration_statement" => VariableDeclarationStatement(node, children),
            
            _ => throw new NotImplementedException($"Unsupported parse tree node: {node.Type}")
        };
    }

    static Argument Argument(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (Identifier)c[0], (Expression)c[1]);

    static ArgumentList ArgumentList(Node n, IReadOnlyList<AstNode> c) =>
        new (n, c.Select(x => (Argument)x).ToList());

    static BinaryExpression BinaryExpression(Node n, IReadOnlyList<AstNode> c)
    {
        var op = n.ChildByFieldName("op")!.Span.ToString();

        return op switch
        {
            "*"  => new MultiplicationExpression(n, (Expression)c[0], (Expression)c[1]),
            "/"  => new DivisionExpression(n, (Expression)c[0], (Expression)c[1]),
            "+"  => new AdditionExpression(n, (Expression)c[0], (Expression)c[1]),
            "-"  => new SubtractionExpression(n, (Expression)c[0], (Expression)c[1]),
            "==" => new EqualityExpression(n, (Expression)c[0], (Expression)c[1]),
            "!=" => new InequalityExpression(n, (Expression)c[0], (Expression)c[1]),
            
            _ => throw new NotImplementedException($"Binary operator {op} not implemented")
        };
    }

    static Block Block(Node n, IReadOnlyList<AstNode> c) =>
        new (n, ((StatementList)c[0]).Values);

    static Bool Bool(Node n) =>
        new (n, n.Span.ToString());

    static DefinitionList DefinitionList(Node n, IReadOnlyList<AstNode> c) =>
        new (n, c.Select(x => (Definition)x));

    static Float Float(Node n) =>
        new (n, n.Span.ToString());

    static FunctionCallExpression FunctionCallExpression(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (Identifier)c[0], ((ArgumentList)c[1]).Values.ToList());

    static FunctionDefinition FunctionDefinition(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (Identifier)c[0], ((ParameterList)c[1]).Values.ToList(), (Type_)c[2], (Block)c[3]);

    static Identifier Identifier(Node n) =>
        new (n, n.Span.ToString());

    static IfStatement IfStatement(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (Expression)c[0], (Block)c[1]);

    static Integer Integer(Node n) =>
        new (n, n.Span.ToString());

    static ModuleDeclaration ModuleDeclaration(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (Identifier)c[0]);

    static Parameter Parameter(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (Identifier)c[0], (Type_)c[1]);

    static ParameterList ParameterList(Node n, IReadOnlyList<AstNode> c) =>
        new (n, c.Select(x => (Parameter)x).ToList());

    static ReturnStatement ReturnStatement(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (Expression)c[0]);

    static SourceFile SourceFile(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (ModuleDeclaration)c[0], ((DefinitionList)c[1]).Values);

    static StatementList StatementList(Node n, IReadOnlyList<AstNode> c) =>
        new (n, c.Select(x => (Statement)x).ToList());

    static String_ String_(Node n) =>
        new (n, n.Span.ToString().Trim('"'));

    static Type_ Type(Node n) =>
        new (n, n.Span.ToString());

    static VariableReference VariableReference(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (Identifier)c[0]);

    static UnaryExpression UnaryExpression(Node n, IReadOnlyList<AstNode> c)
    {
        var op = n.Children.First().Span.ToString();
        return op switch
        {
            "-" => new NegationExpression(n, (Expression)c[0]),
            "!" => new LogicalNegationExpression(n, (Expression)c[0]),
            
            _ => throw new NotImplementedException($"Unary operator {op} not implemented")
        };
    }

    static VariableDeclarationStatement VariableDeclarationStatement(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (Identifier)c[0], (Type_)c[1], (Expression)c[2]);
}
