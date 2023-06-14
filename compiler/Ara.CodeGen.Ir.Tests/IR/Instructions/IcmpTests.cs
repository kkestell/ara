#region

using Ara.CodeGen.Ir.IR.Types;
using Ara.CodeGen.Ir.IR.Values;
using Ara.CodeGen.Ir.IR.Values.Instructions;

#endregion

namespace Ara.CodeGen.Tests.IR.Instructions;

public class IcmpTests : TestBase
{
    [Test]
    public void CompareTwoIntegersForEquality()
    {
        Builder.Icmp(IcmpCondition.Equal, new IntegerValue(1), new IntegerValue(1));

        AssertIr(Module.Emit(), @"
            define void @test () {
            entry:
              %0 = icmp eq i32 1, 1
            }
        ");
    }

    [Test]
    public void CompareTwoPointersForEquality()
    {
        var ptr1 = Builder.Alloca(IrType.Integer);
        var ptr2 = Builder.Alloca(IrType.Integer);
        Builder.Icmp(IcmpCondition.Equal, ptr1, ptr2);

        AssertIr(Module.Emit(), @"
            define void @test () {
            entry:
              %0 = alloca i32, i32 1, align 4
              %1 = alloca i32, i32 1, align 4
              %2 = icmp eq ptr %0, %1
            }
        ");
    }

    [Test]
    public void CompareTwoIntegersForInequality()
    {
        Builder.Icmp(IcmpCondition.NotEqual, new IntegerValue(1), new IntegerValue(1));

        AssertIr(Module.Emit(), @"
            define void @test () {
            entry:
              %0 = icmp ne i32 1, 1
            }"
        );
    }
    
    [Test]
    public void CompareTwoPointersForInequality()
    {
        var ptr1 = Builder.Alloca(IrType.Integer);
        var ptr2 = Builder.Alloca(IrType.Integer);
        Builder.Icmp(IcmpCondition.NotEqual, ptr1, ptr2);

        var ir = Module.Emit();
        AssertIr(ir, @"
            define void @test () {
            entry:
              %0 = alloca i32, i32 1, align 4
              %1 = alloca i32, i32 1, align 4
              %2 = icmp ne ptr %0, %1
            }
        ");
    }
}