using System.Collections.Generic;
using System.Linq;
using Ara.CodeGen.IR;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.Tests.IR;

public class BlockTests
{
    [Test]
    public void NameScopes()
    {
        var module = new Module();
        var type = new FunctionType(IrType.Void, new List<Parameter>());
        var function = module.AddFunction("test", type);
        
        var block = function.AddBlock();
        var builder = block.IrBuilder();

        builder.Alloca(IrType.Integer, 1, "foo");
        
        var newBlock = block.AddChild("block2");
        var newBuilder = newBlock.IrBuilder();

        newBuilder.Alloca(IrType.Integer, 1, "bar");

        Assert.That(block.NameScope.Names.ToList(), Is.EquivalentTo(new [] { "entry", "foo" }));
        Assert.That(newBlock.NameScope.Names.ToList(), Is.EquivalentTo(new [] { "entry", "foo", "block2", "bar" }));
    }
}