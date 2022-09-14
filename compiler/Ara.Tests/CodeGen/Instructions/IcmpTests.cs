
namespace Ara.Tests.CodeGen.Instructions;

public class IcmpTests : TestBase
{
    [Test]
    public void CompareTwoIntegersForEquality()
    {
        builder.Icmp(IcmpCondition.Equal, new IntValue(1), new IntValue(1));

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              %""0"" = icmp eq i32 1, 1
            }
        ");
    }

    [Test]
    public void CompareTwoPointersForEquality()
    {
        var ptr1 = builder.Alloca(IrType.Int);
        var ptr2 = builder.Alloca(IrType.Int);
        builder.Icmp(IcmpCondition.Equal, ptr1, ptr2);

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              %""0"" = alloca i32, align 4
              %""1"" = alloca i32, align 4
              %""2"" = icmp eq ptr %""0"", %""1""
            }
        ");
    }

    [Test]
    public void CompareTwoIntegersForInequality()
    {
        builder.Icmp(IcmpCondition.NotEqual, new IntValue(1), new IntValue(1));

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              %""0"" = icmp ne i32 1, 1
            }"
        );
    }
    
    [Test]
    public void CompareTwoPointersForInequality()
    {
        var ptr1 = builder.Alloca(IrType.Int);
        var ptr2 = builder.Alloca(IrType.Int);
        builder.Icmp(IcmpCondition.NotEqual, ptr1, ptr2);

        var ir = module.Emit();
        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              %""0"" = alloca i32, align 4
              %""1"" = alloca i32, align 4
              %""2"" = icmp ne ptr %""0"", %""1""
            }
        ");
    }
}