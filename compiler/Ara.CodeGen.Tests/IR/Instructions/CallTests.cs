using System.Collections.Generic;
using Ara.CodeGen.IR;
using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.Tests.IR.Instructions;

public class CallTests : TestBase
{
    [Test]
    public void Call()
    {
        builder.Call("test", new List<Argument> { new (IrType.Integer, new IntegerValue(1))});

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              %""0"" = call i32 @test(i32 1)
            }
        ");
    }
}