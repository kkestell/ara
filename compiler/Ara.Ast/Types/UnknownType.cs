using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Types;

public record UnknownType : Type
{
    public override string ToString()
    {
        return "?";
    }
}