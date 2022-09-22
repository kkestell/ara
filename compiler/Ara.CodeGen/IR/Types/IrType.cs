using Ara.Ast.Semantics;
using Type = Ara.Ast.Semantics.Type;

namespace Ara.CodeGen.IR.Types;

public abstract record IrType
{
    public abstract string ToIr();

    public static IrType FromType(Type type)
    {
        return type switch
        {
            Ast.Semantics.VoidType    => Void,
            Ast.Semantics.IntegerType => Integer,
            Ast.Semantics.BooleanType => Bool,
            Ast.Semantics.FloatType   => Float,
            Ast.Semantics.ArrayType a => new ArrayType(FromType(a.Type), a.Size),
            _ => throw new NotImplementedException()
        };
    }

    public static readonly VoidType Void = new ();
    public static readonly IntegerType Integer = new (32);
    public static readonly BooleanType Bool = new ();
    public static readonly FloatType Float = new ();
}
