using AACore.Web.Domain.Device;

namespace AACore.Web.Domain.Data;

public class SendData
{
    public SwitchSet switch_set { get; }
    public bool switch_state { get; }
    public bool ds18b20_msg { get; }
    public bool tmp75_msg { get; }
    public bool sht40_msg { get; }
    public bool ina219_msg { get; }
    public bool gps_msg { get; }
    public double ds18b20_set { get; }
    public double tmp75_set { get; }
    public double gps_time { get; }
    public double send_timestamp { get; }

    public SendData(SwitchSet switchSet, bool switchState, bool ds18B20Msg, bool tmp75Msg, bool sht40Msg,
        bool ina219Msg, bool gpsMsg, double ds18B20Set, double tmp75Set, double gpsTime, double sendTimestamp)
    {
        switch_set = switchSet;
        switch_state = switchState;
        ds18b20_msg = ds18B20Msg;
        tmp75_msg = tmp75Msg;
        sht40_msg = sht40Msg;
        ina219_msg = ina219Msg;
        gps_msg = gpsMsg;
        ds18b20_set = ds18B20Set;
        tmp75_set = tmp75Set;
        gps_time = gpsTime;
        send_timestamp = sendTimestamp;
    }
}