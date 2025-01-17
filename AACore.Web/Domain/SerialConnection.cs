using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Text.Json;
using AACore.Web.Domain.Data;
using AACore.Web.Utils;

namespace AACore.Web.Domain;

public class SerialConnection : IDisposable
{

    private const long HeartbeatInterval = 500;

    private readonly SerialPort _serialPort;
    private readonly ILogger? _logger;
    private readonly CancellationTokenSource _cts;
    private Task _readTask;

    public SerialConnection(SerialPort serialPort, Action<DeviceData> onReceiveData,
        ILogger? logger = null)
    {
        _logger = logger;

        logger?.LogInformation("Serial connection Initializing with parameters:");
        logger?.LogInformation("- Port Name: {PortName}", serialPort.PortName);
        logger?.LogInformation("- Baud Rate: {BaudRate}", serialPort.BaudRate);
        logger?.LogInformation("- Data Bits: {serialPort.DataBits}", serialPort.DataBits);
        logger?.LogInformation("- Parity: {Parity}", serialPort.Parity);
        logger?.LogInformation("- Stop Bits: {StopBits}", serialPort.StopBits);
        logger?.LogInformation("- Handshake: {Handshake}", serialPort.Handshake);
        logger?.LogInformation("- Read Timeout: {ReadTimeout}", serialPort.ReadTimeout);
        logger?.LogInformation("- Write Timeout: {WriteTimeout}", serialPort.WriteTimeout);

        try
        {
            _serialPort = serialPort;
            _serialPort.Open();
        }
        catch (Exception)
        {
            logger?.LogError("Failed to open serial connection on {PortName}.", serialPort.PortName);
            throw;
        }

        logger?.LogInformation("Serial connection on {PortName} opened.", serialPort.PortName);

        logger?.LogInformation("Staring read thread.");
        _cts = new CancellationTokenSource();
        _readTask = NewReadLoop(onReceiveData,_cts.Token);
    }

    private async Task NewReadLoop(Action<DeviceData> callback, CancellationToken ct)
    {
        var sw = new Stopwatch();
        var buffer = new StringBuilder();
        while (!ct.IsCancellationRequested)
        {
            try
            {
                sw.Reset();
                sw.Start();
                var next = (char)_serialPort.ReadChar();
                if (sw.ElapsedMilliseconds > HeartbeatInterval)
                {
                    var jsonData = buffer.ToString();
                    _logger?.LogTrace($"Serial Received: {jsonData}");
                    buffer.Clear();

                    try
                    {
                        var deviceData = JsonSerializer.Deserialize(jsonData,
                            AppJsonSerializerContext.Default.DeviceData);
                        callback?.Invoke(deviceData);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(
                            "JSON processing error: {Message}", ex.Message
                        );
                    }

                    await Task.Delay(100, ct);
                }

                buffer.Append(next);
            }
            catch (TimeoutException)
            {
                _logger?.LogWarning("Read Timeout. Check if device is online?");
            }
            catch (OperationCanceledException)
            {
                _logger?.LogInformation("Read Task Canceled.");
            }
            catch (Exception e)
            {
                _logger?.LogError(
                    "Read error: {Message}", e.Message);
                throw;
            }
        }
    }

    public void Send(DeviceData data)
    {
        var jsonData = JsonSerializer.Serialize(data.ConvertToSendData(), AppJsonSerializerContext.Default.SendData);
        _logger?.LogInformation("Sending data: {Data}", jsonData);
        _serialPort.WriteLine(jsonData);
    }

    # region IDisposable

    public void Dispose()
    {
        _logger?.LogInformation("Disposing Serial Connection.");
        _cts.Cancel();
        ExceptionUtils.IgnoreException(() => _readTask.Wait());
        _serialPort.Dispose();
        GC.SuppressFinalize(this);
    }
    
    # endregion
}