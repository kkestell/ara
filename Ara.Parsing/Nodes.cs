using Ara.Parsing.Types;
using Type = Ara.Parsing.Types.Type;

namespace Ara.Parsing.Nodes;

// Abstract

public abstract record Node(Location Location)
{
    public Node? Parent { get; set; } = null;
    
    public abstract List<Node> Children { get; }
    
    public T? FindParent<T>() where T : Node
    {
        var parent = Parent;
        while (parent is not null)
        {
            if (parent is T result)
                return result;
            parent = parent.Parent;
        }
        
        return null;
    }
    
    public bool TryFindDeclaration(string name, out Node? declaration)
    {
        var current = this;
        while (current is not null)
        {
            if (current is Block block)
            {
                foreach (var statement in block.Statements)
                {
                    if (statement is VariableDeclaration variableDeclaration && variableDeclaration.Name == name)
                    {
                        declaration = variableDeclaration;
                        return true;
                    }
                }
            }
            current = current.Parent;
        }
        
        var forLoop = FindParent<For>();
        
        if (forLoop is not null)
        {
            if (forLoop.Initializer is VariableDeclaration variableDeclaration && variableDeclaration.Name == name)
            {
                declaration = variableDeclaration;
                return true;
            }
        }
        
        var funDef = FindParent<FunctionDefinition>();
        
        if (funDef is not null)
        {
            foreach (var parameter in funDef.Parameters)
            {
                if (parameter.Name == name)
                {
                    declaration = parameter;
                    return true;
                }
            }
        }

        var compilationUnit = FindParent<CompilationUnit>()!;

        foreach (var definition in compilationUnit.Definitions)
        {
            switch (definition)
            {
                case FunctionDefinition functionDefinition when functionDefinition.Name == name:
                    declaration = functionDefinition;
                    return true;
                case ExternFunctionDefinition externFunctionDefinition when externFunctionDefinition.Name == name:
                    declaration = externFunctionDefinition;
                    return true;
                case StructDefinition structDefinition when structDefinition.Name == name:
                    declaration = structDefinition;
                    return true;
            }
        }

        declaration = null;
        return false;
    }
    
    public Node? FindDeclaration(string name)
    {
        if (TryFindDeclaration(name, out var declaration))
            return declaration;
        
        return null;
    }
}

public abstract record Definition(string Name, Location Location) : Node(Location);

public abstract record Expression(Location Location) : Node(Location)
{
    public abstract Type? Type { get; set; }
}

public abstract record TypeRef(Location Location) : Node(Location)
{
    public abstract Type? Type { get; set; }
}

public abstract record Statement(Location Location) : Node(Location)
{
}

// Compilation Unit

public record CompilationUnit(List<Definition> Definitions, Location Location) : Node(Location)
{
    // public SymbolTable SymbolTable { get; set; } = new("Global");

    public override List<Node> Children => [..Definitions];
    
    public override string ToString()
    {
        return $"<compilation-unit>{string.Join("", Definitions)}</compilation-unit>";
    }
}

// Definitions

public record ExternFunctionDefinition(string Name, List<Parameter> Parameters, TypeRef TypeRef, Location Location) : Definition(Name, Location)
{
    // public SymbolTable Scope { get; set; } = new($"(Function {Name})");

    public FunctionType? Type { get; set; }

    public override List<Node> Children => [TypeRef, ..Parameters];

    public override string ToString()
    {
        return $"<extern-function-definition name=\"{Name}\"><parameters>{string.Join("", Parameters)}</parameters><return-type>{TypeRef}</return-type></extern-function-definition>";
    }
}

public record FunctionDefinition(string Name, List<Parameter> Parameters, TypeRef TypeRef, Block Body, Location Location) : Definition(Name, Location)
{
    // public SymbolTable Scope { get; set; } = new($"(Function {Name})");

    public FunctionType? Type { get; set; }
    
    public override List<Node> Children => [TypeRef, ..Parameters, Body];

    public override string ToString()
    {
        return $"<function-definition name=\"{Name}\"><parameters>{string.Join("", Parameters)}</parameters><return-type>{TypeRef}</return-type>{Body}</function-definition>";
    }
}

public record Parameter(string Name, TypeRef TypeRef, Location Location) : Node(Location)
{
    public Type? Type { get; set; }
    
    public override List<Node> Children => [TypeRef];
    
    public override string ToString()
    {
        return $"<parameter name=\"{Name}\">{TypeRef}</parameter>";
    }
}

public record BasicTypeRef(string Name, Location Location) : TypeRef(Location)
{
    public override Type? Type { get; set; }
    
    public override List<Node> Children => [];
    
    public override string ToString()
    {
        return $"<basic-type-ref name=\"{Name}\"/>";
    }
}

public record PointerTypeRef(TypeRef ElementType, Location Location) : TypeRef(Location)
{
    public override Type? Type { get; set; }
    
