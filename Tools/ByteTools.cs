using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcHacker.Tools
{
    public static class ByteTools
    {
        public static byte And(this byte a, byte b) => (byte)(a & b);
        public static bool Compare(this byte[] a, byte[] b)
        {
            for (int i = 0; i < a.Length; i++)
                if (a[i] != b[i])
                    return false;
            return a.Length == b.Length;
        }
    }
}
