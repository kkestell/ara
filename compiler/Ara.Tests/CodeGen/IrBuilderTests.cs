namespace Ara.Tests.CodeGen;

public class IrBuilderTests : TestBase
{
    [Test]
    public void IfThen()
    {
        builder.IfThen(new BoolValue(true), thenBlock =>
        {
            var thenBuilder = thenBlock.Builder();
            thenBuilder.Add(new IntValue(1), new IntValue(1));
        });

        var ir = module.Emit();
        
        AssertIr(ir, @"
            define void @test () {
            entry:
                br i1 1, label %""if"", label %""endif""
            if:
                %""1"" = add i32 1, 1
            endif:
        }");
    }
}
