using System.Linq;

namespace Ar;

sealed class Program
{
    public static int Main(string[] args)
    {
        return ZigCompiler.Run(args.Prepend("ar"));
    }
}