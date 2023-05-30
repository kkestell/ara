#region

using Type = Ara.Ast.Types.Abstract.Type;

#endregion

namespace Ara.Ast.Types;

public record UnknownType : Type
{
    public override string ToString()
    {
        return "?";
    }
}