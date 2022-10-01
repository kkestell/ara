namespace Ara.Ast.Types;

public record BooleanType : Type
{
    public override string ToString()
    {
        return "bool";
    }
}