using AACore.Web.Domain.Device;

namespace AACore.Web.Domain.Data
{
    /// <summary>
    /// 设备回传的全部数据，并且提供了按id获取/设置数据的方法
    /// </summary>
    public struct DeviceData
    {
        public DeviceData()
        {
        }

        public double ds18b20_temp { get; set; } = 0;
        public double tmp75_temp { get; set; } = 0;
        public double sht40_temp { get; set; } = 0;
        public double sht40_humi { get; set; } = 0;
        public double ina219_volt { get; set; } = 0;
        public double ina219_curr { get; set; } = 0;
        public GpsMsg gps_msg { get; set; } = new GpsMsg();
        public string gps_time { get; set; } = "";
        public double send_timestamp { get; set; } = 0;
        public SwitchSet switch_set { get; set; } = new SwitchSet();

        public double GetData(ProfileItem i)
        {
            switch (i)
            {
                case ProfileItem.DC1:
                    return switch_set.switch_1 ? 1 : 0;
                case ProfileItem.DC2:
                    return switch_set.switch_2 ? 1 : 0;
                case ProfileItem.DC3:
                    return switch_set.switch_3 ? 1 : 0;
                case ProfileItem.DC4:
                    return switch_set.switch_4 ? 1 : 0;
                case ProfileItem.DC5:
                    return switch_set.switch_5 ? 1 : 0;
                case ProfileItem.DC6:
                    return switch_set.switch_6 ? 1 : 0;
                case ProfileItem.DC7:
                    return switch_set.switch_7 ? 1 : 0;
                case ProfileItem.PWM1:
                    return switch_set.pwm_1;
                case ProfileItem.PWM2:
                    return switch_set.pwm_2;
                case ProfileItem.PWM3:
                    return switch_set.pwm_3;

                case ProfileItem.MainPower:
                    return (ina219_curr * 15.57 / 100 * ina219_volt + 269.39) / 1000 * 0.83;
                case ProfileItem.InputVoltage:
                    return ina219_volt;
                case ProfileItem.AmbientTemperature:
                    return sht40_temp;
                case ProfileItem.AmbientHumidity:
                    return sht40_humi;
                case ProfileItem.DewFrostPoint:
                    return CalculateDewPoint();

                case ProfileItem.MainMirrorTemperature:
                    return ds18b20_temp;
                case ProfileItem.MainMachineTemperature:
                    return tmp75_temp;
                case ProfileItem.GpsTime:
                    return send_timestamp;

                case ProfileItem.MasterSwitch:
                    return 0;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private double CalculateSVP(double temperature)
        {
            return 6.11 * Math.Pow(x: 10, 7.5 * temperature / (237.7 + temperature));
        }

        private double CalculateVP(double humidity, double svp)
        {
            return humidity * svp / 100;
        }

        private double CalculateDewPoint()
        {
            var svp = CalculateSVP(sht40_temp);
            var vp = CalculateVP(sht40_humi, svp);
            var minDifference = 1e6;
            double minTemp = -100;
            for (double temp = -100; temp <= 100; temp += 0.01)
            {
                var currentSVP = CalculateSVP(temp);
                var difference = Math.Abs(vp - currentSVP);
                if (difference < minDifference)
                {
                    minDifference = difference;
                    minTemp = temp;
                }
            }

            return Math.Round(minTemp, digits: 2);
        }

        public DeviceData SetData(ProfileItem i, double value)
        {
            var tempSwitchSet = switch_set;
            switch (i)
            {
                case ProfileItem.DC1:
                    tempSwitchSet.switch_1 = value > 0.5;
                    break;
                case ProfileItem.DC2:
                    tempSwitchSet.switch_2 = value > 0.5;
                    break;
                case ProfileItem.DC3:
                    tempSwitchSet.switch_3 = value > 0.5;
                    break;
                case ProfileItem.DC4:
                    tempSwitchSet.switch_4 = value > 0.5;
                    break;
                case ProfileItem.DC5:
                    tempSwitchSet.switch_5 = value > 0.5;
                    break;
                case ProfileItem.DC6:
                    tempSwitchSet.switch_6 = value > 0.5;
                    break;
                case ProfileItem.DC7:
                    tempSwitchSet.switch_7 = value > 0.5;
                    break;
                case ProfileItem.PWM1:
                    tempSwitchSet.pwm_1 = (int)value;
                    break;
                case ProfileItem.PWM2:
                    tempSwitchSet.pwm_2 = (int)value;
                    break;
                case ProfileItem.PWM3:
                    tempSwitchSet.pwm_3 = (int)value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch_set = tempSwitchSet;
            return this;
        }

        public SendData ConvertToSendData()
        {
            return new SendData(switch_set, true, true, true, true, true, true,
                GetData(ProfileItem.MainMirrorTemperature), GetData(ProfileItem.MainMachineTemperature),
                GetData(ProfileItem.GpsTime), DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                                              + DateTimeOffset.UtcNow.Millisecond / 1000.0);
        }
    }
}