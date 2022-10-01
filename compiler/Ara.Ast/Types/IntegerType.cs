namespace Ara.Ast.Types;

public record IntegerType : Type
{
    public override string ToString()
    {
        return "int";
    }
}