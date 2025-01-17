namespace AACore.Web.Domain.Device;

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

public class SwitchItem
{
    public string Label { get; set; }
    public string Description { get; set; } = "No Description";
    public bool Writable { get; set; } = false;
    public double MaxValue { get; set; } = Double.MaxValue;
    public double MinValue { get; set; } = Double.MinValue;
    public double Step { get; set; } = 1.0;
}