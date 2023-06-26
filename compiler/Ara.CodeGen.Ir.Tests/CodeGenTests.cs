#region

using System.Linq;
using Ara.Ast;
using Ara.Ast.Semantics;
using Ara.CodeGen.Ir;
using Ara.Parsing;

#endregion

namespace Ara.CodeGen.Tests;

public class CodeGeneratorTests
{
    private Parser _parser = null!;

    [SetUp]
    public void Setup()
    {
        _parser = new Parser();
    }

    [TearDown]
    public void TearDown()
    {
        _parser.Dispose();
    }

    [Test]
    public void EmitFunction()
    {
        using var tree = _parser.Parse(@"
            fn main() -> int {
              return 1
            }
        ");
        var ast = AstTransformer.Transform(tree);
        new ScopeBuilder(ast).Visit();
        new TypeChecker(ast).Visit();
        var ir = new IrCodeGenerator().Generate(ast);

        AssertIr(ir, @"
            define i32 @main () {
            entry:
                ret i32 1
            }
        ");
    }
    
    [Test]
    public void EmitStruct()
    {
        using var tree = _parser.Parse(@"
            struct foo {
              a: int
              b: int
            }
            fn main() -> int {
              return 1
            }
        ");
        
        var ast = AstTransformer.Transform(tree);
        
        new ScopeBuilder(ast).Visit();
        new TypeChecker(ast).Visit();
        
        var ir = new IrCodeGenerator().Generate(ast);

        AssertIr(ir, @"
            %foo = type { i32, i32 }
            define i32 @main () {
            entry:
                ret i32 1
            }
        ");
    }
    
    [Test]
    public void Fib()
    {
        using var tree = _parser.Parse(@"
            fn fib(n: int) -> int {
              if n == 0 {
                return 0
              }
              if n == 1 {
                return 1
              }
              return fib(n-2) + fib(n-1)
            }
        ");
        
        var ast = AstTransformer.Transform(tree);
        
        new ScopeBuilder(ast).Visit();
        new TypeChecker(ast).Visit();

        var ir = new IrCodeGenerator().Generate(ast);
        
        AssertIr(ir, @"
            define i32 @fib (i32 %n) {
            entry:
              %0 = icmp eq i32 %n, 0
              br i1 %0, label %if, label %endif
            if:
              ret i32 0
            endif:
              %1 = icmp eq i32 %n, 1
              br i1 %1, label %if.1, label %endif.1
            if.1:
              ret i32 1
            endif.1:
              %2 = sub i32 %n, 2
              %3 = call i32 @fib(i32 %2)
              %4 = sub i32 %n, 1
              %5 = call i32 @fib(i32 %4)
              %6 = add i32 %3, %5
              ret i32 %6
            }
        ");
    }

    private static void AssertIr(string actual, string expected)
    {
        var a = Trim(actual);
        var e = Trim(expected);
        Assert.That(a, Is.EqualTo(e));
    }

    private static string Trim(string str)
    {
        return string.Join('\n', str.Split('\n').Select(line => line.TrimStart())).Trim();
    }
}