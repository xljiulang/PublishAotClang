using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Net9
{
    public static class Math
    {
        [UnmanagedCallersOnly(EntryPoint = "sub", CallConvs = [typeof(CallConvCdecl)])]
        public static int Sub(int x, int y)
        {
            return x - y;
        }
    }
}
