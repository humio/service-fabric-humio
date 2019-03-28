using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ConsoleExample
{
    class Program
    {
        static int Main(string[] args)
        {
            var log = Path.Combine("log", $"ConsoleExample.log");

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.With(new ServiceFabricEnricher())
                .WriteTo.File(
                    formatter: new JsonFormatter(renderMessage: true),
                    path: log,
                    retainedFileCountLimit: 5,
                    shared: true)
                .CreateLogger();

            try
            {
                var random = new Random();

                while (true)
                {
                    int i = random.Next(100, 120);
                    Log.Information("Number of user sessions {@UserSessions}", i);

                    var position = new { Latitude = 25, Longitude = 134 };

                    if (i == 120)
                    {
                        int ei = random.Next(0, 1000);
                        if (ei == 42)
                        {
                            int ooops = ei / 0;
                        }
                    }

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            return 0;
        }
    }

    public class ServiceFabricEnricher : ILogEventEnricher
    {
        private static readonly string instanceId = Guid.NewGuid().ToString();
        private static readonly string processId = Process.GetCurrentProcess().Id.ToString();
        private static readonly string node = Environment.GetEnvironmentVariable("Fabric_NodeName");
        private static readonly string service = Environment.GetEnvironmentVariable("Fabric_ServiceName");
        private static readonly string application = Environment.GetEnvironmentVariable("Fabric_ApplicationName");

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (!string.IsNullOrWhiteSpace(instanceId))
            {
                logEvent.AddPropertyIfAbsent(new LogEventProperty("InstanceId", new ScalarValue(instanceId)));
            }

            if (!string.IsNullOrWhiteSpace(node))
            {
                logEvent.AddPropertyIfAbsent(new LogEventProperty("Fabric_NodeName", new ScalarValue(node)));
            }

            if (!string.IsNullOrWhiteSpace(service))
            {
                logEvent.AddPropertyIfAbsent(new LogEventProperty("Fabric_ServiceName", new ScalarValue(service)));
            }

            if (!string.IsNullOrWhiteSpace(application))
            {
                logEvent.AddPropertyIfAbsent(new LogEventProperty("Fabric_ApplicationName", new ScalarValue(application)));
            }

            if (!string.IsNullOrWhiteSpace(processId))
            {
                logEvent.AddPropertyIfAbsent(new LogEventProperty("ProcessId", new ScalarValue(processId)));
            }
        }
    }

}
