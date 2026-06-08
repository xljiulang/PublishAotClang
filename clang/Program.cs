using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Clang;

sealed class Program
{
    // 需要丢弃的参数
    private static readonly HashSet<string> _argumentsToSkip =
    [
        "-pie",
        "-Wl,-pie",
        "-Wl,-e0x0",
    ];

    // 需要转换的参数映射
    private static readonly Dictionary<string, string> _argumentReplacements = new()
    {
        ["--discard-all"] = "--as-needed",                 // zig 不支持的参数替换
        ["'-Wl,-rpath,$ORIGIN'"] = "-Wl,-rpath,$ORIGIN",   // 解决 .NET 8 Preview 6 的问题            
    };

    public static int Main(string[] args)
    {
        var zigArgs = GetZigArguments(args);
        return RunZigCompiler(zigArgs);
    }

    private static string GetZigArguments(string[] args)
    {
        var zigArgs = new List<string>
        {
            // 解决 zig 链接器丢弃可执行文件必要部分的问题
            "-Wl,-u,__Module"
        };

        foreach (var arg in args)
        {
            // 检查是否需要跳过
            if (_argumentsToSkip.Contains(arg))
            {
                continue;
            }

            // 检查是否需要转换
            if (_argumentReplacements.TryGetValue(arg, out var replacement))
            {
                zigArgs.Add(replacement);
                continue;
            }

            // 保留所有其他参数
            zigArgs.Add(arg);
        }

        return string.Join(" ", zigArgs);
    }

    private static int RunZigCompiler(string arguments)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "zig",
            Arguments = $"cc {arguments}",
            UseShellExecute = false,
            RedirectStandardOutput = false,
            RedirectStandardError = false
        };

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
