namespace Ara.Ast.Semantics.Types;

public record EmptyType : Type
{
    public override string ToString()
    {
        return "?";
    }
}