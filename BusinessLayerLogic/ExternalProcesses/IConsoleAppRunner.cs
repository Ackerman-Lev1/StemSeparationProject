using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerLogic.ExternalProcesses
{
    public interface IConsoleAppRunner
    {
        //void OpenCommandPrompt();
        string SpleeterArguments(int noOfStems);
        string RunSpleeter(int noOfStems);
    }
}
