using System;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.Tests.IR.Instructions;

public class LoadTests : TestBase
{
    [Test]
    public void LoadAnInteger()
    {
        var ptr = builder.Alloca(IrType.Int32);
        var value = builder.Load(ptr);
        
        var ir = module.Emit();
        
        Assert.Multiple(() =>
        {
            AssertIr(ir, @"
                define void @test () {
                entry:
                  %""0"" = alloca i32, align 4
                  %""1"" = load i32, ptr %""0""
                }
            ");
            Assert.That(value.Type, Is.InstanceOf<IntType>());
        });
    }

    [Test]
    public void ThrowWhenArgumentIsNotAPointer()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            var ptr = builder.Alloca(IrType.Int32);
            var value = builder.Load(ptr);
            builder.Load(value);
        });
    }
}