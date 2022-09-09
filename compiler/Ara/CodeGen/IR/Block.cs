using System.Text;
using Ara.CodeGen.IR.Values.Abstract;
using Ara.CodeGen.IR.Values.Instructions.Abstract;

namespace Ara.CodeGen.IR;

public class Block
{
    readonly List<Value> instructions = new();

    public Instruction AddInstruction(Instruction i)
    {
        instructions.Add(i);
        return i;
    }

    public NameScope Scope { get; } = new();

    public void Emit(StringBuilder sb)
    {
        foreach (var inst in instructions)
        {
            inst.Emit(sb);
        }
    }
}