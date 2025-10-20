using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace AsmPj.Helpers;

public static class SerilogConfig
{
    public static void ConfigureSerilog(WebApplicationBuilder builder)
    {
        var logPath = Path.Combine(Directory.GetCurrentDirectory(),"logs","log-.txt");

        builder.Host.UseSerilog((context, config) =>
        {
            config
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console() // logging to console
                .WriteTo.File(
                    logPath,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7, // keep logs for 7 days
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                );

            // Optionally, you can add more sinks like Seq, etc.
            var seqUrl = context.Configuration["Serilog:SeqUrl"];
            if (!string.IsNullOrEmpty(seqUrl))
            {
                config.WriteTo.Seq(seqUrl);
            }
        });
    }
}