#region

using System.Collections.Generic;
using Ara.CodeGen.Ir.IR;
using Ara.CodeGen.Ir.IR.Types;
using Ara.CodeGen.Ir.IR.Values;

#endregion

namespace Ara.CodeGen.Tests.IR.Instructions;

public class CallTests : TestBase
{
    [Test]
    public void Call()
    {
        Builder.Call("test", new VoidType(), new List<Argument> { new (IrType.Integer, new IntegerValue(1))});

        var ir = Module.Emit();
        AssertIr(ir, @"
            define void @test () {
            entry:
              call void @test(i32 1)
            }
        ");
    }
}