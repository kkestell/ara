using System.Diagnostics;
using Ara.Ast;
using Ara.Ast.Nodes;
using Ara.CodeGen;
using Ara.Parsing;

namespace Ara;

public static class Program
{
    public static void Main()
    {
        using var parser = new Parser();

        using var tree = parser.Parse(@"
            module main

            fn foo() -> int {
              var x: int = 0
              var y: float = 1.1 + 2.2 - 3.3 * 4.4 / 5.5
              return 1 + 2 - 3 * 4 / 5
            }
        ");

        //Console.WriteLine(tree.Root.Sexp());

        var sw1 = new Stopwatch();
        sw1.Start();

        var ast = AstTransformer.Transform(tree);

        Console.WriteLine($"AST {sw1.Elapsed.TotalMilliseconds}ms");
        
        var sw2 = new Stopwatch();
        sw2.Start();

        var ir = CodeGenerator.Generate(ast);
        
        Console.WriteLine($"IR  {sw2.Elapsed.TotalMilliseconds}ms\n");

        Console.WriteLine(ir);

        new GraphGenerator().Generate(ast, "ara.dot");
    }
}