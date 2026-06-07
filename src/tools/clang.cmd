@echo off

set args=%*

rem Work around a .NET 8 Preview 6 issue
set args=%args:'-Wl,-rpath,$ORIGIN'=-Wl,-rpath,$ORIGIN%

rem Work around parameters unsupported by zig. Just drop them from the command line.
set args=%args:--discard-all=--as-needed%
set args=%args:-Wl,-pie =%
set args=%args:-pie =%
set args=%args:-Wl,-e0x0 =%

rem Works around zig linker dropping necessary parts of the executable.
set args=-Wl,-u,__Module %args%

rem Run zig cc
zig cc %args%
