using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Text.Json;
using AACore.Web.Domain.Data;

namespace AACore.Web.Domain.Serial;

public class SerialConnection : IDisposable
{
    private const long HeartbeatInterval = 500;

    private readonly SerialPort _serialPort;
    private readonly ILogger? _logger;
    private readonly CancellationTokenSource _cts;
    private Thread _readThread;

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
        _readThread = new Thread(() => NewReadLoop(onReceiveData, _cts.Token));
        _readThread.Start();
    }

    private void NewReadLoop(Action<DeviceData> callback, CancellationToken ct)
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
                }

                buffer.Append(next);
            }
            catch (TimeoutException)
            {
                _logger?.LogWarning("Read Timeout. Check if device is online?");
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


    // private void ReadLoop(Action<DeviceData> callback, CancellationToken ct)
    // {
    //     var buffer = new StringBuilder();
    //
    //     while (!ct.IsCancellationRequested)
    //     {
    //         try
    //         {
    //             var response = _serialPort.ReadLine();
    //             _logger?.LogTrace($"Serial Received: {response}");
    //
    //             if (!string.IsNullOrEmpty(response))
    //             {
    //                 buffer.Append(response);
    //
    //                 while (true)
    //                 {
    //                     var bufferContent = buffer.ToString();
    //
    //                     var startIndex = bufferContent.IndexOf('{');
    //                     var endIndex = bufferContent.IndexOf('}', startIndex);
    //                     var endIndex2 = bufferContent.IndexOf('}', endIndex + 1);
    //                     var endIndex3 = bufferContent.IndexOf('}', endIndex2 + 1);
    //
    //                     if (startIndex != -1 && endIndex3 != -1)
    //                     {
    //                         var jsonData = bufferContent.Substring(startIndex, endIndex3 - startIndex + 1);
    //                         _logger?.LogError($"Received JSON: {jsonData}");
    //                         try
    //                         {
    //                             var deviceData = JsonSerializer.Deserialize(jsonData,
    //                                 AppJsonSerializerContext.Default.DeviceData);
    //                             callback?.Invoke(deviceData);
    //                         }
    //                         catch (Exception ex)
    //                         {
    //                             _logger?.LogError(
    //                                 "JSON processing error: {Message}", ex.Message
    //                             );
    //                         }
    //
    //                         buffer.Remove(0, endIndex3 + 1);
    //                     }
    //                     else
    //                     {
    //                         break;
    //                     }
    //                 }
    //             }
    //
    //             Thread.Sleep(100);
    //         }
    //         catch (Exception ex)
    //         {
    //             // Log the exception and dispose of the connection
    //             _logger?.LogError("Exception when reading from serial port: {Message}", ex.Message);
    //         }
    //     }
    // }

    # region IDisposable

    public void Dispose()
    {
        _logger?.LogInformation("Disposing Serial Connection.");
        _cts.Cancel();
        _serialPort.Dispose();
    }

    # endregion
}