    public override List<Node> Children => [ElementType];
    
    public override string ToString()
    {
        return $"<pointer-type-ref>{ElementType}</pointer-type-ref>";
    }
}

public record ArrayTypeRef(TypeRef ElementType, int Size, Location Location) : TypeRef(Location)
{
    public override Type? Type { get; set; }

    public override List<Node> Children => [ElementType];
    
    public override string ToString()
    {
        return $"<array-type-ref size=\"{Size}\">{ElementType}</array-type-ref>";
    }
}

public record StructDefinition(string Name, List<StructField> Fields, Location Location) : Definition(Name, Location)
{
    // public SymbolTable Scope { get; set; } = new($"(Struct {Name})");

    public StructType? Type { get; set; }

    public override List<Node> Children => [..Fields];
    
    public override string ToString()
    {
        return $"<struct-definition name=\"{Name}\">{string.Join("", Fields)}</struct-definition>";
    }
}

public record StructField(string Name, TypeRef TypeRef, Location Location) : Node(Location)
{
    public Type? Type { get; set; }
    
    public override List<Node> Children => [TypeRef];
    
    public override string ToString()
    {
        return $"<struct-field name=\"{Name}\">{TypeRef}</struct-field>";
    }
}

// Statements

public record Block(List<Statement> Statements, Location Location) : Statement(Location)
{
    // public SymbolTable Scope { get; set; } = new("(Block)");

    public override List<Node> Children => [..Statements];

    public override string ToString()
    {
        return $"<block>{string.Join("", Statements)}</block>";
    }
}

public record VariableDeclaration(string Name, TypeRef TypeRef, Expression? Expression, Location Location) : Statement(Location)
{
    public Expression? Expression { get; set; } = Expression;
    
    public Type? Type { get; set; }

    public override List<Node> Children
    {
        get
        {
            var nodes = new List<Node> { TypeRef };
            if (Expression is not null)
                nodes.Add(Expression);
            return nodes;
        }
    }

    public override string ToString()
    {
        var expressionString = Expression is not null ? $"<expression>{Expression}</expression>" : "";
        return $"<variable-declaration name=\"{Name}\">{TypeRef}{expressionString}</variable-declaration>";
    }
}

public record Assignment(Expression Left, Expression Right, Location Location) : Statement(Location)
{
    public Expression Right { get; set; } = Right;
    
    public override List<Node> Children => [Left, Right];
    
    public override string ToString()
    {
        return $"<assignment><left>{Left}</left><right>{Right}</right></assignment>";
    }
}

public record Return(Expression? Expression, Location Location) : Statement(Location)
{
    public Expression? Expression { get; set; } = Expression;
    
    public override List<Node> Children => Expression is not null ? [Expression] : [];
    
    public override string ToString()
    {
        var expressionString = Expression is not null ? $"<expression>{Expression}</expression>" : "";
        return $"<return>{expressionString}</return>";
    }
}

public record If(Expression Condition, Statement Then, Statement? Else, Location Location) : Statement(Location)
{
    public override List<Node> Children => [Condition, Then];
    
    public override string ToString()
    {
        var elseString = Else is not null ? $"<else>{Else}</else>" : "";
        return $"<if><condition>{Condition}</condition><then>{Then}</then>{elseString}</if>";
    }
}

public record While(Expression Condition, Statement Then, Location Location) : Statement(Location)
{
    public override List<Node> Children => [Condition, Then];
    
    public override string ToString()
    {
        return $"<while><condition>{Condition}</condition><then>{Then}</then></while>";
    }
}

public record For(Statement Initializer, Expression Condition, Statement Increment, Block Body, Location Location) : Statement(Location)
{
    public override List<Node> Children => [Initializer, Condition, Increment, Body];
    
    // public SymbolTable Scope { get; set; } = new("(For)");
    
    public override string ToString()
    {
        return $"<for><initializer>{Initializer}</initializer><condition>{Condition}</condition><increment>{Increment}</increment><body>{Body}</body></for>";
    }
}

public record Break(Location Location) : Statement(Location)
{
    public override List<Node> Children => [];
    
    public override string ToString()
    {
        return "<break/>";
    }
}

public record Continue(Location Location) : Statement(Location)
{
    public override List<Node> Children => [];
    
    public override string ToString()
    {
        return "<continue/>";
    }
}

public record ExpressionStatement(Expression Expression, Location Location) : Statement(Location)
{
    public override List<Node> Children => [Expression];
    
    public override string ToString()
    {
        return $"<expression-statement>{Expression}</expression-statement>";
    }
}

// Expressions

public record Identifier(string Value, Location Location) : Expression(Location)
{
    public override List<Node> Children => [];
 
    public override Type? Type { get; set; }
    
    public override string ToString()
    {
        return $"<identifier value=\"{Value}\"/>";
    }
}

public record NullLiteral(Location Location) : Expression(Location)
{
    public override List<Node> Children => [];
    
