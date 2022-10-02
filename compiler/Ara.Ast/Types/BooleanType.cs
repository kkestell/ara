using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Types;

public record BooleanType : Type
{
    public override string ToString()
    {
        return "bool";
    }
}