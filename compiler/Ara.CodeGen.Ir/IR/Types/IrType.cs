#region

using Type = Ara.Ast.Types.Abstract.Type;

#endregion

namespace Ara.CodeGen.Ir.IR.Types;

public abstract record IrType
{
    public abstract string ToIr();

    public static IrType FromType(Type type)
    {
        return type switch
        {
            Ast.Types.VoidType    => Void,
            Ast.Types.IntegerType => Integer,
            Ast.Types.BooleanType => Bool,
            Ast.Types.FloatType   => Float,
            Ast.Types.ArrayType a => new ArrayType(FromType(a.Type), a.Size),
            _ => throw new NotImplementedException()
        };
    }

    public static readonly VoidType Void = new ();
    public static readonly IntegerType Integer = new (32);
    public static readonly BooleanType Bool = new ();
    public static readonly FloatType Float = new ();
}
