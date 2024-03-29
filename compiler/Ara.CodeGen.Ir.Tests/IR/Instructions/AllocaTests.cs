﻿#region

using Ara.CodeGen.Ir.IR.Types;

#endregion

namespace Ara.CodeGen.Tests.IR.Instructions;

public class AllocaTests : TestBase
{
    [Test]
    public void AllocateAnInteger()
    {
        Builder.Alloca(IrType.Integer);

        AssertIr(Module.Emit(), @"
            define void @test () {
            entry:
              %0 = alloca i32, i32 1, align 4
            }
        ");
    }
}