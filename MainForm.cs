using System.IO.Ports;

namespace ModbusTool;

public partial class MainForm : Form
{
    private SerialPortService? _serial;
    private TcpClientService? _tcp;
    private readonly SemaphoreSlim _sendLock = new(1, 1);
    private System.Windows.Forms.Timer? _autoTimer;
    private readonly List<string> _pausedBuffer = [];
    private bool _hexLog;

    // 颜色方案
    private static readonly Color Accent      = Color.FromArgb(0, 150, 255);
    private static readonly Color AccentGreen  = Color.FromArgb(40, 200, 120);
    private static readonly Color AccentRed    = Color.FromArgb(240, 70, 80);
    private static readonly Color FgMain       = Color.FromArgb(220, 220, 230);
    private static readonly Color FgDim        = Color.FromArgb(160, 160, 175);
    private static readonly Color BgGroup      = Color.FromArgb(48, 48, 60);
    private static readonly Color BgInput      = Color.FromArgb(55, 55, 70);

    public MainForm()
    {
        InitializeComponent();
        RefreshPortList();
    }

    // ================================================================
    // 串口
    // ================================================================
    private void TabConnection_SelectedIndexChanged(object? s, EventArgs e)
    {
        if (tabConnection.SelectedTab == tabSerial)
            RefreshPortList();
    }

    private void RefreshPortList()
    {
        var prev = cbPort.SelectedItem?.ToString();
        cbPort.Items.Clear();
        foreach (var name in SerialPort.GetPortNames())
            cbPort.Items.Add(name);

        if (cbPort.Items.Count > 0)
        {
            if (prev != null && cbPort.Items.Contains(prev))
                cbPort.SelectedItem = prev;
            else
                cbPort.SelectedIndex = 0;
        }
    }

    private void BtnPortRefresh_Click(object? s, EventArgs e) => RefreshPortList();

    private void BtnSerialOpen_Click(object? s, EventArgs e)
    {
        if (_serial is { IsOpen: true })
        {
            _serial.Close();
            SetSerialConnected(false);
            return;
        }

        if (cbPort.SelectedItem is not string port || string.IsNullOrEmpty(port))
        {
            AppendLog("[错误] 请选择串口");
            return;
        }

        var baud = int.TryParse(cbBaud.SelectedItem?.ToString(), out var b) ? b : 9600;
        var dataBits = int.TryParse(cbData.SelectedItem?.ToString(), out var d) ? d : 8;
        var stop = cbStop.SelectedIndex switch { 1 => StopBits.Two, _ => StopBits.One };
        var parity = cbParity.SelectedIndex switch
        {
            1 => Parity.Odd,
            2 => Parity.Even,
            3 => Parity.Mark,
            4 => Parity.Space,
            _ => Parity.None
        };

        try
        {
            _serial = new SerialPortService(port, baud, dataBits, stop, parity);
            _serial.Open();
            _serial.DataReceived += Serial_DataReceived;
            SetSerialConnected(true);
            AppendLog($"[串口] {port} 已打开 {baud},{dataBits},{stop},{parity}");
        }
        catch (Exception ex)
        {
            AppendLog($"[错误] 打开串口失败: {ex.Message}");
            _serial?.Dispose();
            _serial = null;
        }
    }

    private void SetSerialConnected(bool connected)
    {
        btnSerialOpen.Text = connected ? "■ 关闭串口" : "▶ 打开串口";
        btnSerialOpen.BackColor = connected ? AccentRed : Accent;
        lblSerialStatus.Text = connected ? "● 已连接" : "● 未连接";
        lblSerialStatus.ForeColor = connected ? AccentGreen : AccentRed;
        cbPort.Enabled = !connected;
        cbBaud.Enabled = !connected;
        cbData.Enabled = !connected;
        cbStop.Enabled = !connected;
        cbParity.Enabled = !connected;

        tslConnection.Text = connected
            ? $"串口 {cbPort.SelectedItem} {cbBaud.SelectedItem}"
            : "未连接";
        tslConnection.ForeColor = connected ? AccentGreen : FgDim;
    }

    private void Serial_DataReceived(object? s, byte[] data)
    {
        var hex = BitConverter.ToString(data).Replace("-", " ");
        BeginInvoke(() => AppendLog($"[收] {hex}"));
    }

    // ================================================================
    // TCP
    // ================================================================
    private async void BtnTcpConnect_Click(object? s, EventArgs e)
    {
        if (_tcp is { Connected: true })
        {
            _tcp.Disconnect();
            SetTcpConnected(false);
            return;
        }

        var ip = tbTcpIp.Text.Trim();
        if (string.IsNullOrEmpty(ip))
        {
            AppendLog("[错误] 请输入IP地址");
            return;
        }

        try
        {
            _tcp = new TcpClientService(ip, (int)nudTcpPort.Value, (int)nudTcpTimeout.Value);
            await _tcp.ConnectAsync();
            SetTcpConnected(true);
            AppendLog($"[TCP] 已连接 {ip}:{nudTcpPort.Value}");
        }
        catch (Exception ex)
        {
            AppendLog($"[错误] TCP连接失败: {ex.Message}");
            _tcp?.Dispose();
            _tcp = null;
        }
    }

