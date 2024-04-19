namespace Ara.CodeGen.IR.Tests.IR;

public class LabelTests : TestBase
{
    [Test]
    public void Label()
    {
        Builder.Function.AddInstruction(Builder.Label("foo"));

        AssertIr(Module.Emit(), @"
            define void @test () {
            entry:
            foo:
            }
        ");
    }
}