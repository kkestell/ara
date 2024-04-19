#region

using Ara.CodeGen.IR.Values;
using Ara.CodeGen.IR.Values.Instructions;

#endregion

namespace Ara.CodeGen.IR.Tests.IR.Instructions;

public class FcmpTests : TestBase
{
    [Test]
    public void CompareTwoFloatsForEquality()
    {
        Builder.Fcmp(FcmpCondition.OrderedAndEqual, new FloatValue(3.14f), new FloatValue(2.71f));

        AssertIr(Module.Emit(), @"
            define void @test () {
            entry:
                %0 = fcmp oeq float 0x40091EB860000000, 0x4005AE1480000000
            }
        ");
    }
}