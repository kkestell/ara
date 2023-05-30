#region

using System.Diagnostics;
using Ara.Ast;
using Ara.Ast.Errors;
using Ara.Ast.Semantics;
using Ara.CodeGen;
using Ara.Parsing;

#endregion

namespace Ara;

public static class Cli
{
    public static int Run(string filename)
    {
        // Parse

        using var parser = new Parser();
        using var tree = Time("Parsing", () => parser.Parse(File.ReadAllText(filename), filename));

        // AST

        var ast = Time("AST", () => AstTransformer.Transform(tree));

        // Semantics

        try
        {
            Time("Semantics", () =>
            {
                new ScopeBuilder(ast).Visit();
                new TypeResolver(ast).Visit();
                new TypeChecker(ast).Visit();
                new ArrayBoundsChecker(ast).Visit();
            });
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
            return 1;
        }

        // Generate IR

        var ir = Time("CodeGen", () => new CodeGenerator().Generate(ast));

        Debug.WriteLine("");
        Debug.WriteLine("");
        Debug.WriteLine(ir);

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
            var exePath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName ?? throw new InvalidOperationException());

            Time("LLVM", () =>
            {
                Run(llcPath, $"-filetype=obj -opaque-pointers -O0 {name}.ll -o {name}.o", dir);
                Run(clangPath, $"{name}.o -lc -L{exePath} -lara -o {name}", dir);
            });

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

    private static void Copy(string root, string name, string ext = "")
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), $"{name}{ext}");
        if (File.Exists(path)) File.Delete(path);
        File.Copy(Path.Combine(root, $"{name}{ext}"), path);
    }

    private static void Run(string filename, string arguments, string working)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = filename,
            Arguments = arguments,
            WorkingDirectory = working
        })!.WaitForExit();
    }

    private static string GetTemporaryDirectory()
    {
        var t = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(t);
        return t;
    }

    private static T Time<T>(string label, Func<T> func)
    {
        var sw = new Stopwatch();
        sw.Start();
        var result = func.Invoke();
        sw.Stop();
        Elapsed(label, sw);
        return result;
    }

    private static void Time(string label, Action action)
    {
        var sw = new Stopwatch();
        sw.Start();
        action.Invoke();
        sw.Stop();
        Elapsed(label, sw);
    }

    private static void Elapsed(string label, Stopwatch sw)
    {
        var e = $"{sw.Elapsed.TotalMilliseconds:0.00}";
        Console.WriteLine($"{label,9}{e,10} ms");
    }
}