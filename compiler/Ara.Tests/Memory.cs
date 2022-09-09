namespace Ara.Tests;

public class Memory
{
    readonly IrType intType = new IntegerType(32);

    Module module;
    IrBuilder builder;
    
    [SetUp]
    public void Setup()
    {
        module = new Module();
        
        var func = module.AppendFunction("test", new FunctionType(intType));
        func.AppendBasicBlock();
        
        var block = func.AppendBasicBlock();
        
        builder = new IrBuilder(block);    
    }
    
    [Test]
    public void Alloca()
    {
        builder.Alloca(intType);
        
        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define i32 @test () {\n%\"0\" = alloca i32, align 4\n}"));
    }
    
    [Test]
    public void Load()
    {
        var ptr = builder.Alloca(intType);
        builder.Load(ptr);
        
        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define i32 @test () {\n%\"0\" = alloca i32, align 4\n%\"1\" = load i32, ptr %\"0\"\n}"));
    }
    
    [Test]
    public void LoadShouldThrowWhenValueIsNotPointer()
    {
        Assert.Throws<ArgumentException>(delegate
        {
            var ptr = builder.Alloca(intType);
            var value = builder.Load(ptr);
            builder.Load(value);
        });
    }
    
    [Test]
    public void Store()
    {
        var ptr = builder.Alloca(intType);
        builder.Store(new IntegerValue(1), ptr);
        
        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define i32 @test () {\n%\"0\" = alloca i32, align 4\nstore i32 1, ptr %\"0\"\n}"));
    }
}