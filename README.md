## PublishAotClang
这是一个由 [Zig](https://ziglang.org/) 包装成 Clang 工具链环境的 NuGet 包，用于在 Windows 上辅助 [PublishAot](https://learn.microsoft.com/zh-cn/dotnet/core/deploying/native-aot/) 交叉编译到多个 linux [RID](https://learn.microsoft.com/zh-cn/dotnet/core/rid-catalog)。

本项目源于 [MichalStrehovsky/PublishAotCross](https://github.com/MichalStrehovsky/PublishAotCross)，做了以下额外工作：


| 工作项                          | 说明                                                                               |
| ------------------------------- | ---------------------------------------------------------------------------------- |
| **内置 Zig 工具链**             | 引用 `Vezel.Zig.Toolsets`，Windows 环境下无需单独安装 Zig                          |
| **包含 llvm-objcopy**           | 默认支持 Native AOT 的符号裁剪（strip），减少最终二进制体积                        |
| **内置各 TargetTriple 的 libz** | 解决 .NET 8.0 项目因依赖系统的 `libz` 导致 AOT 发布失败的问题                      |
| **可指定 glibc 版本**           | 例如 `<GLibcVersion>2.17</GLibcVersion>`，.NET 10 项目也可运行于 CentOS 7 等老系统 |
| **优化 clang 包装脚本**         | C# 重写，移除 `-Wl,--export-dynamic`，避免不必要的全局符号导出                     |
| **增加 ar 包装工具**            | C# 编写，支持 `<NativeLib>Static</NativeLib>` 的静态库项目进行 AOT 发布            |

### 如何使用   
1. 在 Native AOT 的项目中，添加对此 [NuGet](https://www.nuget.org/packages/PublishAotClang) 包的引用。

2. 在 Windows 机器上，可以额外 AOT 发布到以下 RID：
* `dotnet publish -r linux-x64`
* `dotnet publish -r linux-arm64`
* `dotnet publish -r linux-arm` (.NET 9+)
* `dotnet publish -r linux-musl-x64`
* `dotnet publish -r linux-musl-arm64`
* `dotnet publish -r linux-musl-arm` (.NET 9+)
