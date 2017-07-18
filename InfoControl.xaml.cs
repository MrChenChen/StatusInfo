using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace MyVSTool
{
    public partial class InfoControl : UserControl
    {
        public static readonly string[] Formats;

        public static readonly string[] FormatDescriptions;

        private readonly Dictionary<string, TextBlockList> textBlockLists;

        private readonly long totalRam;

        private int fixedWidth = 150;

        private bool useFixedWidth;

        public int CpuUsage
        {
            set
            {
                string CpuValue = string.Format("{0,2}%", value);
                textBlockLists["<CPU>"].Text = CpuValue;
                textBlockLists["<#CPU>"].Text = CpuValue;
                textBlockLists["<#CPU>"].Foreground = GetCpuColor(value);
            }
        }

        public int FixedWidth
        {
            get
            {
                return fixedWidth;
            }
            set
            {
                fixedWidth = value;
                UseFixedWidth = UseFixedWidth;
            }
        }

        public string Format
        {
            set
            {
                stackPanel.Children.Clear();
                textBlockLists.Clear();
                InitTextBlockLists();
                string str = value;
                while (str != "")
                {
                    TextBlockList textBlockList;
                    TextBlock nextTextBlock = GetNextTextBlock(ref str, out textBlockList);
                    textBlockList?.Add(nextTextBlock);
                    stackPanel.Children.Add(nextTextBlock);
                }
                foreach (TextBlockList textBlockList1 in textBlockLists.Values)
                {
                    textBlockList1.Text = "N/A";
                }
            }
        }

        public long FreeRam
        {
            set
            {
                long num = totalRam - value;
                string readableByteSize = num.ToReadableByteSize("####.00");

                textBlockLists["<TOTAL_USE_RAM>"].Text = readableByteSize;
                textBlockLists["<#TOTAL_USE_RAM>"].Text = readableByteSize;
                textBlockLists["<#TOTAL_USE_RAM>"].Foreground = GetRamColor(num);

                int num1 = (int)(num * 100 / totalRam);
                readableByteSize = string.Format("{0:####.00}%", num1);
                textBlockLists["<TOTAL_USE_RAM%>"].Text = readableByteSize;
                textBlockLists["<#TOTAL_USE_RAM%>"].Text = readableByteSize;
                textBlockLists["<#TOTAL_USE_RAM%>"].Foreground = GetRamColor(num);

                readableByteSize = value.ToReadableByteSize("####.00");
                textBlockLists["<FREE_RAM>"].Text = readableByteSize;
                textBlockLists["<#FREE_RAM>"].Text = readableByteSize;
                textBlockLists["<#FREE_RAM>"].Foreground = GetRamColor(num);

                num1 = (int)(value * 100 / totalRam);
                readableByteSize = string.Format("{0:####.00}%", num1);
                textBlockLists["<FREE_RAM%>"].Text = readableByteSize;
                textBlockLists["<#FREE_RAM%>"].Text = readableByteSize;
                textBlockLists["<#FREE_RAM%>"].Foreground = GetRamColor(num);
            }
        }

        public long RamUsage
        {
            set
            {
                string readableByteSize = value.ToReadableByteSize("####.00");
                textBlockLists["<RAM>"].Text = readableByteSize;
                textBlockLists["<#RAM>"].Text = readableByteSize;
                textBlockLists["<#RAM>"].Foreground = GetRamColor(value);

                int num = (int)(value * 100 / totalRam);
                readableByteSize = string.Format("{0:####.00}%", num);
                textBlockLists["<RAM%>"].Text = readableByteSize;
                textBlockLists["<#RAM%>"].Text = readableByteSize;
                textBlockLists["<#RAM%>"].Foreground = GetCpuColor(num);
            }
        }

        public int TotalCpuUsage
        {
            set
            {
                string TotalCpuValue = string.Format("{0,2}%", value);
                textBlockLists["<TOTAL_CPU>"].Text = TotalCpuValue;
                textBlockLists["<#TOTAL_CPU>"].Text = TotalCpuValue;
                textBlockLists["<#TOTAL_CPU>"].Foreground = GetCpuColor(value);
            }
        }

        public bool UseFixedWidth
        {
            get
            {
                return useFixedWidth;
            }
            set
            {
                useFixedWidth = value;
                if (!useFixedWidth)
                {
                    Width = double.NaN;
                    return;
                }
                Width = FixedWidth;
            }
        }

        static InfoControl()
        {
            string[] strArrays = { "CPU", "TOTAL_CPU", "RAM", "FREE_RAM", "TOTAL_USE_RAM", "RAM%", "FREE_RAM%", "TOTAL_USE_RAM%" };
            Formats = strArrays;
            string[] strArrayDescriptions = { "Cpu usage of Visual Studio", "Cpu usage of computer", "Ram usage of Visual Studio", "Free ram of computer", "Ram usage of computer", "Ram usage of Visual Studio in percent", "Free ram of computer in percent", "Ram usage of computer in percent" };
            FormatDescriptions = strArrayDescriptions;
        }

        public InfoControl(long pTotalRam)
        {
            totalRam = pTotalRam;
            InitializeComponent();
            textBlockLists = new Dictionary<string, TextBlockList>();
            Format = "Loading MyVSTool...";
        }

        private Brush GetCpuColor(int cpu)
        {
            Color color;
            if (cpu > 50)
            {
                Color yellow = Colors.Yellow;
                color = yellow.FadeTo(Colors.Red, (cpu - 50) / 50f);
            }
            else
            {
                Color white = Colors.White;
                color = white.FadeTo(Colors.Yellow, cpu / 50f);
            }
            return new SolidColorBrush(color);
        }

        private TextBlock GetNextTextBlock(ref string format, out TextBlockList textBlockList)
        {
            string str;
            TextBlock textBlock = new TextBlock
            {
                Foreground = new SolidColorBrush(Colors.White)
            };
            TextBlock textBlock1 = textBlock;
            int num = format.IndexOfAny(textBlockLists.Keys.ToArray(), out str);
            if (num == -1)
            {
                textBlock1.Text = format;
                format = "";
                textBlockList = null;
            }
            else if (num != 0)
            {
                textBlock1.Text = format.Substring(0, num);
                format = format.Substring(num);
                textBlockList = null;
            }
            else
            {
                textBlock1.Text = "";
                format = format.Substring(str.Length);
                textBlockList = textBlockLists[str];
            }
            return textBlock1;
        }

        private Brush GetRamColor(long ram)
        {
            int num = (int)(ram * 100 / totalRam);
            return GetCpuColor(num);
        }

        private void InitTextBlockLists()
        {
            string[] formats = Formats;
            foreach (string t in formats)
            {
                textBlockLists[string.Format("<{0}>", t)] = new TextBlockList();
                textBlockLists[string.Format("<#{0}>", t)] = new TextBlockList();
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            IVsSolution solution = (IVsSolution)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(IVsSolution));

            SolutionBuild sb = StatusInfoPackage.GlobalDte.Solution.SolutionBuild;

            var sc = sb.StartupProjects as object[];

            var startupname = (sc == null || sc.Length == 0) ? null : sc.Select(p => p.ToString()).ToList();

            if (startupname == null)
                foreach (Project project in MyTool.GetProjects(solution))
                {
                    Directory.GetParent(project.FullName).FullName.OpenExplorerPath();
                    break;
                }
            else
                foreach (Project project in MyTool.GetProjects(solution))
                {
                    foreach (var item in startupname)
                    {
                        if (project.FullName.EndsWith(item))
                        {
                            Directory.GetParent(project.FullName).FullName.OpenExplorerPath();
                        }
                    }
                }

        }




    }


    public static class MyTool
    {
        public static void OpenExplorerPath(this string path)
        {
            System.Diagnostics.Process.Start("explorer.exe", path);
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