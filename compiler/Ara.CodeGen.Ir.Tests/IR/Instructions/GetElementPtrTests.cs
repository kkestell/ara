#region

using System;
using Ara.CodeGen.Ir.IR.Types;
using Ara.CodeGen.Ir.IR.Values;

#endregion

namespace Ara.CodeGen.Tests.IR.Instructions;

public class GetElementPtrTests : TestBase
{
    [Test]
    public void GetPointerToAnArrayElement()
    {
        var a = Builder.Alloca(new ArrayType(IrType.Integer, 5));
        Builder.GetElementPtr(a, new IntegerValue(1));

        var ir = Module.Emit();
        AssertIr(ir, @"
            define void @test () {
            entry:
              %0 = alloca {[5 x i32]}, i32 1, align 4
              %1 = getelementptr [5 x i32], ptr %0, i32 0, i32 1
            }
        ");
    }
}
