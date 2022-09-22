using System;
using System.Collections.Generic;
using Ara.CodeGen.IR;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.Tests.IR;

public class ModuleTests : TestBase
{
    [Test]
    public void FunctionDeclarations()
    {
        module = new Module();
        module.DeclareFunction(new FunctionDeclaration("GC_malloc", new PointerType(new VoidType()), new List<IrType> { new IntegerType(64) }));

        var type = new FunctionType(IrType.Integer);
        var function = module.AddFunction("test", type);

        var block = function.AddBlock();

        builder = block.IrBuilder();

        var ir = module.Emit();
        AssertIr(ir, @"
            define i32 @test () {
            entry:
            }
            declare ptr @GC_malloc(i64 noundef)
        ");
    }
}