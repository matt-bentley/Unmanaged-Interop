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
                //int d = LevenshteinDistanceMax(s1, s1.Length, s2, s2.Length, 1);
                //var d = IsLevenshteinDistanceMax(s1, s1.Length, s2, s2.Length, 1);
                //int d = LevenshteinDistance(s1, s1.Length, s2, s2.Length);
                //int d = LevenshteinDistanceCS(s1, s2);
                //int d = LevenshteinDistanceCS2(s1, s1.Length, s2, s2.Length);
                //int d2 = LevenshteinDistanceCS(s1, s2);
                var d = IsLevenshteinDistanceCSMax(s1, s2, 1);
                //var d = IsLevenshteinDistanceCS2Max(s1, s1.Length, s2, s2.Length, 1);
            }

            sw.Stop();
            Console.WriteLine($"Processed in {sw.ElapsedMilliseconds}ms");

            Console.ReadKey();
        }

        private static int LevenshteinDistanceCS2(string s1, int len1, string s2, int len2)
        {
            int column_start = 1;
            int[] column = new int[len1 + 1];

            // Initialize the distance 'matrix'
            for (var j = 1; j <= len1; j++) column[j] = j;

            for (int x = column_start; x <= len2; x++)
            {
                column[0] = x;
                int last_diagonal = x - column_start;
                for (int y = column_start; y <= len1; y++)
                {
                    int old_diagonal = column[y];
                    column[y] = GetMinOfThree(column[y] + 1, column[y - 1] + 1, last_diagonal + (s1[y - 1] == s2[x - 1] ? 0 : 1));
                    last_diagonal = old_diagonal;
                }
            }
            return column[len1];
        }

        private static bool IsLevenshteinDistanceCS2Max(string s1, int len1, string s2, int len2, int maxDistance)
        {
            int column_start = 1;
            int[] column = new int[len1 + 1];

            // Initialize the distance 'matrix'
            for (var j = 1; j <= len1; j++) column[j] = j;

            for (int x = column_start; x <= len2; x++)
            {
                if (column[x - 1] > maxDistance)
                {
                    return true;
                }
                column[0] = x;
                int last_diagonal = x - column_start;
                for (int y = column_start; y <= len1; y++)
                {
                    int old_diagonal = column[y];
                    column[y] = GetMinOfThree(column[y] + 1, column[y - 1] + 1, last_diagonal + (s1[y - 1] == s2[x - 1] ? 0 : 1));
                    last_diagonal = old_diagonal;
                }
            }
            return column[len1] > maxDistance;
        }

        private static int GetMinOfThree(int val1, int val2, int val3)
        {
            if(val1 < val2)
            {
                if(val1 < val3)
                {
                    return val1;
                }
                else
                {
                    return val3;
                }
            }
            else
            {
                if(val2 < val3)
                {
                    return val2;
                }
                else
                {
                    return val3;
                }
            }
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

        private static bool IsLevenshteinDistanceCSMax(string source, string target, int maxDistance)
        {
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
                if (distance[currentRow, i - 1] > maxDistance)
                {
                    return true;
                }

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
            return distance[currentRow, m] > maxDistance;
        }
    }
}
