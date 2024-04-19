using System;
using System.Diagnostics;
using System.IO;

namespace Ara.CodeGen.C;

public static class ClangFormatter
{
    public static string Format(string sourceCode)
    {
        string formattedCode = null;

        var startInfo = new ProcessStartInfo
        {
            FileName = "clang-format",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = new Process { StartInfo = startInfo })
        {
            process.Start();

            using (var sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine(sourceCode);
                }
            }

            using (var sr = process.StandardOutput)
            {
                formattedCode = sr.ReadToEnd();
            }
        }

        return formattedCode;
    }
}
