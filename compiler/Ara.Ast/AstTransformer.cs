using Ara.Ast.Nodes;
using Ara.Ast.Types;
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
            "assignment_statement"           => AssignmentStatement(node, children),
            "binary_expression"              => BinaryExpression(node, children),
            "block"                          => Block(node, children),
            "bool"                           => Bool(node),
            "definition_list"                => DefinitionList(node, children),
            "for_statement"                  => ForStatement(node, children),
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
            "variable_reference"             => VariableReference(node, children),
            "unary_expression"               => UnaryExpression(node, children),
            "variable_declaration_statement" => VariableDeclarationStatement(node, children),
            
            _ => throw new NotImplementedException($"Unsupported parse tree node: {node.Type}")
        };

        foreach (var child in children)
        {
            child._Parent = astNode;
        }

        return astNode;
    }

    static Argument Argument(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (Identifier)c[0], (Expression)c[1]);

    static NodeList<Argument> ArgumentList(Node n, IReadOnlyList<AstNode> c) =>
        new (n, c.Select(x => (Argument)x).ToList());

    static Assignment AssignmentStatement(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (Identifier)c[0], (Expression)c[1]);

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
        new (n, ((NodeList<Statement>)c[0]).Nodes);
    
    static NodeList<Definition> DefinitionList(Node n, IReadOnlyList<AstNode> c) =>
        new (n, c.Select(x => (Definition)x));

    static ForStatement ForStatement(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (Identifier)c[0], (Expression)c[1], (Expression)c[2], (Block)c[3]);
    
    static Call FunctionCallExpression(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (Identifier)c[0], ((NodeList<Argument>)c[1]).Nodes.ToList());

    static FunctionDefinition FunctionDefinition(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (Identifier)c[0], (Identifier)c[1], ((NodeList<Parameter>)c[2]).Nodes.ToList(), (Block)c[3]);

    static Identifier Identifier(Node n) =>
        new (n, n.Span.ToString());

    static If IfStatement(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (Expression)c[0], (Block)c[1]);

    static Constant Bool(Node n) =>
        new (n, n.Span.ToString(), new InferredType("bool"));

    static Constant Integer(Node n) =>
        new (n, n.Span.ToString(), new InferredType("int"));

    static Constant String_(Node n) =>
        new (n, n.Span.ToString().Trim('"'), new InferredType("string"));

    static Constant Float(Node n) =>
        new (n, n.Span.ToString(), new InferredType("float"));

    static ModuleDeclaration ModuleDeclaration(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (Identifier)c[0]);

    static Parameter Parameter(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (Identifier)c[0], (Identifier)c[1]);

    static NodeList<Parameter> ParameterList(Node n, IReadOnlyList<AstNode> c) =>
        new (n, c.Select(x => (Parameter)x).ToList());

    static Return ReturnStatement(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (Expression)c[0]);

    static SourceFile SourceFile(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (ModuleDeclaration)c[0], ((NodeList<Definition>)c[1]).Nodes);

    static NodeList<Statement> StatementList(Node n, IReadOnlyList<AstNode> c) =>
        new (n, c.Select(x => (Statement)x).ToList());
    
    static VariableReference VariableReference(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (Identifier)c[0]);

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

    static VariableDeclaration VariableDeclarationStatement(Node n, IReadOnlyList<AstNode> c) =>
        new (n, (Identifier)c[0], (Identifier)c[1], (Expression)c[2]);
}