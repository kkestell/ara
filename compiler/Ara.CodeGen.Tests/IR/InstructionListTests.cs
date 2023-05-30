#region

using System.Collections.Generic;
using System.Linq;
using Ara.CodeGen.IR;
using Ara.CodeGen.IR.Values;

#endregion

namespace Ara.CodeGen.Tests.IR;

public class InstructionListTests
{
    [Test]
    public void Test1()
    {
        var i = new InstructionList();

        var v1 = new IntegerValue(1);
        var v2 = new IntegerValue(2);
        var v3 = new IntegerValue(3);
        var v4 = new IntegerValue(4);
        var v5 = new IntegerValue(5);
        
        // ·1·
        //   ^
        
        Assert.That(i.Index, Is.EqualTo(0));

        i.Insert(v1);

        // ·1·
        //   ^

        Assert.That(i.Index, Is.EqualTo(1));
        Assert.That(i.Values, Is.EqualTo(new List<Value> { v1 }));

        i.PositionBefore(v1);

        // ·1·
        // ^
        
        Assert.That(i.Index, Is.EqualTo(0));
        
        i.Insert(v2);

        // ·2·1·
        //   ^
        
        Assert.That(i.Index, Is.EqualTo(1));
        Assert.That(i.Values, Is.EqualTo(new List<Value> { v2, v1 }));
        
        i.PositionAfter(v2);

        // ·2·1·
        //   ^
        
        Assert.That(i.Index, Is.EqualTo(1));
        
        i.Insert(v3);

        // ·2·3·1·
        //     ^
        
        Assert.That(i.Index, Is.EqualTo(2));
        Assert.That(i.Values, Is.EqualTo(new List<Value> { v2, v3, v1 }));
        
        i.PositionAtEnd();

        // ·2·3·1·
        //       ^
        
        Assert.That(i.Index, Is.EqualTo(3));
        
        i.Insert(v4);

        // ·2·3·1·4·
        //         ^
        Assert.That(i.Index, Is.EqualTo(4));
        Assert.That(i.Values, Is.EqualTo(new List<Value> { v2, v3, v1, v4 }));
        
        i.PositionAtStart();

        // ·2·3·1·4·
        // ^
        
        Assert.That(i.Index, Is.EqualTo(0));
        
        i.Insert(v5);

        // ·5·2·3·1·4·
        // ^
        Assert.That(i.Index, Is.EqualTo(1));
        Assert.That(i.Values, Is.EqualTo(new List<Value> { v5, v2, v3, v1, v4 }));
    }
    
    [Test]
    public void Insert_MaintainsOrder()
    {
        var i = new InstructionList();

        var v1 = new IntegerValue(1);
        var v2 = new IntegerValue(2);

        // ·1·
        //   ^
        i.Insert(v1);
        
        // ·1·2·
        //   ^
        i.Insert(v2);

        Assert.That(i.Values, Is.EqualTo(new List<Value> { v1, v2 }));
    }

    [Test]
    public void PositionBefore_UpdatesIndex()
    {
        var i = new InstructionList();

        var v1 = new IntegerValue(1);
        i.Insert(v1);

        // ·1·
        // ^
        i.PositionBefore(v1);
        Assert.That(i.Index, Is.EqualTo(0));
    }

    [Test]
    public void PositionAfter_UpdatesIndex()
    {
        var i = new InstructionList();

        var v1 = new IntegerValue(1);
        i.Insert(v1);

        // ·1·
        //   ^
        i.PositionAfter(v1);
        Assert.That(i.Index, Is.EqualTo(1));
    }

    [Test]
    public void PositionAtStart_SetsIndexToZero()
    {
        var i = new InstructionList();

        var v1 = new IntegerValue(1);
        i.Insert(v1);

        // ·1·
        // ^
        i.PositionAtStart();
        Assert.That(i.Index, Is.EqualTo(0));
    }

    [Test]
    public void PositionAtEnd_SetsIndexToEnd()
    {
        var i = new InstructionList();

        var v1 = new IntegerValue(1);
        i.Insert(v1);

        // ·1·
        //   ^
        i.PositionAtEnd();
        Assert.That(i.Index, Is.EqualTo(1)); // Index should be the same as the count after PositionAtEnd
    }

    [Test]
    public void Count_ReturnsCorrectNumber()
    {
        var i = new InstructionList();

        var v1 = new IntegerValue(1);
        var v2 = new IntegerValue(2);

        // ·1·
        //   ^
        i.Insert(v1);
        
        // ·1·2·
        //   ^
        i.Insert(v2);

        Assert.That(i.Count, Is.EqualTo(2));
    }

    [Test]
    public void GetEnumerator_ReturnsCorrectSequence()
    {
        var i = new InstructionList();

        var v1 = new IntegerValue(1);
        var v2 = new IntegerValue(2);
        var v3 = new IntegerValue(3);

        // ·1·
        //   ^
        i.Insert(v1);
        
        // ·1·2·
        //   ^
        i.Insert(v2);
        
        // ·1·2·3·
        //     ^
        i.Insert(v3);

        var values = i.ToList();

        Assert.That(values, Is.EqualTo(new List<Value> { v1, v2, v3 }));
    }

}