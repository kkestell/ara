using Ara.Ast;
using Ara.CodeGen;

namespace Ara.Tests.CodeGen;

public class CodeGenTests : TestBase
{
    [Test]
    public void EmitFunction()
    {
        using var tree = Parse(@"
            module main

            fn main() -> int {
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
        ");
    }
    
    [Test]
    public void Fib()
    {
        using var tree = Parse(@"
            module main

            fn fib(n: int) -> int {
              if n == 0 {
                return 0
              }
              if n == 1 {
                return 1
              }
              return fib(n: n-2) + fib(n: n-1)
            }

            fn main() -> int {
              return fib(n: 10)
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
            endif:
              %""2"" = icmp eq i32 %""n"", 1
              br i1 %""2"", label %""if.0"", label %""endif.0""
            if.0:
              ret i32 1
            endif.0:
              %""4"" = sub i32 %""n"", 2
              %""5"" = call i32 @fib(i32 %""4"")
              %""6"" = sub i32 %""n"", 1
              %""7"" = call i32 @fib(i32 %""6"")
              %""8"" = add i32 %""5"", %""7""
              ret i32 %""8""
            }
            define i32 @main () {
            entry:
              %""0"" = call i32 @fib(i32 10)
              ret i32 %""0""
            }
        ");
    }
}