using Ara.Ast.Nodes;

namespace Ara.Ast.Semantics;

public abstract record Type
{
    public static Type Parse(TypeRef typeRef)
    {
        return typeRef switch
        {
            SingleValueTypeRef s => ParseSingleValueTypeRef(s),
            ArrayTypeRef a => ParseArrayTypeRef(a),
            _ => throw new Exception()
        };
    }

    static Type ParseArrayTypeRef(ArrayTypeRef a)
    {
        var size = int.Parse(a.Size.Value);
        return new ArrayType(Parse(a.Type), size);
    }
    
    static Type ParseSingleValueTypeRef(SingleValueTypeRef x)
    {
        return x.Name switch
        {
            "void"  => new VoidType(),
            "int"   => new IntegerType(),
            "float" => new FloatType(),
            "bool"  => new BooleanType(),
            
            _ => throw new Exception()
        };
    }
}

public record ArrayType(Type Type, int Size) : Type
{
    public override string ToString()
    {
        return $"{Type}[{Size}]";
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