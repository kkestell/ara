using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Types;

public record ArrayType(Type Type, int Size) : Type
{
    public override string ToString()
    {
        return $"{Type}[{Size}]";
    }
}