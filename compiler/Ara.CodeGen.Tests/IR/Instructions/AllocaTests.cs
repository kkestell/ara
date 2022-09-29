using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.Tests.IR.Instructions;

public class AllocaTests : TestBase
{
    [Test]
    public void AllocateAnInteger()
    {
        builder.Alloca(IrType.Integer);

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              %""0"" = alloca i32, i32 1, align 4
            }
        ");
    }
}