## PublishAotClang
这是一个由 [Zig](https://ziglang.org/) 包装成 Clang 工具链环境的 NuGet 包，用于在 Windows 上辅助 [PublishAot](https://learn.microsoft.com/zh-cn/dotnet/core/deploying/native-aot/) 交叉编译到多个 linux [RID](https://learn.microsoft.com/zh-cn/dotnet/core/rid-catalog)。

本项目源于 [MichalStrehovsky/PublishAotCross](https://github.com/MichalStrehovsky/PublishAotCross)，做了以下额外工作：
* Nuget 包含了 llvm-objcopy
* Nuget 引用了 [Vezel.Zig.Toolsets](https://github.com/vezel-dev/zig-toolsets)
* Nuget 包含了 .NET8.0 项目可能需要用到的 libz


### 如何使用   
1. 在 Native AOT 的项目中，添加对此 [NuGet](https://www.nuget.org/packages/PublishAotClang) 包的引用。

2. 在 Windows 机器上，可以额外 AOT 发布到以下 RID：
* `dotnet publish -r linux-x64`
* `dotnet publish -r linux-arm64`
* `dotnet publish -r linux-arm` (.NET 9+)
* `dotnet publish -r linux-musl-x64`
* `dotnet publish -r linux-musl-arm64`
* `dotnet publish -r linux-musl-arm` (.NET 9+)
