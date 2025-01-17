using System.IO.Ports;
using AACore.Web.Domain.Configuration;
using AACore.Web.Domain.Data;

namespace AACore.Web.Domain;

public class AACoreDevice(ILoggerProvider loggerProvider)
    : IDisposable
{
    public DeviceData Data
    {
        get => _data;
        set { _data = value; _connection.Send(_data); }
    }

    public ProfileConfiguration ProfileConfiguration { get; } = new();
    
    private SerialConnection? _connection;
    private readonly ILogger _logger = loggerProvider.CreateLogger(nameof(AACoreDevice));
    private DeviceData _data = new();

    public int BaudRate { get; set; } = 115200;
    public string PortName { get; set; } = "COM1";
    public int ReceiveTimeout { get; set; } = 3000;

    public string[] AvailablePorts => SerialPort.GetPortNames();

    public void Connect()
    {
        if (_connection != null)
        {
            _logger.LogWarning("Serial connection already exists.");
            throw new InvalidOperationException("Serial connection already exists.");
        }

        var serialPort = new SerialPort(PortName, BaudRate)
        {
            ReadTimeout = ReceiveTimeout,
            WriteTimeout = ReceiveTimeout
        };

        _connection = new SerialConnection(serialPort, OnReceiveData,
            loggerProvider.CreateLogger(nameof(SerialConnection)));
    }

    public void Disconnect()
    {
        if (_connection == null)
        {
            _logger.LogWarning("No serial connection to disconnect.");
            throw new InvalidOperationException("No serial connection to disconnect.");
        }

        _connection.Dispose();
        _connection = null;
    }

    private void OnReceiveData(DeviceData data)
    {
        _logger.LogInformation("Received data: {Data}", data);
        _data = data;
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}