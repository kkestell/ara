namespace Ara.Tests;

public class BinaryOperations
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
    public void Add()
    {
        var value = builder.Add(new IntegerValue(1), new IntegerValue(1));
        builder.Return(value);
        
        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define i32 @test () {\n%\"0\" = add i32 1, 1\nret i32 %\"0\"\n}"));
    }

    [Test]
    public void SDiv()
    {
        var value = builder.SDiv(new IntegerValue(1), new IntegerValue(1));
        builder.Return(value);
        
        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define i32 @test () {\n%\"0\" = sdiv i32 1, 1\nret i32 %\"0\"\n}"));
    }
    
    [Test]
    public void Mul()
    {
        var value = builder.Mul(new IntegerValue(1), new IntegerValue(1));
        builder.Return(value);
        
        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define i32 @test () {\n%\"0\" = mul i32 1, 1\nret i32 %\"0\"\n}"));
    }
    
    [Test]
    public void Sub()
    {
        var value = builder.Sub(new IntegerValue(1), new IntegerValue(1));
        builder.Return(value);
        
        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define i32 @test () {\n%\"0\" = sub i32 1, 1\nret i32 %\"0\"\n}"));
    }
    
    [Test]
    public void ShouldThrowWithMismatchedTypes()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.Add(new IntegerValue(1), new FloatValue(3.14f));
        });
    }
    
    [Test]
    public void ShouldThrowWhenArgumentsAreNotIntegers()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            builder.Add(new FloatValue(1), new FloatValue(3.14f));
        });
    }
}