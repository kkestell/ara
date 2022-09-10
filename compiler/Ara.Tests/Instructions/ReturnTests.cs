namespace Ara.Tests.Instructions;

public class ReturnTests
{
    Module module;
    IrBuilder builder;
    
    [SetUp]
    public void Setup()
    {
        module = new Module();
        
        var func = module.AppendFunction("test", new FunctionType(new IntegerType(32)));
        func.AppendBasicBlock();
        
        var block = func.AppendBasicBlock();
        
        builder = new IrBuilder(block);    
    }

    [Test]
    public void ReturnAnInteger()
    {
        builder.Return(new IntegerValue(1));
        
        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define i32 @test () {\nret i32 1\n}"));
    }
}