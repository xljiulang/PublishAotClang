## PublishAotZig
这是一个包含 MSBuild 目标的 NuGet 包，用于辅助 [PublishAot](https://learn.microsoft.com/zh-cn/dotnet/core/deploying/native-aot/) 的交叉编译。使用 ZIG 工具链在 Windows 机器上交叉编译到多个 linux RID。

本项目源于 [MichalStrehovsky/PublishAotCross](https://github.com/MichalStrehovsky/PublishAotCross)，做了以下额外工作：
* 补充了部分 zlib.a
* Nuget 包含了 llvm-objcopy
* Nuget 引用了 [zig-toolsets](https://github.com/vezel-dev/zig-toolsets)，你可以使用 ZigVersion 属性指定 zig 的版本，默认是 0.16.0.2



### 如何使用   
1. 在 Native AOT 的项目中，添加对此 [NuGet](https://www.nuget.org/packages/PublishAotZig) 包的引用。

2. 在 Windows 机器上，可以额外 AOT 发布到以下 RID：
* `dotnet publish -r linux-x64`
* `dotnet publish -r linux-arm64`
* `dotnet publish -r linux-arm` (.NET 9+)
* `dotnet publish -r linux-musl-x64`
* `dotnet publish -r linux-musl-arm64`
* `dotnet publish -r linux-musl-arm` (.NET 9+)
