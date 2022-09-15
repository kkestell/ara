namespace LlvmIR.Tests.Instructions;

public class ReturnTests : TestBase
{
    [Test]
    public void ReturnVoid()
    {
        builder.Return();
        
        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              ret
            }
        ");
    }
    
    [Test]
    public void ReturnAnInteger()
    {
        builder.Return(new IntValue(1));
        
        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              ret i32 1
            }
        ");
    }
}