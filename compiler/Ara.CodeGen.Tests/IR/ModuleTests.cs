using Ara.CodeGen.IR;
using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.Tests.IR;

public class ModuleTests : TestBase
{
    [Test]
    public void FunctionDeclarations()
    {
        module = new Module();

        var type = new FunctionType(IrType.Integer);
        var function = module.AddFunction("test", type);

        var block = function.AddBlock();

        builder = block.IrBuilder();

        var ir = module.Emit();
        AssertIr(ir, @"
            define i32 @test () {
            entry:
            }
        ");
    }
}