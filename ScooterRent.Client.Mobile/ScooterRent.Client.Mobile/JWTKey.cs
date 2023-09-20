using System;
using System.Collections.Generic;
using System.Text;

namespace App2
{
    internal static class JWTKey
    {
        public static string Key { get; private set; }
        public static void SetKey(string k)
        {
            Key = k;
        }
    }
}