    private void SetTcpConnected(bool connected)
    {
        btnTcpConnect.Text = connected ? "■ 断开" : "▶ 连接";
        btnTcpConnect.BackColor = connected ? AccentRed : Accent;
        lblTcpStatus.Text = connected ? "● 已连接" : "● 未连接";
        lblTcpStatus.ForeColor = connected ? AccentGreen : AccentRed;
        tbTcpIp.Enabled = !connected;
        nudTcpPort.Enabled = !connected;
        nudTcpTimeout.Enabled = !connected;

        tslConnection.Text = connected
            ? $"TCP {tbTcpIp.Text}:{nudTcpPort.Value}"
            : "未连接";
        tslConnection.ForeColor = connected ? AccentGreen : FgDim;
    }

    // ================================================================
    // 读取寄存器
    // ================================================================
    private async void BtnRead_Click(object? s, EventArgs e) => await ReadRegisters();

    private async Task ReadRegisters()
    {
        try
        {
            await _sendLock.WaitAsync();
            var slaveId = (byte)nudSlaveId.Value;
            var startAddr = (ushort)nudStartAddr.Value;
            var quantity = (ushort)nudQuantity.Value;

            var request = ModbusService.BuildReadHoldingRegisters(slaveId, startAddr, quantity);
            AppendLog($"[发] {BitConverter.ToString(request).Replace("-", " ")}");

            byte[] response;
            if (_serial is { IsOpen: true })
                response = await _serial.SendAsync(request);
            else if (_tcp is { Connected: true })
                response = await _tcp.SendAsync(request);
            else
            {
                AppendLog("[错误] 请先连接串口或TCP");
                return;
            }

            AppendLog($"[收] {BitConverter.ToString(response).Replace("-", " ")}");

            var values = ModbusService.ParseReadResponse(response, quantity);
            dgvRegisters.Rows.Clear();
            for (var i = 0; i < values.Length; i++)
            {
                dgvRegisters.Rows.Add(
                    (startAddr + i).ToString(),
                    $"0x{values[i]:X4}",
                    values[i].ToString(),
                    Convert.ToString(values[i], 2).PadLeft(16, '0')
                );
            }

            tslCount.Text = $"寄存器: {values.Length}";
            tslStatus.Text = "● 就绪";
            tslStatus.ForeColor = AccentGreen;
        }
        catch (Exception ex)
        {
            AppendLog($"[错误] {ex.Message}");
            tslStatus.Text = "● 错误";
            tslStatus.ForeColor = AccentRed;
        }
        finally
        {
            _sendLock.Release();
        }
    }

    // ================================================================
    // 自动读取
    // ================================================================
    private void ChkAutoRead_CheckedChanged(object? s, EventArgs e)
    {
        if (chkAutoRead.Checked)
        {
            _autoTimer = new System.Windows.Forms.Timer
            {
                Interval = (int)nudAutoInterval.Value
            };
            _autoTimer.Tick += async (_, _) => await ReadRegisters();
            _autoTimer.Start();
            nudAutoInterval.Enabled = false;
        }
        else
        {
            _autoTimer?.Stop();
            _autoTimer?.Dispose();
            _autoTimer = null;
            nudAutoInterval.Enabled = true;
        }
    }

    // ================================================================
    // 日志区
    // ================================================================
    private void AppendLog(string text)
    {
        var ts = DateTime.Now.ToString("HH:mm:ss.fff");
        if (chkPauseLog.Checked)
        {
            _pausedBuffer.Add($"[{ts}] {text}");
            return;
        }

        rtbLog.AppendText($"[{ts}] {text}\n");
        rtbLog.ScrollToCaret();
    }

    private void ChkHexLog_CheckedChanged(object? s, EventArgs e)
    {
        _hexLog = chkHexLog.Checked;
        AppendLog(_hexLog ? "[设置] HEX显示已开启" : "[设置] HEX显示已关闭");
    }

    private void ChkPauseLog_CheckedChanged(object? s, EventArgs e)
    {
        if (!chkPauseLog.Checked && _pausedBuffer.Count > 0)
        {
            foreach (var line in _pausedBuffer)
                rtbLog.AppendText(line + "\n");
            _pausedBuffer.Clear();
            rtbLog.ScrollToCaret();
        }
    }

    private void BtnClearLog_Click(object? s, EventArgs e)
    {
        rtbLog.Clear();
        _pausedBuffer.Clear();
    }

    private void BtnSaveLog_Click(object? s, EventArgs e)
    {
        using var sfd = new SaveFileDialog
        {
            Filter = "日志文件|*.txt",
            FileName = $"ModbusLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
        };
        if (sfd.ShowDialog() == DialogResult.OK)
        {
            File.WriteAllText(sfd.FileName, rtbLog.Text);
            AppendLog($"[保存] 日志已保存至 {sfd.FileName}");
        }
    }

    private void dgvRegisters_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
    }
}