    public override Type? Type { get; set; } = new PointerType(new UnknownType());
    
    public override string ToString()
    {
        return "<null-literal/>";
    }
}

public record IntegerLiteral(int Value, Location Location) : Expression(Location)
{
    public override List<Node> Children => [];

    public override Type? Type { get; set; } = new IntType(32);

    public override string ToString()
    {
        return $"<integer-literal value=\"{Value}\"/>";
    }
}

public record FloatLiteral(float Value, Location Location) : Expression(Location)
{
    public override List<Node> Children => [];
    
    public override Type? Type { get; set; } = new FloatType(32);
    
    public override string ToString()
    {
        return $"<float-literal value=\"{Value}\"/>";
    }
}

public record StringLiteral(string Value, Location Location) : Expression(Location)
{
    public override List<Node> Children => [];
    
    public override Type? Type
    {
        get => new StringType();
        set => throw new NotImplementedException();
    }
    public override string ToString()
    {
        return $"<string-literal value=\"{Value}\"/>";
    }
}

public record BooleanLiteral(bool Value, Location Location) : Expression(Location)
{
    public override List<Node> Children => [];
    
    public override Type? Type
    {
        get => new BoolType();
        set => throw new NotImplementedException();
    }

    public override string ToString()
    {
        return $"<boolean-literal value=\"{Value}\"/>";
    }
}

public record UnaryExpression(TokenType Operator, Expression Operand, Location Location) : Expression(Location)
{
    public override List<Node> Children => [Operand];
    
    public override Type? Type { get; set; }

    public override string ToString()
    {
        return $"<unary-expression operator=\"{Operator}\">{Operand}</unary-expression>";
    }
}

public record BinaryExpression(Expression Left, TokenType Operator, Expression Right, Location Location) : Expression(Location)
{
    public Expression Left { get; set; } = Left;
    
    public Expression Right { get; set; } = Right;
    
    public override List<Node> Children => [Left, Right];
    
    public override Type? Type { get; set; }

    public override string ToString()
    {
        return $"<binary-expression operator=\"{Operator}\"><left>{Left}</left><right>{Right}</right></binary-expression>";
    }
}

public record MemberAccessExpression(Expression Left, Expression Right, Location Location) : Expression(Location)
{
    public override List<Node> Children => [Left, Right];
    
    public override Type? Type { get; set; }

    public override string ToString()
    {
        return $"<member-access-expression><left>{Left}</left><right>{Right}</right></member-access-expression>";
    }
}

public record CallExpression(Expression Callee, List<Argument> Arguments, Location Location) : Expression(Location)
{
    public Expression Callee { get; set; } = Callee;

    public List<Argument> Arguments { get; set; } = Arguments;

    public override List<Node> Children => [Callee, ..Arguments];
    
    public override Type? Type { get; set; }

    public override string ToString()
    {
        return $"<call-expression><callee>{Callee}</callee><arguments>{string.Join("", Arguments)}</arguments></call-expression>";
    }
}

public record Argument(Expression Expression, Location Location) : Node(Location)
{
    public Expression Expression { get; set; } = Expression;
    
    public override List<Node> Children => [Expression];
    
    public Type? Type { get; set; }
    
    public override string ToString()
    {
        return $"<argument>{Expression}</argument>";
    }
}

public record IndexAccessExpression(Expression Left, Expression Right, Location Location) : Expression(Location)
{
    public override List<Node> Children => [Left, Right];
    
    public override Type? Type { get; set; }

    public override string ToString()
    {
        return $"<index-access-expression><left>{Left}</left><right>{Right}</right></index-access-expression>";
    }
}

public record ArrayInitializationExpression(List<Expression> Elements, Location Location) : Expression(Location)
{
    public override List<Node> Children => [..Elements];
    
    public override Type? Type { get; set; }

    public override string ToString()
    {
        return $"<array-initialization-expression>{string.Join("", Elements)}</array-initialization-expression>";
    }
}

public record CastExpression(TypeRef TypeRef, Expression Expression, Location Location) : Expression(Location)
{
    public override List<Node> Children => [TypeRef, Expression];
    
    public override Type? Type { get; set; }

    public override string ToString()
    {
        return $"<cast-expression>{TypeRef}{Expression}</cast-expression>";
    }
}

public record DereferenceExpression(Expression Operand, Location Location) : Expression(Location)
{
    public override List<Node> Children => [Operand];
    
    public override Type? Type { get; set; }

    public override string ToString()
    {
        return $"<dereference-expression>{Operand}</dereference-expression>";
    }
}

public record AddressOfExpression(Expression Operand, Location Location) : Expression(Location)
{
    public override List<Node> Children => [Operand];
    
    public override Type? Type { get; set; }
    
    public override string ToString()
    {
        return $"<address-of-expression>{Operand}</address-of-expression>";
    }
}
