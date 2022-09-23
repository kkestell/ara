namespace Ara.Ast.Semantics.Types;

public record IntegerType : Type
{
    public override string ToString()
    {
        return "int";
    }
}