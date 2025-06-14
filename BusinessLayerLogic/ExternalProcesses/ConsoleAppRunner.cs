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

        public string SpleeterArguments(int noOfStems)
        {
            var Argument = string.Empty;
            switch (noOfStems)
            {
                case 2:
                    Argument = "spleeter separate -p spleeter:2stems -o \"C:\\Users\\sukum\\OneDrive\\Documents\" ";
                    break;
                case 4:
                    Argument = "spleeter separate -p spleeter:4stems -o \"C:\\Users\\sukum\\OneDrive\\Documents\" ";
                    break;
                case 5:
                    Argument = "spleeter separate -p spleeter:5stems -o \"C:\\Users\\sukum\\OneDrive\\Documents\" ";
                    break;
                default:
                    return "INVALID SELECTION";
            }
            return Argument;
        }

        public string RunSpleeter(int noOfStems)
        {
            string filePath = "C:\\Users\\sukum\\Projects\\K_For_Krishna.mp3";
            string Arguments ="/c" + SpleeterArguments(noOfStems);
            var startInfo = new ProcessStartInfo("cmd.exe",Arguments + filePath);
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
