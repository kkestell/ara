namespace Ara.Tests;

public class NamedValueTests
{
    [Test]
    public void UseTheProvidedValue()
    {
        var module = new Module();
        var func = module.AppendFunction("test", new FunctionType(new IntegerType(32)));
        func.AppendBasicBlock();
        var block = func.AppendBasicBlock();
        var builder = new IrBuilder(block);
        var value = builder.Add(new IntegerValue(1), new IntegerValue(1), "foo");
        builder.Return(value);

        var ir = module.Emit();
        Assert.That(ir, Is.EqualTo("define i32 @test () {\n%\"foo\" = add i32 1, 1\nret i32 %\"foo\"\n}"));
    }
}