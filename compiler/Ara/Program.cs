using System.Diagnostics;
using Ara.Ast;
using Ara.Ast.Errors;
using Ara.Ast.Semantics;
using Ara.CodeGen;
using Ara.Parsing;

namespace Ara;

public static class Program
{
    public static int Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Usage: ara file");
            return 1;
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
            return 1;
        }
        
        // Output
        
        var dir = GetTemporaryDirectory();
        var name = Path.GetFileNameWithoutExtension(args[0]);

        // Make IR
        
        var ir = new CodeGenerator().Generate(ast);
        File.WriteAllText(Path.Combine(dir, $"{name}.ll"), ir);

        var irPath = Path.Combine(Directory.GetCurrentDirectory(), $"{name}.ll");
        if (File.Exists(irPath))
            File.Delete(irPath);
        File.Copy(Path.Combine(dir, $"{name}.ll"), irPath);

        // Make binary
        
        var llc = Process.Start(new ProcessStartInfo
        {
            FileName = "llc",
            Arguments = $"-filetype=obj -opaque-pointers {name}.ll -o {name}.o",
            WorkingDirectory = dir
        });
        llc.WaitForExit();
        
        var clang = Process.Start(new ProcessStartInfo
        {
            FileName = "clang",
            Arguments = $"{name}.o -o {name}",
            WorkingDirectory = dir
        });
        clang.WaitForExit();

        var binPath = Path.Combine(Directory.GetCurrentDirectory(), name);
        if (File.Exists(binPath))
            File.Delete(binPath);
        File.Copy(Path.Combine(dir, name), binPath);

        // Make AST graph
        
        new GraphGenerator().Generate(ast, Path.Combine(dir, $"{name}.dot"));

        var dot = Process.Start(new ProcessStartInfo
        {
            FileName = "dot",
            Arguments = $"-Tpdf {name}.dot -o {name}.pdf",
            WorkingDirectory = dir
        });
        dot.WaitForExit();
        
        var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), $"{name}.pdf");
        if (File.Exists(pdfPath))
            File.Delete(pdfPath);
        File.Copy(Path.Combine(dir, $"{name}.pdf"), pdfPath);
        
        return 0;
    }
    
    static string GetTemporaryDirectory()
    {
        var t = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(t);
        return t;
    }
}