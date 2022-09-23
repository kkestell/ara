namespace Ara.Ast.Semantics.Types;

public record FloatType : Type
{
    public override string ToString()
    {
        return "float";
    }
}