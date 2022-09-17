using Ara.CodeGen.Types;

namespace LlvmIR.Tests.Instructions;

public class AllocaTests : TestBase
{
    [Test]
    public void AllocateAnInteger()
    {
        builder.Alloca(IrType.Int32);

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              %""0"" = alloca i32, align 4
            }
        ");
    }
}