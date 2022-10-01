using Type = Ara.Ast.Types.Abstract.Type;

namespace Ara.Ast.Types;

public record IntegerType : Type
{
    public override string ToString()
    {
        return "int";
    }
}