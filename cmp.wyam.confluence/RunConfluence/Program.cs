using System;
using System.Diagnostics;

namespace RunConfluence
{
    class Program
    {
        static void Main(string[] args)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = "cmd.EXE",
                Arguments = "/K D:\\Tools\\Wyam\\Wyam.exe"
            };

            var process = Process.Start(processInfo);
            process.WaitForExit();
        }
    }
}
