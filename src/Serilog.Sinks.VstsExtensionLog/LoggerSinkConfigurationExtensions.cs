using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.VstsExtensionLog;
using System;

namespace Serilog
{
    /// <summary>
    /// The logger configuration VSTS Extension log extensions.
    /// </summary>
    public static class LoggerSinkConfigurationExtensions
    {
        /// <summary>Creates a new VSTS Extension log sink .</summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <returns>The <see cref="LoggerConfiguration"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static LoggerConfiguration VstsExtensionLog(
            this LoggerSinkConfiguration loggerConfiguration,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum
        )
        {
            if (loggerConfiguration == null)
                throw new ArgumentNullException(nameof(loggerConfiguration));

            return loggerConfiguration.Sink(new VstsExtensionLogSink(), restrictedToMinimumLevel);
        }
    }
}