## PublishAotClang
这是一个由 [Zig](https://ziglang.org/) 包装成 Clang 工具链环境的 NuGet 包，用于在 Windows 上辅助 [PublishAot](https://learn.microsoft.com/zh-cn/dotnet/core/deploying/native-aot/) 交叉编译到多个 linux [RID](https://learn.microsoft.com/zh-cn/dotnet/core/rid-catalog)。

本项目源于 [MichalStrehovsky/PublishAotCross](https://github.com/MichalStrehovsky/PublishAotCross)，做了以下额外工作：
* [x] 集成了 llvm-objcopy，支持 `<StripSymbols>`
* [x] 集成了 [Vezel.Zig.Toolsets](https://github.com/vezel-dev/zig-toolsets)，不需要额外安装 Zig
* [x] 集成了 .NET8.0 压缩库依赖的 libz
* [ ] `<StaticICULinking>`待支持
* [ ] `<StaticOpenSslLinking>`待支持

### 如何使用   
1. 在 Native AOT 的项目中，添加对此 [NuGet](https://www.nuget.org/packages/PublishAotClang) 包的引用。

2. 在 Windows 机器上，可以额外 AOT 发布到以下 RID：
* `dotnet publish -r linux-x64`
* `dotnet publish -r linux-arm64`
* `dotnet publish -r linux-arm` (.NET 9+)
* `dotnet publish -r linux-musl-x64`
* `dotnet publish -r linux-musl-arm64`
* `dotnet publish -r linux-musl-arm` (.NET 9+)
