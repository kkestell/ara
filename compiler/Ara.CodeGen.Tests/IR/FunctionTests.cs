using System;
using System.Collections.Generic;
using Ara.CodeGen.IR;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.Tests.IR;

public class FunctionTests : TestBase
{
    [Test]
    public void Arguments()
    {
        module = new Module();
        
        var type = new FunctionType(IrType.Integer,
            new List<Parameter> { new ("a", IrType.Integer), new ("b", IrType.Integer) });
        var function = module.AddFunction("test", type);
        
        var block = function.AddBlock();
        
        builder = block.IrBuilder();
        
        var a = function.Argument("a")!;
        var b = function.Argument("b")!;
        builder.Return(builder.Add(a, b));
        
        Console.WriteLine(module.Emit());
    }
}
