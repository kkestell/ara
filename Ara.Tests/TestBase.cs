using System.Diagnostics;
using Ara.IR;
using Ara.Parsing;
using Ara.Parsing.Utils;
using Ara.Semantics;

namespace Ara.Tests;

public record ExecutionResult(int ExitCode, string Output);

public class TestBase
{
    protected ExecutionResult CompileAndExecute(string source)
    {
        const string prelude = 
            """
            extern fn putchar(c: i32): i32
            extern fn malloc(size: i64): ^void
            extern fn free(ptr: ^void): void
            
            """;
        source = prelude + source;
        
        var lexer = new Lexer(source);
        var parser = new Parser(lexer);

        var root = parser.ParseUnit();

        var parentSetterVisitor = new ParentSetter();
        parentSetterVisitor.Visit(root);

        var ufcsVisitor = new UfcsVisitor();
        ufcsVisitor.Visit(root);

        // Console.WriteLine(AstFormatter.Format(root.ToString()));

        var typeVisitor = new TypeVisitor();
        typeVisitor.Visit(root);

        // Console.WriteLine(AstFormatter.Format(root.ToString()));

        var irGenerator = new IrGenerator();
        var module = irGenerator.Generate(root);

        Console.WriteLine(module);

        var tempFilePath = Path.GetTempFileName();
        var llvmFilePath = tempFilePath + ".ll";
        File.WriteAllText(llvmFilePath, module.ToString());

        var executablePath = tempFilePath + ".exe";
        var clangCompile = new ProcessStartInfo("clang", $"{llvmFilePath} -o {executablePath} -lc")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var clangProcess = Process.Start(clangCompile);
        clangProcess.WaitForExit();

        var clangOutput = clangProcess.StandardOutput.ReadToEnd();
        var clangError = clangProcess.StandardError.ReadToEnd();

        Debug.WriteLine("Compilation output: " + clangOutput);
        Debug.WriteLine("Compilation error: " + clangError);

        if (clangProcess.ExitCode != 0)
        {
            throw new Exception(clangError);
        }

        var executeBinary = new ProcessStartInfo(executablePath)
        {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var binaryProcess = Process.Start(executeBinary);
        var binaryOutput = binaryProcess.StandardOutput.ReadToEnd();

        binaryProcess.WaitForExit();
        Debug.WriteLine($"Executable: {executablePath}");
        Debug.WriteLine("Executable return code: " + binaryProcess.ExitCode);
        Debug.WriteLine("Executable output: " + binaryOutput);

        File.Delete(tempFilePath);
        File.Delete(llvmFilePath);
        File.Delete(executablePath);

        return new ExecutionResult(binaryProcess.ExitCode, binaryOutput);
    }
}