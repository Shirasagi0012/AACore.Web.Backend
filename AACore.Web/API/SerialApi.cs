namespace AACore.Web.API;

public static class SerialApi
{
    public static void MapSerialApi(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/serials", () =>
        {
            var serials = Program.Device.AvailablePorts;
            return Results.Ok(serials);
        });
    }
}