#region

using System.Collections.Generic;
using Ara.CodeGen.IR;
using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;

#endregion

namespace Ara.CodeGen.IR.Tests.IR.Instructions;

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