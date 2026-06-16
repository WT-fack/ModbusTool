namespace ModbusTool;

public static class ModbusService
{
    /// <summary>构建功能码03（读保持寄存器）请求帧</summary>
    public static byte[] BuildReadHoldingRegisters(byte slaveId, ushort startAddr, ushort quantity)
    {
        var frame = new byte[8];
        frame[0] = slaveId;
        frame[1] = 0x03; // 功能码
        frame[2] = (byte)(startAddr >> 8);   // 起始地址高字节
        frame[3] = (byte)(startAddr & 0xFF); // 起始地址低字节
        frame[4] = (byte)(quantity >> 8);     // 数量高字节
        frame[5] = (byte)(quantity & 0xFF);   // 数量低字节
        var crc = CalculateCrc(frame, 6);
        frame[6] = crc.Item1; // CRC 低字节
        frame[7] = crc.Item2; // CRC 高字节
        return frame;
    }

    /// <summary>解析功能码03响应，返回 ushort 数组</summary>
    public static ushort[] ParseReadResponse(byte[] response, ushort expectedQuantity)
    {
        if (response.Length < 5)
            throw new Exception("响应帧过短");

        var slaveId = response[0];
        var funcCode = response[1];

        // 异常响应检查
        if ((funcCode & 0x80) != 0)
        {
            var excCode = response.Length > 2 ? response[2] : (byte)0;
            var msg = excCode switch
            {
                1 => "非法功能码",
                2 => "非法数据地址",
                3 => "非法数据值",
                4 => "从站设备故障",
                _ => $"异常码 0x{excCode:X2}"
            };
            throw new Exception($"从站 {slaveId} 返回异常: {msg}");
        }

        if (funcCode != 0x03)
            throw new Exception($"功能码不匹配: 期望 0x03, 实际 0x{funcCode:X2}");

        var byteCount = response[2];
        var expectedBytes = expectedQuantity * 2;
        if (byteCount != expectedBytes)
            throw new Exception($"字节数不匹配: 期望 {expectedBytes}, 实际 {byteCount}");

        // CRC 校验
        var crc = CalculateCrc(response, response.Length - 2);
        if (crc.Item1 != response[^2] || crc.Item2 != response[^1])
            throw new Exception("CRC 校验失败");

        var values = new ushort[expectedQuantity];
        for (var i = 0; i < expectedQuantity; i++)
        {
            values[i] = (ushort)((response[3 + i * 2] << 8) | response[4 + i * 2]);
        }

        return values;
    }

    /// <summary>Modbus CRC-16 校验（查表法）</summary>
    private static (byte, byte) CalculateCrc(byte[] data, int length)
    {
        ushort crc = 0xFFFF;
        for (var i = 0; i < length; i++)
        {
            crc ^= data[i];
            for (var j = 0; j < 8; j++)
            {
                if ((crc & 0x0001) != 0)
                    crc = (ushort)((crc >> 1) ^ 0xA001);
                else
                    crc >>= 1;
            }
        }
        return ((byte)(crc & 0xFF), (byte)(crc >> 8)); // 低字节在前
    }
}
