using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BusinessLayerLogic.ExternalProcesses
{
    public class ConsoleAppRunner : IConsoleAppRunner
    {

        public string SpleeterArguments(int noOfStems, string outputPath)
        {
            string Argument;
            switch (noOfStems)
            {
                case 2:
                    Argument = "spleeter separate -p spleeter:2stems -o " + outputPath;
                    Console.WriteLine(Argument);
                    break;
                case 4:
                    Argument = "spleeter separate -p spleeter:4stems -o " + outputPath;
                    Console.WriteLine(Argument);
                    break;
                case 5:
                    Argument = "spleeter separate -p spleeter:5stems -o " + outputPath;
                    Console.WriteLine(Argument);
                    break;
                default:
                    return "INVALID SELECTION";
            }
            return Argument;
        }

        public async Task<string> RunSpleeter(int noOfStems, string originalTrackFilePath, string trackFilePath)
        {
            string Arguments ="/c" + SpleeterArguments(noOfStems, originalTrackFilePath);
            var startInfo = new ProcessStartInfo("cmd.exe",Arguments +" " + trackFilePath);
            startInfo.RedirectStandardOutput = true; 
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = false;

            using (var process = Process.Start(startInfo))
            {
                var standardOutput = new StringBuilder();
                while (!process.HasExited)
                {
                    standardOutput.Append(process.StandardOutput.ReadToEnd());
                }

                standardOutput.Append(process.StandardOutput.ReadToEnd());

                return standardOutput.ToString();
            }
        }
    }
}
