using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.Configuration
{
    public class ServiceCreationOptions
    {
        /// <summary> The logger providers to inject into the service dependencies </summary>
        public List<ILoggerProvider> LoggerProviders { get; set; } = new List<ILoggerProvider>();

    }
}
