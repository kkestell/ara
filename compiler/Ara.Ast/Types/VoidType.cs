using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Types;

public record VoidType : Type
{
    public override string ToString()
    {
        return "void";
    }
}