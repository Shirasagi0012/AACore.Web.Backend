namespace AACore.Web.Domain.Data;

public struct GpsMsg
{
    public GpsMsg()
    {
    }

    public double gps_latitude { get; set; } = 0;
    public string gps_latitude_dir { get; set; } = "";
    public double gps_longitude { get; set; } = 0;
    public string gps_longitude_dir { get; set; } = "";
}