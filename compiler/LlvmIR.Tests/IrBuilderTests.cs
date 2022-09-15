namespace LlvmIR.Tests;

public class IrBuilderTests : TestBase
{
    [Test]
    public void IfThen()
    {
        builder.IfThen(new BoolValue(true), thenBlock =>
        {
            var thenBuilder = thenBlock.IrBuilder();
            thenBuilder.Add(new IntValue(1), new IntValue(1));
        });
        
        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
                br i1 1, label %""if"", label %""endif""
            if:
                %""1"" = add i32 1, 1
            endif:
            }
        ");
    }
    
    [Test]
    public void SimplestFunction()
    {
        module = new Module();

        var type = new FunctionType(IrType.Int32);
        var func = module.AddFunction("main", type);

        var block = func.AddBlock();

        builder = block.IrBuilder();
        builder.Return(new IntValue(42));

        AssertIr(module.Emit(), @"
            define i32 @main () {
            entry:
                ret i32 24
            }
        ");
    }
}
