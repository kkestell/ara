#region

using Ara.CodeGen.Ir.IR.Values;

#endregion

namespace Ara.CodeGen.Tests.IR.Instructions;

public class ReturnTests : TestBase
{
    [Test]
    public void ReturnVoid()
    {
        Builder.Return();
        
        AssertIr(Module.Emit(), @"
            define void @test () {
            entry:
              ret void
            }
        ");
    }
    
    [Test]
    public void ReturnAnInteger()
    {
        Builder.Return(new IntegerValue(1));
        
        AssertIr(Module.Emit(), @"
            define void @test () {
            entry:
              ret i32 1
            }
        ");
    }
}