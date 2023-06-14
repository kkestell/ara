#region

using System.Collections;
using Ara.CodeGen.Ir.IR.Values;

#endregion

namespace Ara.CodeGen.Ir.IR;

public class InstructionList : IEnumerable<Value>
{
    private readonly List<Value> _values = new();
    
    public void Insert(Value instruction)
    {
        _values.Insert(Index, instruction);
        Index++;
    }

    public IEnumerable<Value> Values => _values.AsEnumerable();

    public int Index { get; private set; }

    public int Count => _values.Count;

    public void PositionBefore(Value instruction)
    {
        Index = _values.IndexOf(instruction);
    }

    public void PositionAfter(Value instruction)
    {
        Index = _values.IndexOf(instruction) + 1;
    }

    public void PositionAtStart()
    {
        Index = 0;
    }

    public void PositionAtEnd()
    {
        Index = _values.Count;
    }

    public IEnumerator<Value> GetEnumerator()
    {
        return _values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_values).GetEnumerator();
    }
}