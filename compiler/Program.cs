using System.Diagnostics;
using Ara.Ast;
using Ara.TreeSitter;

namespace Ara;

public static class Program
{
    public static void Main()
    {
        using var parser = new Parser();
        using var tree = parser.Parse(@"
            module main

            fn fib(n: int, x: bool) -> bool {
              return true
            }

            fn main(x: int) -> int {
              var y: int = fib(n: 10, x: true)
              var z: bool = true
              if z == true {
                return y + 8
              }
              return (1 + 1) + 2
            }
        ");
        
        Console.WriteLine(tree.Root.Sexp());

        var sw = new Stopwatch();
        sw.Start();

        var ast = AstTransformer.Transform(tree);

        Console.WriteLine($"Generated AST in {sw.Elapsed.TotalMilliseconds}ms");

        new GraphGenerator().Generate(ast, "ara.dot");
    }
}
