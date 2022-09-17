using Ara.CodeGen.Values;
using Ara.CodeGen.Values.Instructions;

namespace LlvmIR.Tests.Instructions;

public class BrTests : TestBase
{
    [Test]
    public void Branch()
    {
        var l1 = builder.Label("l1");
        var l2 = builder.Label("l2");
        var pred = builder.Icmp(IcmpCondition.Equal, new IntValue(1), new IntValue(1));
        builder.Br(pred, l1, l2);
        builder.Block.AddInstruction(l1);
        builder.Add(new IntValue(1), new IntValue(2));
        builder.Block.AddInstruction(l2);
        builder.Add(new IntValue(3), new IntValue(4));

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              %""0"" = icmp eq i32 1, 1
              br i1 %""0"", label %""l1"", label %""l2""
            l1:
              %""2"" = add i32 1, 2
            l2:
              %""3"" = add i32 3, 4
            }
        ");
    }
}