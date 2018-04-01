using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;

namespace NetCoreUnmanagedInterop
{
    class Program
    {
        [SuppressUnmanagedCodeSecurity]
        [DllImport(@"C:\Repo\UnmanagedInterop\NetCoreUnmanagedInterop\x64\Release\CppInterop.dll", EntryPoint =
        "levenshtein_distance", CallingConvention = CallingConvention.StdCall)]
        public static extern int LevenshteinDistance([MarshalAs(UnmanagedType.LPWStr)] string s1, int len1, [MarshalAs(UnmanagedType.LPWStr)] string s2, int len2);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(@"C:\Repo\UnmanagedInterop\NetCoreUnmanagedInterop\x64\Release\CppInterop.dll", EntryPoint =
        "levenshtein_distance_max", CallingConvention = CallingConvention.StdCall)]
        public static extern int LevenshteinDistanceMax([MarshalAs(UnmanagedType.LPWStr)] string s1, int len1, [MarshalAs(UnmanagedType.LPWStr)] string s2, int len2, int maxDistance);

        [SuppressUnmanagedCodeSecurity]
        [DllImport(@"C:\Repo\UnmanagedInterop\NetCoreUnmanagedInterop\x64\Release\CppInterop.dll", EntryPoint =
        "is_levenshtein_distance_max", CallingConvention = CallingConvention.StdCall)]
        public static extern InteropBool IsLevenshteinDistanceMax([MarshalAs(UnmanagedType.LPWStr)] string s1, int len1, [MarshalAs(UnmanagedType.LPWStr)] string s2, int len2, int maxDistance);

        static void Main(string[] args)
        {
            //int result = Add("Hello", "Hellp");
            //Console.WriteLine("result is {0}", result);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            string s1 = "Helfsmlj.slo";
            string s2 = "Hcdsjncellp";

            for (int i = 0; i < 100000000; i++)
            {
                int d = LevenshteinDistanceMax(s1, s1.Length, s2, s2.Length, 1);
                //var d = IsLevenshteinDistanceMax(s1, s1.Length, s2, s2.Length, 1);
                //int d = LevenshteinDistance(s1, s1.Length, s2, s2.Length);
                //int d = LevenshteinDistanceCS(s1, s2);
            }

            sw.Stop();
            Console.WriteLine($"Processed in {sw.ElapsedMilliseconds}ms");

            Console.ReadKey();
        }

        private static int LevenshteinDistanceCS(string source, string target)
        {
            if (String.IsNullOrEmpty(source))
            {
                if (String.IsNullOrEmpty(target)) return 0;
                return target.Length;
            }
            if (String.IsNullOrEmpty(target)) return source.Length;

            if (source.Length > target.Length)
            {
                var temp = target;
                target = source;
                source = temp;
            }

            var m = target.Length;
            var n = source.Length;
            var distance = new int[2, m + 1];
            // Initialize the distance 'matrix'
            for (var j = 1; j <= m; j++) distance[0, j] = j;

            var currentRow = 0;
            for (var i = 1; i <= n; ++i)
            {
                currentRow = i & 1;
                distance[currentRow, 0] = i;
                var previousRow = currentRow ^ 1;
                for (var j = 1; j <= m; j++)
                {
                    var cost = (target[j - 1] == source[i - 1] ? 0 : 1);
                    distance[currentRow, j] = Math.Min(Math.Min(
                                distance[previousRow, j] + 1,
                                distance[currentRow, j - 1] + 1),
                                distance[previousRow, j - 1] + cost);
                }
            }
            return distance[currentRow, m];
        }
    }
}
