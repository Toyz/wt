using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using wt_tool.Commands;

namespace wt_tool
{
    class Program
    {
        private static string windowsTerminalPath = string.Empty;
        private static Terminal.Terminal Terminal;

        public static void Main(string[] args)
        {
            windowsTerminalPath = FindWindowsTermianlSettings();
            if (windowsTerminalPath == string.Empty)
            {
                Console.Error.WriteLine("Windows terminal is not installed!");
                return;
            }

            var types = LoadVerbs();

            Parser.Default.ParseArguments(args, types)
                .WithParsed(Run)
                .WithNotParsed(HandleErrors);
        }

        private static void HandleErrors(IEnumerable<Error> obj)
        {
        }

        private static void Run(object obj)
        {
            Terminal = new Terminal.Terminal(windowsTerminalPath);

            switch (obj)
            {
                case Background bg:
                    bg.Run(Terminal);
                    break;

            }
        }

        private static Type[] LoadVerbs()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttribute<VerbAttribute>() != null).ToArray();
        }


        private static string FindWindowsTermianlSettings()
        {
            var packagesPath = Directory.GetDirectories(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "packages"));

            var terminalPath = packagesPath.FirstOrDefault(x => x.Contains("Microsoft.WindowsTerminal"));
            if (terminalPath == null)
            {
                return string.Empty;
            }

            return Path.Combine(terminalPath, "LocalState", "settings.json");
        }
    }
}
