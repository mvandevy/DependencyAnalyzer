using System;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.VstsExtensionLog
{
    public class VstsExtensionLogSink : ILogEventSink
    {
        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null) return;

            switch (logEvent.Level)
            {
                case LogEventLevel.Error:
                case LogEventLevel.Fatal:
                    Console.WriteLine($"##vso[task.logissue type=error;]{logEvent.RenderMessage()}");
                    break;
                case LogEventLevel.Warning:
                    Console.WriteLine($"##vso[task.logissue type=warning;]{logEvent.RenderMessage()}");
                    break;
                case LogEventLevel.Debug:
                case LogEventLevel.Information:
                case LogEventLevel.Verbose:
                    Console.WriteLine(logEvent.RenderMessage());
                    break;
            }

            if (logEvent.Exception != null)
            {
                Console.WriteLine($"##vso[task.logissue type=error;]{logEvent.Exception}");
            }
        }
    }
}