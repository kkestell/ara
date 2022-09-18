using Ara.CodeGen.IR.Types;
using Ara.CodeGen.IR.Values;

namespace Ara.CodeGen.Tests.IR;

public class NamedValueTests : TestBase
{
    [Test]
    public void UseTheProvidedValue()
    {
        builder.Add(new IntValue(1), new IntValue(1), "foo");

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              %""foo"" = add i32 1, 1
            }
        ");
    }
    
    [Test]
    public void GenerateUniqueNamesWithinABlock()
    {
        for (var i = 0; i < 3; i++)
        {
            builder.Add(new IntValue(1), new IntValue(1), "foo");
        }

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              %""foo"" = add i32 1, 1
              %""foo.0"" = add i32 1, 1
              %""foo.1"" = add i32 1, 1
            }
        ");
    }
    
    [Test]
    public void GenerateUniqueNamesAcrossBlocks()
    {
        for (var i = 0; i < 3; i++)
        {
            builder.Alloca(IrType.Int32, 1, "foo");
            var child = builder.Block.AddChild(builder.Label("bar"));
            builder = child.IrBuilder();
            builder.Alloca(IrType.Int32, 1, "bar");
        }

        AssertIr(module.Emit(), @"
            define void @test () {
            entry:
              %""foo"" = alloca i32, align 4
            bar:
              %""bar.0"" = alloca i32, align 4
              %""foo.0"" = alloca i32, align 4
            bar.1:
              %""bar.2"" = alloca i32, align 4
              %""foo.1"" = alloca i32, align 4
            bar.3:
              %""bar.4"" = alloca i32, align 4
            }
        ");
    }
}
