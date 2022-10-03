namespace Ara.Ast.Types.Abstract;

public abstract record Type
{
    public static VoidType    Void    => new();
    public static IntegerType Integer => new();
    public static FloatType   Float   => new();
    public static BooleanType Boolean => new();
    public static UnknownType Unknown => new();
}