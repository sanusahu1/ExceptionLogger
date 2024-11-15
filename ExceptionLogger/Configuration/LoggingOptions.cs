using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionLogger.Configuration
{
    public class LoggingOptions
    {
        public bool IncludeStackTrace { get; set; } = true;
        public bool IncludeInnerExceptions { get; set; } = true;
        public bool IncludeSystemInfo { get; set; } = true;
        public bool IncludeRequestInfo { get; set; } = true;
        public int MaxInnerExceptionDepth { get; set; } = 5;
        public string PathToStore = string.Empty;
    }
}
