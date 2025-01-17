using AACore.Web.Domain.Device;

namespace AACore.Web.Domain.Serial.Configuration;

public record ProfileConfiguration
{
    internal Dictionary<ProfileItem, SwitchItem> Switches { get; } = new Dictionary<ProfileItem, SwitchItem>()
    {
        { ProfileItem.MainPower, new SwitchItem { Label = "整机功率(W)", Step = 0.0001 } },
        { ProfileItem.InputVoltage, new SwitchItem { Label = "输入电压(V)", Step = 0.0001 } },
        { ProfileItem.AmbientTemperature, new SwitchItem { Label = "环境温度(°C)", Step = 0.0001 } },
        {
            ProfileItem.AmbientHumidity,
            new SwitchItem { Label = "环境湿度(%)", MaxValue = 100.0, MinValue = 0.0, Step = 0.0001 }
        },
        { ProfileItem.DewFrostPoint, new SwitchItem { Label = "露/霜点(°C)", Step = 0.0001 } },
        { ProfileItem.MainMirrorTemperature, new SwitchItem { Label = "主镜温度(°C)", Step = 0.0001 } },
        { ProfileItem.MainMachineTemperature, new SwitchItem { Label = "主机温度(°C)", Step = 0.0001 } },
        { ProfileItem.GpsTime, new SwitchItem { Label = "GPS 时间", Step = 0.0001 } },
        { ProfileItem.MasterSwitch, new SwitchItem { Label = "主开关", MaxValue = 1.0, MinValue = 0.0 } },
        { ProfileItem.DC1, new SwitchItem { Label = "主镜供电", Writable = true, MaxValue = 1.0, MinValue = 0.0 } },
        { ProfileItem.DC2, new SwitchItem { Label = "主相机供电", Writable = true, MaxValue = 1.0, MinValue = 0.0 } },
        { ProfileItem.DC3, new SwitchItem { Label = "赤道仪供电", Writable = true, MaxValue = 1.0, MinValue = 0.0 } },
        { ProfileItem.DC4, new SwitchItem { Label = "滤镜轮供电", Writable = true, MaxValue = 1.0, MinValue = 0.0 } },
        { ProfileItem.DC5, new SwitchItem { Label = "电调供电", Writable = true, MaxValue = 1.0, MinValue = 0.0 } },
        { ProfileItem.DC6, new SwitchItem { Label = "N/C", Writable = true, MaxValue = 1.0, MinValue = 0.0 } },
        { ProfileItem.DC7, new SwitchItem { Label = "N/C", Writable = true, MaxValue = 1.0, MinValue = 0.0 } },
        { ProfileItem.PWM1, new SwitchItem { Label = "主镜自适应加热", Writable = true, MaxValue = 100.0, MinValue = 0.0 } },
        { ProfileItem.PWM2, new SwitchItem { Label = "平场板亮度", Writable = true, MaxValue = 100.0, MinValue = 0.0 } },
        { ProfileItem.PWM3, new SwitchItem { Label = "主机自适应加热", Writable = true, MaxValue = 100.0, MinValue = 0.0 } },
    };
}