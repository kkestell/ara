#region

using System;
using System.Collections.Generic;
using Ara.CodeGen.IR;
using Ara.CodeGen.IR.Types;

#endregion

namespace Ara.CodeGen.Tests.IR;

public class FunctionTests : TestBase
{
    [Test]
    public void Arguments()
    {
        Module = new Module();
        
        var type = new FunctionType(IrType.Integer,
            new List<Parameter> { new ("a", IrType.Integer), new ("b", IrType.Integer) });
        var function = Module.AddFunction("test", type);

        Builder = function.IrBuilder();
        
        var a = function.Argument("a")!;
        var b = function.Argument("b")!;
        Builder.Return(Builder.Add(a, b));
        
        Console.WriteLine(Module.Emit());
    }
}
