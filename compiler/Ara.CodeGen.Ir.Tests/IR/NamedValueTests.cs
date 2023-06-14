#region

using Ara.CodeGen.Ir.IR.Values;

#endregion

namespace Ara.CodeGen.Tests.IR;

public class NamedValueTests : TestBase
{
    [Test]
    public void UseTheProvidedName()
    {
        Builder.Add(new IntegerValue(1), new IntegerValue(1), "foo");

        AssertIr(Module.Emit(), @"
            define void @test () {
            entry:
              %foo = add i32 1, 1
            }
        ");
    }
}
