using System;
using System.Collections.Generic;

namespace ExceptionLogger.Models
{
    public class ExceptionContext
    {
        public ExceptionContext()
        {
            // Use System.Environment.MachineName to get the machine name
            MachineName = System.Environment.MachineName;
            Timestamp = DateTime.UtcNow;
            AdditionalData = new Dictionary<string, object>();
        }

        public Exception Exception { get; set; }
        public string ApplicationName { get; set; }
        public string Environment { get; set; }
        public Dictionary<string, object> AdditionalData { get; set; }
        public string RequestId { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public string MachineName { get; set; }
    }
}