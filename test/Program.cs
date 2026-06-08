using System;
using System.IO;
using System.IO.Compression;

// .NET8.0 libz
using var stream = new MemoryStream();
using var zStream = new ZLibStream(stream, CompressionMode.Compress);
zStream.Write([1, 2, 3, 4]);
zStream.Flush();

Console.WriteLine("Hello World");
