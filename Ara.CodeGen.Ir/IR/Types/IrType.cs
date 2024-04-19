using Ara.Parsing.Nodes;

namespace Ara.CodeGen.IR.Types;

public abstract record IrType
{
    public abstract string ToIr();

    public static IrType FromType(Parsing.Nodes.TypeRef type)
    {
        if (type is Parsing.Nodes.VoidTypeRef)
        {
            return Void;
        }
        
        if (type is BasicTypeRef basicType)
        {
            return basicType.Identifier.Value switch
            {
                "int" => Integer,
                "bool" => Bool,
                "float" => Float,
                _ => throw new NotImplementedException()
            };
        }
        
        if (type is Parsing.Nodes.ArrayTypeRef arrayType)
        {
            var elementType = FromType(arrayType.ElementType);
            return new ArrayType(elementType, arrayType.Sizes);
        }
        
        throw new NotImplementedException();
    }

    public static readonly VoidType Void = new ();
    public static readonly IntegerType Integer = new (32);
    public static readonly BooleanType Bool = new ();
    public static readonly FloatType Float = new ();
}
