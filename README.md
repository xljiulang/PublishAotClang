## PublishAotClang
这是一个由 [Zig](https://ziglang.org/) 包装成 Clang 工具链环境的 NuGet 包，用于在 Windows 上辅助 [PublishAot](https://learn.microsoft.com/zh-cn/dotnet/core/deploying/native-aot/) 交叉编译到多个 linux [RID](https://learn.microsoft.com/zh-cn/dotnet/core/rid-catalog)，本项目源于 [MichalStrehovsky/PublishAotCross](https://github.com/MichalStrehovsky/PublishAotCross)。

### 新特性
| 工作项                  | 说明                                                                                                                         |
| ----------------------- | ---------------------------------------------------------------------------------------------------------------------------- |
| **内置 zig 工具链**     | 引用 [Vezel.Zig.Toolsets](https://github.com/vezel-dev/zig-toolsets)，Windows 环境下无需单独安装 [zig](https://ziglang.org/) |
| **内置 objcopy** 工具   | 默认支持 Native AOT 的符号裁剪（strip），减少最终二进制体积                                                                  |
| **内置 libz 静态库**    | 由 zig build zlib-1.3.2 得到，弥补.NET 8.0 AOT发布时未自带 libz 的问题                                                     |
| **支持 glibc 版本指定** | 例如 `<GLibcVersion>2.17</GLibcVersion>`，.NET 10 项目也可运行于 CentOS 7 等老系统                                           |
| **重写 clang 包装工具** | C# 重写，严格处理 clang 参数，增加移除 `-Wl,--export-dynamic`，避免不必要的全局符号导出                                      |
| **增加 ar 包装工具**    | C# 编写，支持 `<NativeLib>Static</NativeLib>` 的静态库项目进行 AOT 发布                                                      |

### 如何使用   
1. 在 Native AOT 的项目中，添加对此 [NuGet](https://www.nuget.org/packages/PublishAotClang) 包的引用。

2. 在 Windows 机器上，可以额外 AOT 发布到以下 RID：
* `dotnet publish -r linux-x64`
* `dotnet publish -r linux-arm64`
* `dotnet publish -r linux-arm` (.NET 9+)
* `dotnet publish -r linux-musl-x64`
* `dotnet publish -r linux-musl-arm64`
* `dotnet publish -r linux-musl-arm` (.NET 9+)
