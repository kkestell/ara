#region

using Type = Ara.Ast.Types.Abstract.Type;

#endregion

namespace Ara.Ast.Types;

public record BooleanType : Type
{
    public override string ToString()
    {
        return "bool";
    }
}