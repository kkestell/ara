#region

using System;
using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;

#endregion

namespace Ara.CodeGen.Tests.IR.Instructions;

public class GetElementPtrTests : TestBase
{
    [Test]
    public void GetPointerToAnArrayElement()
    {
        var a = Builder.Alloca(IrType.Integer, 5);
        var p = Builder.GetElementPtr(a, new IntegerValue(1));
        Builder.Return(p);

        var ir = Module.Emit();
        AssertIr(ir, @"
            define void @test () {
            entry:
              %0 = alloca i32, i32 5, align 4
              %1 = getelementptr [5 x i32], ptr %0, i32 0, i32 1
              ret ptr %1
            }
        ");
    }

    [Test]
    public void ThrowWhenOperandIsNotAnArray()
    {
        var a = Builder.Alloca(IrType.Integer);
            
        Assert.Throws<ArgumentException>(delegate
        {
            Builder.GetElementPtr(a, new IntegerValue(1));
        });
    }
}
