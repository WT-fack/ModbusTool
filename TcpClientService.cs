using System.Net.Sockets;

namespace ModbusTool;

public class TcpClientService : IDisposable
{
    private TcpClient? _client;
    private NetworkStream? _stream;
    private readonly string _host;
    private readonly int _port;
    private readonly int _timeoutMs;

    public bool Connected => _client?.Connected ?? false;

    public TcpClientService(string host, int port, int timeoutMs = 3000)
    {
        _host = host;
        _port = port;
        _timeoutMs = timeoutMs;
    }

    public async Task ConnectAsync()
    {
        _client = new TcpClient { ReceiveTimeout = _timeoutMs, SendTimeout = _timeoutMs };
        await _client.ConnectAsync(_host, _port);
        _stream = _client.GetStream();
    }

    public void Disconnect()
    {
        _stream?.Close();
        _client?.Close();
        _stream = null;
        _client = null;
    }

    public async Task<byte[]> SendAsync(byte[] data)
    {
        if (_stream == null)
            throw new InvalidOperationException("TCP 未连接");

        // Modbus TCP 帧头: 事务标识(2) + 协议标识(2) + 长度(2) + 单元标识(1) + PDU
        var mbap = new byte[7];
        mbap[0] = 0x00; // 事务标识高字节
        mbap[1] = 0x01; // 事务标识低字节
        mbap[2] = 0x00; // 协议标识
        mbap[3] = 0x00;
        var length = (ushort)(1 + data.Length); // 单元标识 + PDU
        mbap[4] = (byte)(length >> 8);
        mbap[5] = (byte)(length & 0xFF);
        mbap[6] = 0x01; // 单元标识

        var frame = new byte[mbap.Length + data.Length];
        Buffer.BlockCopy(mbap, 0, frame, 0, mbap.Length);
        Buffer.BlockCopy(data, 0, frame, mbap.Length, data.Length);

        await _stream.WriteAsync(frame);
        await _stream.FlushAsync();

        // 读取响应
        var header = new byte[7];
        var read = await _stream.ReadAsync(header);
        if (read < 7)
            throw new IOException("TCP 响应头不完整");

        var respLen = (header[4] << 8) | header[5];
        var pdu = new byte[respLen - 1]; // 减去单元标识
        read = await _stream.ReadAsync(pdu);
        if (read < pdu.Length)
            throw new IOException("TCP 响应数据不完整");

        return pdu;
    }

    public void Dispose()
    {
        Disconnect();
        _client?.Dispose();
    }
}
