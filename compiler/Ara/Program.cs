using Ara.Ast;
using Ara.Ast.Errors;
using Ara.Ast.Semantics;
using Ara.CodeGen;
using Ara.Parsing;

namespace Ara;

public static class Program
{
    public static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Usage: ara file");
            return;
        }
        
        using var parser = new Parser();

        using var tree = parser.Parse(File.ReadAllText(args[0]));

        //Console.WriteLine(tree.Root.Sexp());
        
        var ast = AstTransformer.Transform(tree);
        
        try
        {
            new TypeResolver().Visit(ast);
            new TypeChecker().Visit(ast);
        }
        catch (CompilerException ex)
        {
            Console.WriteLine(ex.ToString());
            return;
        }

        new GraphGenerator().Generate(ast, "ara.dot");

        var outFile = Path.ChangeExtension(args[0], ".ll");
        var ir = CodeGenerator.Generate(ast);
        File.WriteAllText(outFile, ir);
        
        // llc -filetype=obj -opaque-pointers adder.ll -o adder.o
        // clang adder.o -o adder
    }
}