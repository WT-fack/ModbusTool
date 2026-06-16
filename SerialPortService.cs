using System.IO.Ports;

namespace ModbusTool;

public class SerialPortService : IDisposable
{
    private readonly SerialPort _port;
    public bool IsOpen => _port.IsOpen;

    public event EventHandler<byte[]>? DataReceived;

    public SerialPortService(string portName, int baudRate, int dataBits, StopBits stopBits, Parity parity)
    {
        _port = new SerialPort(portName, baudRate, parity, dataBits, stopBits)
        {
            ReadTimeout = 3000,
            WriteTimeout = 1000,
            RtsEnable = true
        };
        _port.DataReceived += OnDataReceived;
    }

    public void Open() => _port.Open();

    public void Close()
    {
        if (_port.IsOpen)
            _port.Close();
    }

    public async Task<byte[]> SendAsync(byte[] data)
    {
        await Task.Run(() =>
        {
            // 清空缓冲区
            _port.DiscardInBuffer();
            _port.Write(data, 0, data.Length);
        });

        // 等待响应（Modbus RTU 帧间间隔 3.5 字符时间 + 响应时间）
        await Task.Delay(100);

        var buffer = new List<byte>();
        var timeout = DateTime.Now.AddMilliseconds(_port.ReadTimeout);

        while (DateTime.Now < timeout)
        {
            try
            {
                var b = (byte)_port.ReadByte();
                buffer.Add(b);

                // Modbus RTU 响应最小长度: 从站ID(1)+功能码(1)+字节数(1)+数据(≥0)+CRC(2) = 5
                // 收到完整帧后等待 50ms 无新数据则认为接收完毕
                if (buffer.Count >= 5)
                {
                    await Task.Delay(50);
                    if (_port.BytesToRead == 0)
                        break;
                }
            }
            catch (TimeoutException)
            {
                break;
            }
        }

        if (buffer.Count == 0)
            throw new TimeoutException("未收到从站响应");

        return buffer.ToArray();
    }

    private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        var count = _port.BytesToRead;
        var buf = new byte[count];
        _port.Read(buf, 0, count);
        DataReceived?.Invoke(this, buf);
    }

    public void Dispose()
    {
        if (_port.IsOpen)
            _port.Close();
        _port.Dispose();
    }
}
