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

        var sw1 = new Stopwatch();
        sw1.Start();
        var ast = AstTransformer.Transform(tree);
        sw1.Stop();
        Console.WriteLine(Time("AST", sw1.Elapsed.TotalMilliseconds));
        
        // Semantics

        try
        {
            var sw2 = new Stopwatch();
            sw2.Start();
            new ScopeBuilder(ast).Visit();
            new TypeResolver(ast).Visit();
            new TypeChecker(ast).Visit();
            sw2.Stop();
            Console.WriteLine(Time("Semantics", sw2.Elapsed.TotalMilliseconds));
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
        
        var sw3 = new Stopwatch();
        sw3.Start();
        var ir = new CodeGenerator().Generate(ast);
        sw3.Stop();
        Console.WriteLine(Time("Code Gen", sw3.Elapsed.TotalMilliseconds));

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
            var sw4 = new Stopwatch();
            sw4.Start();
            Run(llcPath, $"-filetype=obj -opaque-pointers -O0 {name}.ll -o {name}.o", dir);
            Run(clangPath, $"{name}.o -o {name} -lgc", dir);
            sw4.Stop();
            Console.WriteLine(Time("LLVM", sw4.Elapsed.TotalMilliseconds));
            
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
    
    static string Time(string label, double elapsed)
    {
        var e = $"{elapsed:0.00}";
        return $"{label,9}{e,10} ms";
    }
}