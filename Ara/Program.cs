using System.Diagnostics;
using Ara.IR;
using Ara.Parsing;
using Ara.Parsing.Utils;
using Ara.Semantics;

namespace Ara;

public static class Program
{
    public static int Main()
    {
//         const string source = """
//             struct Foo
//             {
//                 bar: i32;
//             }
//                 
//             struct Test
//             {
//                 x: i32;
//                 foo: Foo;
//             }
//             
//             fn test1(): i32
//             {
//                 return 42;
//             }
//
//             fn test2(): bool
//             {
//                 var x: i32 = 0;
//                 
//                 while (true) 
//                 {
//                     x = x + 1;
//                     
//                     if (x == 10) 
//                     {
//                         break;
//                     }
//                     else
//                     {
//                         continue;
//                     }
//                     
//                     return false;
//                 }
//                 
//                 return true;
//             }
//             
//             fn test3(test: Test): i32
//             {
//                 return test.foo.bar;
//             }
//
//             fn main(): i32
//             {
//                 var y: bool = test2();
//                 
//                 var test: Test;
//                 test.foo.bar = test1();
//                 
//                 if (y == false)
//                 {
//                     return 0;
//                 }
//                 
//                 return test3(test);
//             }
//         """;

        // const string source = """

        // """;

        // const string source = """

        // """;

        // const string source = """

        //
        // """;
        //
//         const string source = """
//             // Implicit casts

//         """;

//         const string source = """

//         """;

//           const string source = """

//           """;

        // try
        // {
            // const string source = """
            // fn main(): i32
            // {
            //     var value: i32 = 100;
            //     var pointer: ^i32 = &value;
            //     var dereferenced_value: i32 = pointer^;
            //     return dereferenced_value;
            // }
            // """;
            //
            // const string source = """
            //     fn main(): void
            //     {
            //         var value: i32;
            //         var pointer: ^i32;
            //         pointer = &value;
            //     }
            // """;

//               const string source = """
                                     
//                                     """;
//             

//             const string source = """

//                 """;

            // var lexer = new Lexer(source);
            // var parser = new Parser(lexer);
            //
            // var root = parser.ParseUnit();
            //
            // var parentSetterVisitor = new ParentSetter();
            // parentSetterVisitor.Visit(root);
            //
            // var ufcsVisitor = new UfcsVisitor();
            // ufcsVisitor.Visit(root);
            //
            // // Console.WriteLine(AstFormatter.Format(root.ToString()));
            //
            // var variableInitializationSplitter = new VariableInitializationSplitter();
            // variableInitializationSplitter.Visit(root);
            //
            // var typeVisitor = new TypeVisitor();
            // typeVisitor.Visit(root);
            //
            // Console.WriteLine(AstFormatter.Format(root.ToString()));
            //
            // var irGenerator = new IrGenerator();
            // var module = irGenerator.Generate(root);
            //
            // Console.WriteLine(module);
            //
            // CompileAndExecute(module.ToString());

            return 0;
        // }
        // catch (ParsingException ex)
        // {
        //     Console.WriteLine(ex.Message);
        // }
        // catch (SemanticException ex)
        // {
        //     Console.WriteLine(ex.Message);
        // }

        // var symbolTableVisitor = new SymbolTableVisitor();
        // symbolTableVisitor.Visit(root);


        //
        // Console.WriteLine(AstFormatter.Format(root.ToString()));
        //


        // catch (ParsingException ex)
        // {
        //     Console.WriteLine(ex.Message);
        // }
        // catch (SemanticException ex)
        // {
        //     Console.WriteLine(ex.Message);
        // }
        
        return 0;
    }

    public static void CompileAndExecute(string llvmIR)
    {
        var tempFilePath = Path.GetTempFileName();
        var llvmFilePath = tempFilePath + ".ll";
        File.WriteAllText(llvmFilePath, llvmIR);

        var executablePath = tempFilePath + ".exe";
        var clangCompile = new ProcessStartInfo("clang", $"{llvmFilePath} -o {executablePath} -lc")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = Process.Start(clangCompile))
        {
            process.WaitForExit();

            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();

            if (process.ExitCode != 0)
            {
                Console.WriteLine("Error during compilation:\n" + error);
                return;
            }
            
            Console.WriteLine("Compilation output:\n" + output);
        }

        var executeBinary = new ProcessStartInfo(executablePath)
        {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = Process.Start(executeBinary))
        {
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            Console.WriteLine("Return code: " + process.ExitCode);
            Console.WriteLine("Execution output:\n" + output);
        }
        
        // File.Delete(tempFilePath);
        // File.Delete(llvmFilePath);
        // File.Delete(executablePath);
        
        Console.WriteLine($"Executable: {executablePath}");
    }
}
