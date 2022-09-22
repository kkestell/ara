using System.Diagnostics;
using Ara.Ast;
using Ara.Ast.Errors;
using Ara.Ast.Semantics;
using Ara.CodeGen;
using Ara.Parsing;

namespace Ara;

public static class Cli
{
    public static int Run(string filename)
    {
        // Parse
            
        using var parser = new Parser();
        using var tree = parser.Parse(File.ReadAllText(filename), filename);
        
        // AST
        
        var ast = AstTransformer.Transform(tree);
        
        // Semantics

        try
        {
            new ScopeBuilder().Visit(ast);
            new TypeResolver().Visit(ast);
            new TypeChecker().Visit(ast);
        }
        catch (SemanticException ex)
        {
            Console.WriteLine(ex.ToString());
            return 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            Console.WriteLine(ex.StackTrace);
        }
        
        // Generate IR
        
        var ir = new CodeGenerator().Generate(ast);
        
        #region Output
        
        var dir = GetTemporaryDirectory();
        var name = Path.GetFileNameWithoutExtension(filename);

        // IR
        
        File.WriteAllText(Path.Combine(dir, $"{name}.ll"), ir);
        Copy(dir, name, ".ll");
        
        // Make binary

        var llcPath = Environment.GetEnvironmentVariable("LLC") ?? "llc";
        var clangPath = Environment.GetEnvironmentVariable("CLANG") ?? "clang";

        try
        {
            Run(llcPath, $"-filetype=obj -opaque-pointers -O0 {name}.ll -o {name}.o", dir);
            Run(clangPath, $"{name}.o -o {name} -lgc", dir);
            Copy(dir, name);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }
        
        // Make AST graph
        
        // new GraphGenerator().Generate(ast, Path.Combine(dir, $"{name}.dot"));
        // Run("dot", $"-Tpdf {name}.dot -o {name}.pdf", dir);
        // Copy(dir, name, ".pdf");
        
        #endregion
        
        return 0;
    }

    static void Copy(string root, string name, string ext = "")
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), $"{name}{ext}");
        if (File.Exists(path)) File.Delete(path);
        File.Copy(Path.Combine(root, $"{name}{ext}"), path);
    }

    static void Run(string filename, string arguments, string working)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = filename,
            Arguments = arguments,
            WorkingDirectory = working
        })!.WaitForExit();
    }

    static string GetTemporaryDirectory()
    {
        var t = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(t);
        return t;
    }
}