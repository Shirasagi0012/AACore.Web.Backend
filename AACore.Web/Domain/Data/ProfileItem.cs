using AACore.Web.API;

namespace AACore.Web.Domain.Data;

public enum ProfileItem
{
    MainPower,
    InputVoltage,
    AmbientTemperature,
    AmbientHumidity,
    DewFrostPoint,
    MainMirrorTemperature,
    MainMachineTemperature,
    GpsTime,
    DC1,
    DC2,
    DC3,
    DC4,
    DC5,
    DC6,
    DC7,
    PWM1,
    PWM2,
    PWM3,
    MasterSwitch,
    ComPort,
    TraceState,
}

public static class ProfileItemExtensions
{
    public static int ToPwmId(this ProfileItem item) => item switch
    {
        ProfileItem.PWM1 => 1,
        ProfileItem.PWM2 => 2,
        ProfileItem.PWM3 => 3,
        _ => throw new ArgumentOutOfRangeException(nameof(item), item, null)
    };

    public static int ToSwitchId(this ProfileItem item) => item switch
    {
        ProfileItem.DC1 => 1,
        ProfileItem.DC2 => 2,
        ProfileItem.DC3 => 3,
        ProfileItem.DC4 => 4,
        ProfileItem.DC5 => 5,
        ProfileItem.DC6 => 6,
        ProfileItem.DC7 => 7,
        _ => throw new ArgumentOutOfRangeException(nameof(item), item, null)
    };

    public static ProfileItem ToPwmItem(this int id) => id switch
    {
        1 => ProfileItem.PWM1,
        2 => ProfileItem.PWM2,
        3 => ProfileItem.PWM3,
        _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
    };

    public static ProfileItem ToSwitchItem(this int id) => id switch
    {
        1 => ProfileItem.DC1,
        2 => ProfileItem.DC2,
        3 => ProfileItem.DC3,
        4 => ProfileItem.DC4,
        5 => ProfileItem.DC5,
        6 => ProfileItem.DC6,
        7 => ProfileItem.DC7,
        _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
    };

    public static SwitchConfig[] ToSwitchConfigs(this Dictionary<ProfileItem, SwitchItem> @this) =>
    @this.Where(x =>
        {
            try
            {
                x.Key.ToSwitchId();
                return true;
            }
            catch
            {
                return false;
            }
        })
        .Select(x => new SwitchConfig(x.Key.ToSwitchId(), x.Value.Label))
        .ToArray();
    
    public static PwmConfig[] ToPwmConfigs(this Dictionary<ProfileItem, SwitchItem> @this) =>
    @this.Where(x =>
        {
            try
            {
                x.Key.ToPwmId();
                return true;
            }
            catch
            {
                return false;
            }
        })
        .Select(x => new PwmConfig(x.Key.ToPwmId(), x.Value.Label))
        .ToArray();
}

public class SwitchItem
{
    public string Label { get; set; }
    public string Description { get; set; } = "No Description";
    public bool Writable { get; set; } = false;
    public double MaxValue { get; set; } = Double.MaxValue;
    public double MinValue { get; set; } = Double.MinValue;
    public double Step { get; set; } = 1.0;
}