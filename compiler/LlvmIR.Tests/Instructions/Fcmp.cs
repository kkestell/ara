using Ara.CodeGen.Values;
using Ara.CodeGen.Values.Instructions;

namespace LlvmIR.Tests.Instructions;

public class FcmpTests : TestBase
{
    [Test]
    public void CompareTwoFloatsForEquality()
    {
        builder.Fcmp(FcmpCondition.OrderedAndEqual, new FloatValue(3.14f), new FloatValue(2.71f));

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
                %""0"" = fcmp oeq float 0x40091EB860000000, 0x4005AE1480000000
            }
        ");
    }
}