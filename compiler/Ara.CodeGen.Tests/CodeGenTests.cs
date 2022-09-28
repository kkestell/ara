using System.Linq;
using Ara.Ast;
using Ara.Parsing;

namespace Ara.CodeGen.Tests;

public class CodeGeneratorTests
{
    Parser parser = null!;

    [SetUp]
    public void Setup()
    {
        parser = new Parser();
    }

    [TearDown]
    public void TearDown()
    {
        parser.Dispose();
    }

    [Test]
    public void EmitFunction()
    {
        using var tree = parser.Parse(@"
            module main

            int main() {
              return 1
            }
        ");
        var ast = AstTransformer.Transform(tree);
        var ir = new CodeGenerator().Generate(ast);

        AssertIr(ir, @"
            define i32 @main () {
            entry:
                ret i32 1
            }
            declare ptr @GC_malloc(i64 noundef)
        ");
    }
    
    [Test]
    public void Fib()
    {
        using var tree = parser.Parse(@"
            module main

            int fib(n: int) {
              if n == 0 {
                return 0
              }
              if n == 1 {
                return 1
              }
              return fib(n: n-2) + fib(n: n-1)
            }
        ");
        var ast = AstTransformer.Transform(tree);
        var ir = new CodeGenerator().Generate(ast);

        AssertIr(ir, @"
            define i32 @fib (i32 %n) {
            entry:
              %""0"" = icmp eq i32 %""n"", 0
              br i1 %""0"", label %""if"", label %""endif""
            if:
              ret i32 0
              br label %""endif""
            endif:
              %""4"" = icmp eq i32 %""n"", 1
              br i1 %""4"", label %""if.0"", label %""endif.0""
            if.0:
              ret i32 1
              br label %""endif.0""
            endif.0:
              %""8"" = sub i32 %""n"", 2
              %""9"" = call i32 @fib(i32 %""8"")
              %""10"" = sub i32 %""n"", 1
              %""11"" = call i32 @fib(i32 %""10"")
              %""12"" = add i32 %""9"", %""11""
              ret i32 %""12""
            }
            declare ptr @GC_malloc(i64 noundef)
        ");
    }
    
    protected static void AssertIr(string actual, string expected)
    {
        var a = Trim(actual);
        var e = Trim(expected);
        Assert.That(a, Is.EqualTo(e));
    }

    static string Trim(string str)
    {
        return string.Join('\n', str.Split('\n').Select(line => line.TrimStart())).Trim();
    }
}