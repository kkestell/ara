namespace Ara.Tests.CodeGen;

public class LabelTests : TestBase
{
    [Test]
    public void Label()
    {
        builder.Block.AddInstruction(builder.Label("foo"));

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
            foo:
            }
        ");
    }
}