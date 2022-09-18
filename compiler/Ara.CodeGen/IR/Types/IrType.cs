namespace Ara.CodeGen.IR.Types;

public abstract record IrType
{
    public abstract string ToIr();

    public static IrType FromString(string type)
    {
        return type switch
        {
            "void"  => Void,
            "int"   => Int32,
            "bool"  => Bool,
            "float" => Float,
            "string" => String,
            
            _ => throw new NotImplementedException()
        };
    }

    public static readonly VoidType Void = new ();
    public static readonly IntType Int32 = new (32);
    public static readonly BitType Bool = new ();
    public static readonly FloatType Float = new ();
    public static readonly StringType String = new();
}
