using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

    public static int Main(string[] clangArgs)
    {
        var zigArgs = GetZigArguments(clangArgs);
        return RunZigCompiler(zigArgs);
    }

    private static IEnumerable<string> GetZigArguments(string[] clangArgs)
    {
        yield return "cc";

        // 解决 zig 链接器丢弃可执行文件必要部分的问题
        const string ModuleArg = "-Wl,-u,__Module";
        if (!clangArgs.Contains(ModuleArg))
        {
            yield return ModuleArg;
        }

        const string TargetPrefix = "--target=";
        var lastTargetIndex = Array.FindLastIndex(clangArgs, arg => arg.StartsWith(TargetPrefix));

        for (var index = 0; index < clangArgs.Length; index++)
        {
            var clangArg = clangArgs[index];

            // 检查是否需要跳过
            if (_argumentsToSkip.Contains(clangArg))
            {
                continue;
            }

            // 跳过非最后一个 --target= 参数
            if (clangArg.StartsWith(TargetPrefix) && index != lastTargetIndex)
            {
                continue;
            }

            // 检查是否需要转换
            if (_argumentReplacements.TryGetValue(clangArg, out var zigArg))
            {
                yield return zigArg;
            }
            else
            {
                // 保留所有其他参数
                yield return clangArg;
            }
        }
    }

    private static int RunZigCompiler(IEnumerable<string> args)
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
