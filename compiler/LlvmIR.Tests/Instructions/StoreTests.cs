namespace LlvmIR.Tests.Instructions;

public class StoreTests : TestBase
{
    [Test]
    public void StoreAnInteger()
    {
        var ptr = builder.Alloca(IrType.Int32);
        builder.Store(new IntValue(1), ptr);
        
        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
                %""0"" = alloca i32, align 4
                store i32 1, ptr %""0""
            }
        ");
    }
}