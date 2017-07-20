using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace System.Windows.Media
{
    public static class ColorExtension
    {
        public static Color FadeTo(this Color color, Color toColor, float percent)
        {
            return Color.FromArgb((byte)Lerp(color.A, toColor.A, percent), (byte)Lerp(color.R, toColor.R, percent), (byte)Lerp(color.G, toColor.G, percent), (byte)Lerp(color.B, toColor.B, percent));
        }

        private static float Lerp(float v0, float v1, float t)
        {
            return v0 + (v1 - v0) * t;
        }
    }
}

namespace System
{
    public static class Int64Extension
    {
        public static string ToReadableByteSize(this long @long)
        {
            return @long.ToReadableByteSize("0.## ");
        }

        public static string ToReadableByteSize(this long @long, string format)
        {
            string str;
            string str1 = (@long < (long)0 ? "-" : string.Empty);
            double num;
            if (@long >= 1152921504606846976L)
            {
                str = "EB";
                num = @long >> 50;
            }
            else if (@long >= 1125899906842624L)
            {
                str = "PB";
                num = @long >> 40;
            }
            else if (@long >= 1099511627776L)
            {
                str = "TB";
                num = @long >> 30;
            }
            else if (@long >= 1073741824)
            {
                str = "GB";
                num = @long >> 20;
            }
            else if (@long < 1048576)
            {
                if (@long < 1024)
                {
                    return @long.ToString(string.Concat(str1, "0 B"));
                }
                str = "KB";
                num = @long;
            }
            else
            {
                str = "MB";
                num = @long >> 10;
            }
            num = num / 1024;
            return string.Concat(str1, num.ToString(format), str);
        }
    }

    public static class StringExtension
    {
        public static int IndexOfAny(this string @string, char[] anyOf, out char? foundChar)
        {
            return @string.IndexOfAny(anyOf, 0, out foundChar);
        }

        public static int IndexOfAny(this string @string, char[] anyOf, int startIndex, out char? foundChar)
        {
            return @string.IndexOfAny(anyOf, startIndex, @string.Length - startIndex, out foundChar);
        }

        public static int IndexOfAny(this string @string, char[] anyOf, int startIndex, int count, out char? foundChar)
        {
            foundChar = null;
            int num = @string.IndexOfAny(anyOf, startIndex, count);
            if (num > -1)
            {
                bool flag = false;
                for (int i = 0; i < anyOf.Length && !flag; i++)
                {
                    char? nullable = anyOf[i];
                    char? nullable1 = nullable;
                    foundChar = nullable;
                    char? nullable2 = nullable1;
                    int str = @string[num];
                    flag = (nullable2.GetValueOrDefault() == (char)str);
                }
            }
            return num;
        }

        public static int IndexOfAny(this string @string, String[] anyOf)
        {
            string str;
            return @string.IndexOfAny(anyOf, out str);
        }

        public static int IndexOfAny(this string @string, String[] anyOf, out string foundString)
        {
            return @string.IndexOfAny(anyOf, 0, out foundString);
        }

        public static int IndexOfAny(this string @string, String[] anyOf, int startIndex)
        {
            string str;
            return @string.IndexOfAny(anyOf, startIndex, out str);
        }

        public static int IndexOfAny(this string @string, String[] anyOf, int startIndex, out string foundString)
        {
            return @string.IndexOfAny(anyOf, startIndex, @string.Length - startIndex, out foundString);
        }

        public static int IndexOfAny(this string @string, String[] anyOf, int startIndex, int count)
        {
            string str;
            return @string.IndexOfAny(anyOf, startIndex, count, out str);
        }

        public static int IndexOfAny(this string @string, String[] anyOf, int startIndex, int count, out string foundString)
        {
            Dictionary<string, int> strs = new Dictionary<string, int>();
            String[] strArrays = anyOf;

            foreach (string str in strArrays)
            {
                int num = @string.IndexOf(str, startIndex, count, StringComparison.Ordinal);
                if (num > -1)
                {
                    strs[str] = num;
                }
            }
            if (!strs.Any())
            {
                foundString = null;
                return -1;
            }
            KeyValuePair<string, int> keyValuePair = strs.ElementAt(0);
            foundString = keyValuePair.Key;
            return keyValuePair.Value;
        }

        public static int LastIndexOfAny(this string @string, char[] anyOf, out char? foundChar)
        {
            return @string.LastIndexOfAny(anyOf, @string.Length - 1, out foundChar);
        }

        public static int LastIndexOfAny(this string @string, char[] anyOf, int startIndex, out char? foundChar)
        {
            return @string.LastIndexOfAny(anyOf, startIndex, startIndex + 1, out foundChar);
        }

        public static int LastIndexOfAny(this string @string, char[] anyOf, int startIndex, int count, out char? foundChar)
        {
            foundChar = null;
            int num = @string.LastIndexOfAny(anyOf, startIndex, count);
            if (num > -1)
            {
                bool flag = false;
                for (int i = 0; i < anyOf.Length && !flag; i++)
                {
                    char? nullable = anyOf[i];
                    char? nullable1 = nullable;
                    foundChar = nullable;
                    char? nullable2 = nullable1;
                    int str = @string[num];
                    flag = (nullable2.GetValueOrDefault() == (char)str);
                }
            }
            return num;
        }

