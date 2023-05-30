#region

using Ara.CodeGen.IR.Values;
using Ara.CodeGen.IR.Values.Instructions;

#endregion

namespace Ara.CodeGen.Tests.IR.Instructions;

public class BrTests : TestBase
{
    [Test]
    public void BranchConditionally()
    {
        var l1 = Builder.Label("l1");
        var l2 = Builder.Label("l2");
        var pred = Builder.Icmp(IcmpCondition.Equal, new IntegerValue(1), new IntegerValue(1));
        Builder.Br(pred, l1, l2);
        Builder.Function.AddInstruction(l1);
        Builder.Add(new IntegerValue(1), new IntegerValue(2));
        Builder.Function.AddInstruction(l2);
        Builder.Add(new IntegerValue(3), new IntegerValue(4));

        AssertIr(Module.Emit(), @"
            define void @test () {
            entry:
              %0 = icmp eq i32 1, 1
              br i1 %0, label %l1, label %l2
            l1:
              %1 = add i32 1, 2
            l2:
              %2 = add i32 3, 4
            }
        ");
    }
    
    [Test]
    public void BranchUnconditionally()
    {
        var label = Builder.Label("label");
        Builder.Br(label);
        Builder.Function.AddInstruction(label);

        AssertIr(Module.Emit(), @"
            define void @test () {
            entry:
              br label %label
            label:
            }
        ");
    }
}