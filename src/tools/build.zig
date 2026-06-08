const std = @import("std");

// 将 Zig target 映射为 .NET Runtime Identifier (RID)
fn toRuntimeId(query: std.Target.Query) []const u8 {
    const cpu = query.cpu_arch.?;
    const abi = query.abi.?;

    return switch (cpu) {
        .x86_64 => if (abi == .musl) "linux-musl-x64" else "linux-x64",
        .x86 => if (abi == .musl) "linux-musl-x86" else "linux-x86",
        .aarch64 => if (abi == .musl) "linux-musl-arm64" else "linux-arm64",
        .arm => if (abi == .musleabihf) "linux-musl-arm" else "linux-arm",
        else => "unknown",
    };
}

pub fn build(b: *std.Build) void {
    // 多目标交叉编译配置
    const targets: []const std.Target.Query = &.{
        .{ .cpu_arch = .x86_64, .os_tag = .linux, .abi = .gnu },
        .{ .cpu_arch = .x86_64, .os_tag = .linux, .abi = .musl },     
        .{ .cpu_arch = .x86, .os_tag = .linux, .abi = .gnu },
        .{ .cpu_arch = .x86, .os_tag = .linux, .abi = .musl },       
        .{ .cpu_arch = .aarch64, .os_tag = .linux, .abi = .gnu },
        .{ .cpu_arch = .aarch64, .os_tag = .linux, .abi = .musl },      
        .{ .cpu_arch = .arm, .os_tag = .linux, .abi = .gnueabihf },
        .{ .cpu_arch = .arm, .os_tag = .linux, .abi = .musleabihf },
    };

    // 使用 ReleaseSmall 优化以减小体积
    const optimize = std.builtin.OptimizeMode.ReleaseSmall;

    // 为每个目标构建静态库
    for (targets) |query| {
        const target = b.resolveTargetQuery(query);

        // 生成 .NET RID 格式的目标名称（如 linux-x64, linux-musl-arm64）
        const target_str = toRuntimeId(query);

        // zlib 模块
        const zlib_mod = b.createModule(.{
            .target = target,
            .optimize = optimize,
            .link_libc = true,
        });

        // 新 LazyPath API
        zlib_mod.addIncludePath(b.path("zlib"));

        // 新 addCSourceFiles API
        zlib_mod.addCSourceFiles(.{
            .root = b.path("zlib"),
            .files = &[_][]const u8{
                "adler32.c",
                "compress.c",
                "crc32.c",
                "deflate.c",
                "infback.c",
                "inffast.c",
                "inflate.c",
                "inftrees.c",
                "trees.c",
                "uncompr.c",
                "zutil.c",
            },
            .flags = &.{},
        });

        // 静态库
        const zlib = b.addLibrary(.{
            .name = "z",
            .root_module = zlib_mod,
            .linkage = .static,
        });

        // 按目标分别安装到不同目录
        const install_step = b.addInstallArtifact(zlib, .{
            .dest_dir = .{
                .override = .{
                    .custom = target_str,
                },
            },
        });
        b.getInstallStep().dependOn(&install_step.step);

        // 为每个目标安装 zlib.h 头文件
        const install_header = b.addInstallFile(
            b.path("zlib/zlib.h"),
            b.fmt("{s}/zlib.h", .{target_str}),
        );
        b.getInstallStep().dependOn(&install_header.step);
    }
}