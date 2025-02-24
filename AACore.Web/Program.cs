using AACore.Web.API;
using AACore.Web.Domain;
using Microsoft.AspNetCore.Routing.Constraints;
using Scalar.AspNetCore;

namespace AACore.Web;

internal class Program
{
    public static AACoreDevice Device { get; private set; }
    
    public static bool EnableLogging { get; set; } = true;

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);

        builder.Services.Configure<RouteOptions>(options =>
            options.SetParameterPolicy<RegexInlineRouteConstraint>("regex"));

        builder.Services.AddOpenApi();

        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
        });

        var app = builder.Build();

        using var device = new AACoreDevice(app.Services.GetService<ILoggerProvider>() ??
                                            throw new InvalidOperationException(
                                                "Can't initialize ILoggerProvider service."));

        Device = device;
        
        app.MapScalarApiReference("scalar"); // scalar/v1
        app.MapOpenApi();

        var prefix = app.MapGroup("/api/v1"); // ASCOM Alpaca API

        // prefix.MapSerialApi();
        // prefix.MapDeviceApi();

        var prefix2 = app.MapGroup("/api/v1/aacore"); // AACore Web API

        prefix2.MapAACoreWebApi();

        app.Run();
    }
}