using System.Collections.Generic;
using Ara.CodeGen.IR;
using Ara.CodeGen.IR.Values;

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
        var v4 = new IntegerValue(3);
        var v5 = new IntegerValue(3);
        
        // ·1·
        //   ^
        Assert.That(i.Pointer, Is.EqualTo(0));

        // ·1·
        //   ^
        i.Insert(v1);
        Assert.That(i.Pointer, Is.EqualTo(1));
        Assert.That(i.Instructions, Is.EqualTo(new List<Value> { v1 }));
        
        // ·1·
        // ^
        i.PositionBefore(v1);
        Assert.That(i.Pointer, Is.EqualTo(0));
        
        // ·2·1·
        //   ^
        i.Insert(v2);
        Assert.That(i.Pointer, Is.EqualTo(1));
        Assert.That(i.Instructions, Is.EqualTo(new List<Value> { v2, v1 }));
        
        // ·2·1·
        //   ^
        i.PositionAfter(v2);
        Assert.That(i.Pointer, Is.EqualTo(1));
        
        // ·2·3·1·
        //     ^
        i.Insert(v3);
        Assert.That(i.Pointer, Is.EqualTo(2));
        Assert.That(i.Instructions, Is.EqualTo(new List<Value> { v2, v3, v1 }));
        i.PositionAtEnd();
        
        // ·2·3·1·
        //       ^
        Assert.That(i.Pointer, Is.EqualTo(3));
        
        // ·2·3·1·4·
        //         ^
        i.Insert(v4);
        Assert.That(i.Pointer, Is.EqualTo(4));
        Assert.That(i.Instructions, Is.EqualTo(new List<Value> { v2, v3, v1, v4 }));
        
        // ·2·3·1·4·
        // ^
        i.PositionAtStart();
        Assert.That(i.Pointer, Is.EqualTo(0));
        
        // ·5·2·3·1·4·
        // ^
        i.Insert(v5);
        Assert.That(i.Pointer, Is.EqualTo(1));
        Assert.That(i.Instructions, Is.EqualTo(new List<Value> { v5, v2, v3, v1, v4 }));
    }
}