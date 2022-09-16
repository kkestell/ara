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
        using var tree = parser.Parse(File.ReadAllText(filename));
        
        // AST
        
        var ast = AstTransformer.Transform(tree);
        
        // Semantics

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
        
        Run("/usr/local/opt/llvm/bin/llc", $"-filetype=obj -opaque-pointers {name}.ll -o {name}.o", dir);
        Run("/usr/local/opt/llvm/bin/clang", $"{name}.o -o {name}", dir);
        Copy(dir, name);

        // Make AST graph
        
        new GraphGenerator().Generate(ast, Path.Combine(dir, $"{name}.dot"));
        Run("dot", $"-Tpdf {name}.dot -o {name}.pdf", dir);
        Copy(dir, name, ".pdf");
        
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