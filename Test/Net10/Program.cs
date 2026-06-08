using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Net10;

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);

        var app = builder.Build();
        app.MapGet("/", context => context.Response.WriteAsync("Hello World"));
        app.Run();
    }
}