
using System;
using System.Collections.Generic;
using System.Diagnostics;

public static class ZigCompiler
{
    public static int Run(IEnumerable<string> args)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "zig",
            UseShellExecute = false,
            RedirectStandardOutput = false,
            RedirectStandardError = false
        };

        foreach (var arg in args)
        {
            startInfo.ArgumentList.Add(arg);
        }

        Console.WriteLine($"zig {string.Join(" ", startInfo.ArgumentList)}");

        try
        {
            using var process = Process.Start(startInfo);
            if (process == null)
            {
                Console.Error.WriteLine("Error: Failed to start zig compiler");
                return 1;
            }

            process.WaitForExit();
            return process.ExitCode;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: Failed to execute zig compiler - {ex.Message}");
            return 1;
        }
    }
}