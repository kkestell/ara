namespace Ara.Ast.Semantics.Types;

public record BooleanType : Type
{
    public override string ToString()
    {
        return "bool";
    }
}