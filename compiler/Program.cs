using System.Diagnostics;
using Ara.Ast;
using Ara.Parsing;

namespace Ara;

public static class Program
{
    public static void Main()
    {
        using var parser = new Parser();

        using var tree = parser.Parse(@"
            module main

            fn foo(x: int, y: float, z: bool, w: string) -> void {
                var a: int = 10
                if x == 1 {
                  var b: float = y - 4
                  if b {
                    return 69
                  }
                }
                return 4.282168342 * (33.234442 - 2.0)
            }

            fn main(x: int) -> float {
              return 1.123 + foo(x: 10, y: 32.1, z: true, w: ""hi!"")
            }
        ");
        
        //Console.WriteLine(tree.Root.Sexp());

        var sw = new Stopwatch();
        sw.Start();

        var ast = AstTransformer.Transform(tree);

        Console.WriteLine($"Generated AST in {sw.Elapsed.TotalMilliseconds}ms");

        new GraphGenerator().Generate(ast, "ara.dot");
    }
}
