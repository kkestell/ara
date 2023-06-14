#region

using Ara.CodeGen.Ir.IR.Types;
using Ara.CodeGen.Ir.IR.Values;

#endregion

namespace Ara.CodeGen.Tests.IR.Instructions;

public class StoreTests : TestBase
{
    [Test]
    public void StoreAnInteger()
    {
        var ptr = Builder.Alloca(IrType.Integer);
        Builder.Store(new IntegerValue(1), ptr);
        
        AssertIr(Module.Emit(), @"
            define void @test () {
            entry:
                %0 = alloca i32, i32 1, align 4
                store i32 1, ptr %0
            }
        ");
    }
}