        public static int LastIndexOfAny(this string @string, String[] anyOf)
        {
            string str;
            return @string.LastIndexOfAny(anyOf, out str);
        }

        public static int LastIndexOfAny(this string @string, String[] anyOf, out string foundString)
        {
            return @string.LastIndexOfAny(anyOf, @string.Length - 1, out foundString);
        }

        public static int LastIndexOfAny(this string @string, String[] anyOf, int startIndex)
        {
            string str;
            return @string.LastIndexOfAny(anyOf, startIndex, out str);
        }

        public static int LastIndexOfAny(this string @string, String[] anyOf, int startIndex, out string foundString)
        {
            return @string.LastIndexOfAny(anyOf, startIndex, startIndex + 1, out foundString);
        }

        public static int LastIndexOfAny(this string @string, String[] anyOf, int startIndex, int count)
        {
            string str;
            return @string.LastIndexOfAny(anyOf, startIndex, count, out str);
        }

        public static int LastIndexOfAny(this string @string, String[] anyOf, int startIndex, int count, out string foundString)
        {
            Dictionary<string, int> strs = new Dictionary<string, int>();
            String[] strArrays = anyOf;
            foreach (string str in strArrays)
            {
                int num = @string.LastIndexOf(str, startIndex, count, StringComparison.Ordinal);
                if (num > -1)
                {
                    strs[str] = num;
                }
            }
            if (!strs.Any())
            {
                foundString = null;
                return -1;
            }
            KeyValuePair<string, int> keyValuePair = strs.ElementAt(0);
            foundString = keyValuePair.Key;
            return keyValuePair.Value;
        }
    }
}

namespace System.Diagnostics
{
    public static class ProcessExtension
    {
        private static Dictionary<int, DateTime> cpuCheckTimes;

        private static Dictionary<int, TimeSpan> cpuTimes;

        static ProcessExtension()
        {
            cpuTimes = new Dictionary<int, TimeSpan>();
            cpuCheckTimes = new Dictionary<int, DateTime>();
        }

        public static double GetCpuUsage(this Process process)
        {
            process.Refresh();
            if (!cpuTimes.ContainsKey(process.Id))
            {
                process.InitCpuUsage();
            }
            TimeSpan totalProcessorTime = process.TotalProcessorTime;
            DateTime utcNow = DateTime.UtcNow;
            TimeSpan item = totalProcessorTime - cpuTimes[process.Id];
            double totalSeconds = item.TotalSeconds;
            TimeSpan timeSpan = utcNow - cpuCheckTimes[process.Id];
            double num = timeSpan.TotalSeconds;
            double processorCount = totalSeconds / (Environment.ProcessorCount * num);
            cpuTimes[process.Id] = totalProcessorTime;
            cpuCheckTimes[process.Id] = utcNow;
            return processorCount;
        }

        public static void InitCpuUsage(this Process process)
        {
            cpuTimes[process.Id] = process.TotalProcessorTime;
            cpuCheckTimes[process.Id] = DateTime.UtcNow;
        }
    }
}


namespace MyVSTool
{
    public static class MyTool
    {
        public static void OpenExplorerPath(this string path)
        {
            if (Directory.Exists(path))
            {
                Process.Start("explorer.exe", path);
            }
            else
            {
                MessageBox.Show("Cannot Open: " + path, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static IEnumerable<EnvDTE.Project> GetProjects(IVsSolution solution)
        {
            foreach (IVsHierarchy hier in GetProjectsInSolution(solution))
            {
                EnvDTE.Project project = GetDTEProject(hier);
                if (project != null)
                    yield return project;
            }
        }

        public static IEnumerable<IVsHierarchy> GetProjectsInSolution(IVsSolution solution)
        {
            return GetProjectsInSolution(solution, __VSENUMPROJFLAGS.EPF_LOADEDINSOLUTION);
        }

        public static IEnumerable<IVsHierarchy> GetProjectsInSolution(IVsSolution solution, __VSENUMPROJFLAGS flags)
        {
            if (solution == null)
                yield break;

            IEnumHierarchies enumHierarchies;
            Guid guid = Guid.Empty;
            solution.GetProjectEnum((uint)flags, ref guid, out enumHierarchies);
            if (enumHierarchies == null)
                yield break;

            IVsHierarchy[] hierarchy = new IVsHierarchy[1];
            uint fetched;
            while (enumHierarchies.Next(1, hierarchy, out fetched) == VSConstants.S_OK && fetched == 1)
            {
                if (hierarchy.Length > 0 && hierarchy[0] != null)
                    yield return hierarchy[0];
            }
        }

        public static EnvDTE.Project GetDTEProject(IVsHierarchy hierarchy)
        {
            if (hierarchy == null)
                throw new ArgumentNullException("hierarchy");

            object obj;
            hierarchy.GetProperty(VSConstants.VSITEMID_ROOT, (int)__VSHPROPID.VSHPROPID_ExtObject, out obj);
            return obj as EnvDTE.Project;
        }
    }

}