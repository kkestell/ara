namespace Ara.Ast.Semantics.Types;

public record VoidType : Type
{
    public override string ToString()
    {
        return "void";
    }
}