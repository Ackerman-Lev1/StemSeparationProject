using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerLogic.ExternalProcesses
{
    public class CommandResult
    {
        public string? Output { get; set; }
        public string? Error { get; set; }
        public int? ExitCode { get; set; }
    }
}
