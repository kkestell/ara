using Ara.CodeGen.IR.Values;
using Ara.CodeGen.IR.Values.Instructions;

namespace Ara.CodeGen.Tests.IR.Instructions;

public class BrTests : TestBase
{
    [Test]
    public void BranchConditionally()
    {
        var l1 = builder.Label("l1");
        var l2 = builder.Label("l2");
        var pred = builder.Icmp(IcmpCondition.Equal, new IntegerValue(1), new IntegerValue(1));
        builder.Br(pred, l1, l2);
        builder.Block.AddInstruction(l1);
        builder.Add(new IntegerValue(1), new IntegerValue(2));
        builder.Block.AddInstruction(l2);
        builder.Add(new IntegerValue(3), new IntegerValue(4));

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
    
    [Test]
    public void BranchUnconditionally()
    {
        var label = builder.Label("label");
        builder.Br(label);
        builder.Block.AddInstruction(label);

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              br label %""label""
            label:
            }
        ");
    }
}