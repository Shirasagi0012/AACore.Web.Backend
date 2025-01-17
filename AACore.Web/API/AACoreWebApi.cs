using AACore.Web.Domain.Data;
using AACore.Web.Domain.Device;
using Microsoft.AspNetCore.Mvc;

namespace AACore.Web.API;

public static class AACoreWebApi
{
    public static void MapAACoreWebApi(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/scan", () =>
        {
            var serials = Program.Device.AvailablePorts;
            return Results.Ok(serials);
        }).WithSummary("扫描串口").WithDescription("扫描串口并且获取信息，以数组的形式返回").Produces<string[]>();

        builder.MapGet("/all", () => { return Results.Ok(Program.Device.Data); }) // TODO: Not satisfy requirement
            .WithSummary("获取所有信息")
            .WithDescription("以JSON格式返回所有的Switch和PWM的信息").Produces<DeviceData>();

        builder.MapPost("/switch", (SwitchRequest request) =>
            {
                var data = Program.Device.Data.SetData(request.id switch
                {
                    1 => ProfileItem.DC1,
                    2 => ProfileItem.DC2,
                    3 => ProfileItem.DC3,
                    4 => ProfileItem.DC4,
                    5 => ProfileItem.DC5,
                    6 => ProfileItem.DC6,
                    7 => ProfileItem.DC7,
                }, request.value switch
                {
                    true => 1,
                    false => 0,
                });
                Program.Device.Data = data;
                return Results.Ok();
            })
            .WithSummary("设置Switch状态")
            .WithDescription("设置指定Switch状态，如果Switch不存在或参数类型有误则返回400非法输入，如果服务器出现任何错误则返回500服务器错误")
            .Produces(200)
            .ProducesProblem(400)
            .ProducesProblem(500);

        builder.MapPost("/pwm", (PwmRequest request) =>
            {
                var data = Program.Device.Data.SetData(request.id switch
                {
                    1 => ProfileItem.PWM1,
                    2 => ProfileItem.PWM2,
                    3 => ProfileItem.PWM3,
                }, request.value);
                Program.Device.Data = data;
                return Results.Ok();
            })
            .WithSummary("设置PWM状态")
            .WithDescription("设置指定PWM状态，如果PWM不存在或参数类型有误则返回400非法输入，如果服务器出现任何错误则返回500服务器错误")
            .Produces(200)
            .ProducesProblem(400)
            .ProducesProblem(500);

        builder.MapGet("/config/get", () =>
            {
                var config = (switches: new[] { (id: 0, value: "string") }, pwms: new[] { (id: 0, value: "string") },
                    enable_log: true);
                return Results.Ok(config);
            })
            .WithSummary("获取配置")
            .WithDescription("获取配置，即端口和名称的对应关系")
            .Produces<((int, string)[], (int, string)[], bool)>();

        builder.MapPost("/config/set", (ConfigRequest request) =>
            {
                // Implement logic to set configuration
                return Results.Ok();
            })
            .WithSummary("修改配置")
            .WithDescription("修改配置")
            .Produces(200);

        builder.MapPost("/connect", (ConnectRequest request) =>
            {
                Program.Device.Connect();
                return Results.Ok();
            })
            .WithSummary("连接")
            .Produces(200);

        builder.MapGet("/disconnect", () =>
            {
                Program.Device.Disconnect();
                return Results.Ok();
            })
            .WithSummary("断开连接")
            .WithDescription("断开连接，如果正常断开则返回200")
            .Produces(200);

        builder.MapGet("/log", () =>
            {
                var log = "string";
                return Results.Ok(log);
            })
            .WithSummary("获取日志")
            .WithDescription("获取当前完整的日志")
            .Produces<string>()
            .ProducesProblem(500);
    }
}

public record SwitchRequest(int id, bool value);

public record PwmRequest(int id, int value);

public record ConfigRequest(SwitchConfig[]? switches, PwmConfig[]? pwms, bool? enable_log);

public record SwitchConfig(int id, string value);

public record PwmConfig(int id, string value);

public record ConnectRequest(string com, string bard_rate, int timeout, int retry);