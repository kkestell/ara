namespace Ara.CodeGen.IR.Types;

public record ArrayType(IrType Type, List<int> Sizes) : IrType
{
    public override string ToIr()
    {
        return ToIrRecursive(Sizes, 0, Type);
    }

    private string ToIrRecursive(List<int> sizes, int index, IrType type)
    {
        if (index >= sizes.Count) 
        {
            return type.ToIr();
        }
        
        return $"[{sizes[index]} x {ToIrRecursive(sizes, index + 1, type)}]";
    }
}
