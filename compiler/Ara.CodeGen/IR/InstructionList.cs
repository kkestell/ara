using System.Collections;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.IR;

public class InstructionList : IEnumerable<Value>
{
    readonly List<Value> values = new();
    
    public void Insert(Value instruction)
    {
        values.Insert(Index, instruction);
        Index++;
    }

    public IEnumerable<Value> Values => values.AsEnumerable();

    public int Index { get; private set; }

    public int Count => values.Count;

    public void PositionBefore(Value instruction)
    {
        Index = values.IndexOf(instruction);
    }

    public void PositionAfter(Value instruction)
    {
        Index = values.IndexOf(instruction) + 1;
    }

    public void PositionAtStart()
    {
        Index = 0;
    }

    public void PositionAtEnd()
    {
        Index = values.Count;
    }

    public IEnumerator<Value> GetEnumerator()
    {
        return values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)values).GetEnumerator();
    }
}