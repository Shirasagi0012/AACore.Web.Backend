using System.Text.Json.Serialization;
using AACore.Web.API;
using AACore.Web.Domain.Data;

namespace AACore.Web;

[JsonSerializable(typeof(string[]))]
[JsonSerializable(typeof(double))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(DeviceData))]
[JsonSerializable(typeof(SendData))]
[JsonSerializable(typeof(GpsMsg))]
[JsonSerializable(typeof(SwitchSet))]
[JsonSerializable(typeof(SwitchRequest))]
[JsonSerializable(typeof(PwmRequest))]
[JsonSerializable(typeof(ConfigRequest))]
[JsonSerializable(typeof(SwitchConfig))]
[JsonSerializable(typeof(PwmConfig))]
[JsonSerializable(typeof(ConnectRequest))]
[JsonSerializable(typeof(ConfigResponse))]
[JsonSerializable(typeof(SwitchConfig[]))]
[JsonSerializable(typeof(PwmConfig[]))]
[JsonSerializable(typeof(((int, string)[], (int, string)[], bool)))]
[JsonSerializable(typeof(Microsoft.AspNetCore.Mvc.ProblemDetails))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}