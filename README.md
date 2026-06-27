## PublishAotClang
这是一个由 [Zig](https://ziglang.org/) 包装成 Clang 工具链环境的 NuGet 包，用于在 Windows 上辅助 [PublishAot](https://learn.microsoft.com/zh-cn/dotnet/core/deploying/native-aot/) 交叉编译到多个 linux [RID](https://learn.microsoft.com/zh-cn/dotnet/core/rid-catalog)。

本项目源于 [MichalStrehovsky/PublishAotCross](https://github.com/MichalStrehovsky/PublishAotCross)，做了以下额外工作：
* Nuget 包含了 llvm-objcopy，默认支持文件符号裁剪
* Nuget 引用了 [Vezel.Zig.Toolsets](https://github.com/vezel-dev/zig-toolsets)，无需在 Windows 上安装 Zig
* Nuget 包含了 .NET8.0 项目可能需要用到的 libz，避免依赖 libz 的项目 Aot 发布失败
* 支持`<GLibcVersion>`指定glibc版本，例如`2.17`，.Net10.0项目也能在老旧的 centos7 上运行
* 重写了 clang 工具，移除了 -Wl,--export-dynamic 参数，避免导出所有符号
* 增加了 ar 工具，支持`<NativeLib>Static</NativeLib>`的静态库项目 Aot 发布

### 如何使用   
1. 在 Native AOT 的项目中，添加对此 [NuGet](https://www.nuget.org/packages/PublishAotClang) 包的引用。

2. 在 Windows 机器上，可以额外 AOT 发布到以下 RID：
* `dotnet publish -r linux-x64`
* `dotnet publish -r linux-arm64`
* `dotnet publish -r linux-arm` (.NET 9+)
* `dotnet publish -r linux-musl-x64`
* `dotnet publish -r linux-musl-arm64`
* `dotnet publish -r linux-musl-arm` (.NET 9+)
