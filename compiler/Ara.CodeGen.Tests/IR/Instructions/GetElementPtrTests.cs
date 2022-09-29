using System;
using Ara.CodeGen.IR;
using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.Tests.IR.Instructions;

public class GetElementPtrTests : TestBase
{
    [Test]
    public void GetPointerToAnArrayElement()
    {
        var a = builder.Call("GC_malloc", new PointerType(IrType.Integer),
            new[] { new Argument(IrType.Integer, new IntegerValue(5)) });
        var p = builder.GetElementPtr(a, new IntegerValue(1));
        builder.Return(p);

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              %""0"" = call ptr @GC_malloc(i32 5)
              %""1"" = getelementptr inbounds [0 x i32], ptr %""0"", i32 0, i32 1
              ret void %""1""
            }
        ");
    }

    [Test]
    public void ThrowWhenArgumentsHaveDifferentTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.Add(new IntegerValue(1), new FloatValue(3.14f));
        });
    }

    [Test]
    public void ThrowWhenArgumentsAreNotIntegers()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.Add(new FloatValue(1), new FloatValue(3.14f));
        });
    }
}