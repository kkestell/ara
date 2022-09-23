using Type = Ara.Ast.Semantics.Types.Type;

namespace Ara.CodeGen.IR.Types;

public abstract record IrType
{
    public abstract string ToIr();

    public static IrType FromType(Type type)
    {
        return type switch
        {
            Ast.Semantics.Types.VoidType    => Void,
            Ast.Semantics.Types.IntegerType => Integer,
            Ast.Semantics.Types.BooleanType => Bool,
            Ast.Semantics.Types.FloatType   => Float,
            Ast.Semantics.Types.ArrayType a => new ArrayType(FromType(a.Type), a.Size),
            _ => throw new NotImplementedException()
        };
    }

    public static readonly VoidType Void = new ();
    public static readonly IntegerType Integer = new (32);
    public static readonly BooleanType Bool = new ();
    public static readonly FloatType Float = new ();
}
