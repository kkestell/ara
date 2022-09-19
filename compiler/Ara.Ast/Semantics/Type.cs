namespace Ara.Ast.Semantics;

public abstract record Type
{
    public static Type Parse(string type)
    {
        if (type.EndsWith("[]"))
        {
            return new ArrayType(Parse(type[..^2]));
        }

        return type switch
        {
            "int" => new IntegerType(),

            _ => throw new NotImplementedException()
        };
    }
}

public record ArrayType(Type ElementType) : Type
{
    public override string ToString()
    {
        return $"[]{ElementType}";
    }
}

public record EmptyType : Type
{
    public override string ToString()
    {
        return "?";
    }
}

public record VoidType : Type
{
    public override string ToString()
    {
        return "void";
    }
}

public record IntegerType : Type
{
    public override string ToString()
    {
        return "int";
    }
}

public record FloatType : Type
{
    public override string ToString()
    {
        return "float";
    }
}

public record BooleanType : Type;