## PublishAotZig
这是一个包含 MSBuild 目标的 NuGet 包，用于辅助 [PublishAot](https://learn.microsoft.com/zh-cn/dotnet/core/deploying/native-aot/) 的交叉编译。使用 ZIG 工具链在 Windows 机器上交叉编译到 `linux-x64`、`linux-arm64`、`linux-musl-x64`、`linux-musl-arm64`。


本项目源于 [MichalStrehovsky/PublishAotCross](https://github.com/MichalStrehovsky/PublishAotCross)，做了以下额外工作：
* 补齐 zlib.a，使依赖于 System.IO.Compression 的项目（例如 ASP.NET Core）能直接 Aot。
* Nuget 包自带了 llvm-objcopy 工具，用于去除符号使生成的可执行文件更小。


### 环境准备
在你的 Windows 主机 [下载](https://ziglang.org/download/) Zig 压缩包，解压然后将其完整目录设置到 PATH 环境变量中。例如解压后路径是 `D:\zig-windows-x86_64-0.13.0\zig.exe`，PATH 值为 `D:\zig-windows-x86_64-0.13.0`。

### 如何使用   
1. 在 Native AOT 的项目中，添加对此 [NuGet](https://www.nuget.org/packages/PublishAotZig) 包的引用。

2. 发布到以下的 RID 之一：
* `dotnet publish -r linux-x64`
* `dotnet publish -r linux-arm64`
* `dotnet publish -r linux-musl-x64`
* `dotnet publish -r linux-musl-arm64`

