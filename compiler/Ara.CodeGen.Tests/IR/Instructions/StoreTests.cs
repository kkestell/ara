using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.Tests.IR.Instructions;

public class StoreTests : TestBase
{
    [Test]
    public void StoreAnInteger()
    {
        var ptr = builder.Alloca(IrType.Integer);
        builder.Store(new IntegerValue(1), ptr);
        
        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
                %""0"" = alloca i32, align 4
                store i32 1, ptr %""0""
            }
        ");
    }
}