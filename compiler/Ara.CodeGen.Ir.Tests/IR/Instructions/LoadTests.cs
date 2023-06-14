#region

using System;
using Ara.CodeGen.Ir.IR.Types;

#endregion

namespace Ara.CodeGen.Tests.IR.Instructions;

public class LoadTests : TestBase
{
    [Test]
    public void LoadAnInteger()
    {
        var ptr = Builder.Alloca(IrType.Integer);
        var value = Builder.Load(ptr);
        
        var ir = Module.Emit();
        
        Assert.Multiple(() =>
        {
            AssertIr(ir, @"
                define void @test () {
                entry:
                  %0 = alloca i32, i32 1, align 4
                  %1 = load i32, ptr %0
                }
            ");
            Assert.That(value.Type, Is.InstanceOf<IntegerType>());
        });
    }

    [Test]
    public void ThrowWhenArgumentIsNotAPointer()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            var ptr = Builder.Alloca(IrType.Integer);
            var value = Builder.Load(ptr);
            Builder.Load(value);
        });
    }
}