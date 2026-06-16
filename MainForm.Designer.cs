namespace ModbusTool;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null!;

    // 左侧面板
    private Panel pnlLeft;
    private TabControl tabConnection;
    private TabPage tabSerial;
    private TabPage tabTcp;

    // 串口
    private GroupBox gbSerial;
    private Label lblPort;
    private ComboBox cbPort;
    private Button btnPortRefresh;
    private Label lblBaud;
    private ComboBox cbBaud;
    private Label lblData;
    private ComboBox cbData;
    private Label lblStop;
    private ComboBox cbStop;
    private Label lblParity;
    private ComboBox cbParity;
    private Button btnSerialOpen;
    private Label lblSerialStatus;

    // TCP
    private GroupBox gbTcp;
    private Label lblTcpIp;
    private TextBox tbTcpIp;
    private Label lblTcpPort;
    private NumericUpDown nudTcpPort;
    private Label lblTcpTimeout;
    private NumericUpDown nudTcpTimeout;
    private Button btnTcpConnect;
    private Label lblTcpStatus;

    // 读取参数
    private GroupBox gbRead;
    private Label lblSlaveId;
    private NumericUpDown nudSlaveId;
    private Label lblStartAddr;
    private NumericUpDown nudStartAddr;
    private Label lblQuantity;
    private NumericUpDown nudQuantity;
    private Button btnRead;
    private CheckBox chkAutoRead;
    private Label lblAutoInterval;
    private NumericUpDown nudAutoInterval;

    // 右侧容器
    private Panel pnlRight;

    // 数据表格
    private DataGridView dgvRegisters;
    private DataGridViewTextBoxColumn colAddr;
    private DataGridViewTextBoxColumn colHex;
    private DataGridViewTextBoxColumn colDec;
    private DataGridViewTextBoxColumn colBin;

    // 工具栏
    private Panel pnlToolbar;
    private CheckBox chkHexLog;
    private CheckBox chkPauseLog;
    private Button btnClearLog;
    private Button btnSaveLog;

    // 日志
    private RichTextBox rtbLog;

    // 状态栏
    private StatusStrip statusStrip;
    private ToolStripStatusLabel tslStatus;
    private ToolStripStatusLabel tslConnection;
    private ToolStripStatusLabel tslCount;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        statusStrip    = new StatusStrip();
        tslStatus      = new ToolStripStatusLabel();
        tslConnection  = new ToolStripStatusLabel();
        tslCount       = new ToolStripStatusLabel();
        pnlLeft        = new Panel();
        tabConnection  = new TabControl();
        tabSerial      = new TabPage();
        gbSerial       = new GroupBox();
        lblPort        = new Label();
        cbPort         = new ComboBox();
        btnPortRefresh = new Button();
        lblBaud        = new Label();
        cbBaud         = new ComboBox();
        lblData        = new Label();
        cbData         = new ComboBox();
        lblStop        = new Label();
        cbStop         = new ComboBox();
        lblParity      = new Label();
        cbParity       = new ComboBox();
        btnSerialOpen  = new Button();
        lblSerialStatus= new Label();
        tabTcp         = new TabPage();
        gbTcp          = new GroupBox();
        lblTcpIp       = new Label();
        tbTcpIp        = new TextBox();
        lblTcpPort     = new Label();
        nudTcpPort     = new NumericUpDown();
        lblTcpTimeout  = new Label();
        nudTcpTimeout  = new NumericUpDown();
        btnTcpConnect  = new Button();
        lblTcpStatus   = new Label();
        gbRead         = new GroupBox();
        lblSlaveId     = new Label();
        nudSlaveId     = new NumericUpDown();
        lblStartAddr   = new Label();
        nudStartAddr   = new NumericUpDown();
        lblQuantity    = new Label();
        nudQuantity    = new NumericUpDown();
        btnRead        = new Button();
        chkAutoRead    = new CheckBox();
        lblAutoInterval= new Label();
        nudAutoInterval= new NumericUpDown();
        pnlRight       = new Panel();
        dgvRegisters   = new DataGridView();
        colAddr        = new DataGridViewTextBoxColumn();
        colHex         = new DataGridViewTextBoxColumn();
        colDec         = new DataGridViewTextBoxColumn();
        colBin         = new DataGridViewTextBoxColumn();
        pnlToolbar     = new Panel();
        chkHexLog      = new CheckBox();
        chkPauseLog    = new CheckBox();
        btnClearLog    = new Button();
        btnSaveLog     = new Button();
        rtbLog         = new RichTextBox();

        // ============================================================
        // 颜色方案
        // ============================================================
        var bgDark        = Color.FromArgb(30, 30, 38);
        var bgPanel       = Color.FromArgb(40, 40, 50);
        var bgGroup       = Color.FromArgb(48, 48, 60);
        var bgInput       = Color.FromArgb(55, 55, 70);
        var accent        = Color.FromArgb(0, 150, 255);
        var accentGreen   = Color.FromArgb(40, 200, 120);
        var accentRed     = Color.FromArgb(240, 70, 80);
        var fgMain        = Color.FromArgb(220, 220, 230);
        var fgDim         = Color.FromArgb(160, 160, 175);
        var fgBright      = Color.FromArgb(240, 240, 245);

        // ============================================================
        // 样式 Helper
        // ============================================================
        void StyleLabel(Label l, string t) { l.Text = t; l.ForeColor = fgDim; l.BackColor = Color.Transparent; l.Font = new Font("Microsoft YaHei UI", 9F); l.AutoSize = true; }
        void StyleCombo(ComboBox c) { c.BackColor = bgInput; c.ForeColor = fgMain; c.Font = new Font("Consolas", 9.5F); c.FlatStyle = FlatStyle.Flat; c.DropDownHeight = 150; }
        void StyleNum(NumericUpDown n, int min, int max, int v) { n.Minimum = min; n.Maximum = max; n.Value = v; n.BackColor = bgInput; n.ForeColor = fgMain; n.Font = new Font("Consolas", 9.5F); n.BorderStyle = BorderStyle.FixedSingle; }
        void StyleBtn(Button b, string t, Color c) { b.Text = t; b.BackColor = c; b.ForeColor = fgBright; b.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold); b.FlatStyle = FlatStyle.Flat; b.FlatAppearance.BorderSize = 0; b.Cursor = Cursors.Hand; }
        void StyleGb(GroupBox g, string t) { g.Text = t; g.ForeColor = accent; g.BackColor = bgGroup; g.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold); }
        void StyleChk(CheckBox c, string t) { c.Text = t; c.ForeColor = fgMain; c.BackColor = Color.Transparent; c.Font = new Font("Microsoft YaHei UI", 9F); c.AutoSize = true; c.FlatStyle = FlatStyle.Flat; }

        // ============================================================
        // pnlLeft — 固定宽度 280，不可拖动
        // ============================================================
        pnlLeft.Dock = DockStyle.Left;
        pnlLeft.Width = 280;
        pnlLeft.BackColor = bgPanel;
        pnlLeft.Padding = new Padding(8, 8, 4, 8);

        // ============================================================
        // tabConnection — 连接选项卡（左侧上半部分，固定高度 270）
        // ============================================================
        tabConnection.Dock = DockStyle.Top;
        tabConnection.Height = 310;
        tabConnection.ItemSize = new Size(60, 28);
        tabConnection.SizeMode = TabSizeMode.Fixed;
        tabConnection.BackColor = bgPanel;
        tabConnection.Font = new Font("Microsoft YaHei UI", 9.5F);
        tabConnection.Padding = new Point(8, 4);
        tabConnection.SelectedIndexChanged += TabConnection_SelectedIndexChanged;

        // ---- 串口页 ----
        tabSerial.Text = "串口";
        tabSerial.BackColor = bgPanel;
        tabSerial.Padding = new Padding(6);

        StyleGb(gbSerial, "  串口配置");
        gbSerial.Dock = DockStyle.Fill;
        gbSerial.Padding = new Padding(10, 16, 10, 6);

        StyleLabel(lblPort, "端口");
        lblPort.Location = new Point(12, 22);
        cbPort.Location = new Point(12, 40);
        cbPort.Size = new Size(145, 28);
        StyleCombo(cbPort);
        btnPortRefresh.Location = new Point(163, 38);
        btnPortRefresh.Size = new Size(70, 28);
        StyleBtn(btnPortRefresh, "刷新", Color.FromArgb(70, 70, 90));
        btnPortRefresh.Click += BtnPortRefresh_Click;

        StyleLabel(lblBaud, "波特率");
        lblBaud.Location = new Point(12, 70);
        cbBaud.Location = new Point(12, 90);
        cbBaud.Size = new Size(145, 28);
        StyleCombo(cbBaud);
        cbBaud.Items.AddRange(new object[] { "1200", "2400", "4800", "9600", "19200", "38400", "57600", "115200" });
        cbBaud.SelectedIndex = 3;

        StyleLabel(lblData, "数据位");
        lblData.Location = new Point(163, 70);
        cbData.Location = new Point(163, 90);
        cbData.Size = new Size(70, 28);
        StyleCombo(cbData);
        cbData.Items.AddRange(new object[] { "6", "7", "8" });
        cbData.SelectedIndex = 2;

        StyleLabel(lblStop, "停止位");
        lblStop.Location = new Point(12, 124);
        cbStop.Location = new Point(12, 144);
        cbStop.Size = new Size(100, 28);
        StyleCombo(cbStop);
        cbStop.Items.AddRange(new object[] { "1", "2" });
        cbStop.SelectedIndex = 0;

        StyleLabel(lblParity, "校验");
        lblParity.Location = new Point(120, 124);
        cbParity.Location = new Point(120, 144);
        cbParity.Size = new Size(113, 28);
        StyleCombo(cbParity);
        cbParity.Items.AddRange(new object[] { "None", "Odd", "Even", "Mark", "Space" });
        cbParity.SelectedIndex = 0;

        btnSerialOpen.Location = new Point(12, 176);
        btnSerialOpen.Size = new Size(221, 34);
        StyleBtn(btnSerialOpen, "▶ 打开串口", accent);
        btnSerialOpen.Click += BtnSerialOpen_Click;

        lblSerialStatus.Location = new Point(12, 214);
        lblSerialStatus.AutoSize = true;
        lblSerialStatus.Font = new Font("Microsoft YaHei UI", 8.5F);
        lblSerialStatus.ForeColor = accentRed;
        lblSerialStatus.BackColor = Color.Transparent;
        lblSerialStatus.Text = "● 未连接";

        gbSerial.Controls.AddRange(new Control[] { lblPort, cbPort, btnPortRefresh, lblBaud, cbBaud, lblData, cbData, lblStop, cbStop, lblParity, cbParity, btnSerialOpen, lblSerialStatus });
        tabSerial.Controls.Add(gbSerial);

        // ---- TCP 页 ----
        tabTcp.Text = "TCP";
        tabTcp.BackColor = bgPanel;
        tabTcp.Padding = new Padding(6);

        StyleGb(gbTcp, "  TCP 配置");
        gbTcp.Dock = DockStyle.Fill;
        gbTcp.Padding = new Padding(10, 16, 10, 6);

        StyleLabel(lblTcpIp, "IP 地址");
        lblTcpIp.Location = new Point(12, 22);
        tbTcpIp.Location = new Point(12, 40);
        tbTcpIp.Size = new Size(221, 28);
        tbTcpIp.BackColor = bgInput;
        tbTcpIp.ForeColor = fgMain;
        tbTcpIp.Font = new Font("Consolas", 9.5F);
        tbTcpIp.BorderStyle = BorderStyle.FixedSingle;
        tbTcpIp.Text = "127.0.0.1";

        StyleLabel(lblTcpPort, "端口");
        lblTcpPort.Location = new Point(12, 74);
        nudTcpPort.Location = new Point(12, 94);
        nudTcpPort.Size = new Size(110, 28);
        StyleNum(nudTcpPort, 1, 65535, 502);

        StyleLabel(lblTcpTimeout, "超时(ms)");
        lblTcpTimeout.Location = new Point(130, 74);
        nudTcpTimeout.Location = new Point(130, 94);
        nudTcpTimeout.Size = new Size(103, 28);
        StyleNum(nudTcpTimeout, 100, 30000, 3000);

        btnTcpConnect.Location = new Point(12, 134);
        btnTcpConnect.Size = new Size(221, 34);
        StyleBtn(btnTcpConnect, "▶ 连接", accent);
        btnTcpConnect.Click += BtnTcpConnect_Click;

        lblTcpStatus.Location = new Point(12, 172);
        lblTcpStatus.AutoSize = true;
        lblTcpStatus.Font = new Font("Microsoft YaHei UI", 8.5F);
        lblTcpStatus.ForeColor = accentRed;
        lblTcpStatus.BackColor = Color.Transparent;
        lblTcpStatus.Text = "● 未连接";

        gbTcp.Controls.AddRange(new Control[] { lblTcpIp, tbTcpIp, lblTcpPort, nudTcpPort, lblTcpTimeout, nudTcpTimeout, btnTcpConnect, lblTcpStatus });
        tabTcp.Controls.Add(gbTcp);

        tabConnection.Controls.Add(tabSerial);
        tabConnection.Controls.Add(tabTcp);

        // ============================================================
        // gbRead — 读取参数（左侧下半部分，填满剩余空间）
        // ============================================================
        StyleGb(gbRead, "  读取参数");
        gbRead.Dock = DockStyle.Fill;
        gbRead.Padding = new Padding(10, 16, 10, 6);

        StyleLabel(lblSlaveId, "从站 ID");
        lblSlaveId.Location = new Point(12, 22);
        nudSlaveId.Location = new Point(12, 42);
        nudSlaveId.Size = new Size(100, 28);
        StyleNum(nudSlaveId, 1, 247, 1);

        StyleLabel(lblStartAddr, "起始地址");
        lblStartAddr.Location = new Point(120, 22);
        nudStartAddr.Location = new Point(120, 42);
        nudStartAddr.Size = new Size(113, 28);
        StyleNum(nudStartAddr, 0, 65535, 0);

        StyleLabel(lblQuantity, "数量");
        lblQuantity.Location = new Point(12, 76);
        nudQuantity.Location = new Point(12, 96);
        nudQuantity.Size = new Size(100, 28);
        StyleNum(nudQuantity, 1, 125, 10);

        btnRead.Location = new Point(120, 96);
        btnRead.Size = new Size(113, 32);
        StyleBtn(btnRead, "▶ 读取", accentGreen);
        btnRead.Click += BtnRead_Click;

        StyleChk(chkAutoRead, "自动轮询");
        chkAutoRead.Location = new Point(12, 136);
        chkAutoRead.CheckedChanged += ChkAutoRead_CheckedChanged;

        StyleLabel(lblAutoInterval, "间隔(ms)");
        lblAutoInterval.Location = new Point(100, 138);
        nudAutoInterval.Location = new Point(160, 136);
        nudAutoInterval.Size = new Size(73, 28);
        StyleNum(nudAutoInterval, 100, 60000, 1000);

        gbRead.Controls.AddRange(new Control[] { lblSlaveId, nudSlaveId, lblStartAddr, nudStartAddr, lblQuantity, nudQuantity, btnRead, chkAutoRead, lblAutoInterval, nudAutoInterval });

        // ---- 组装左侧 ----
        pnlLeft.Controls.Add(gbRead);
        pnlLeft.Controls.Add(tabConnection);

        // ============================================================
        // pnlRight — 右侧容器（Dock=Fill，不可拖动）
        // ============================================================
        pnlRight.Dock = DockStyle.Fill;
        pnlRight.BackColor = bgDark;
        pnlRight.Padding = new Padding(0, 0, 0, 0);

        // ============================================================
        // dgvRegisters — 数据表格（填满右侧上部）
        // ============================================================
        dgvRegisters.Dock = DockStyle.Fill;
        dgvRegisters.BackgroundColor = bgDark;
        dgvRegisters.BorderStyle = BorderStyle.None;
        dgvRegisters.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvRegisters.EnableHeadersVisualStyles = false;
        dgvRegisters.GridColor = Color.FromArgb(50, 50, 65);
        dgvRegisters.Font = new Font("Consolas", 9.5F);
        dgvRegisters.RowHeadersVisible = false;
        dgvRegisters.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvRegisters.AllowUserToAddRows = false;
        dgvRegisters.AllowUserToResizeRows = false;
        dgvRegisters.MultiSelect = false;
        dgvRegisters.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 58);
        dgvRegisters.ColumnHeadersDefaultCellStyle.ForeColor = fgDim;
        dgvRegisters.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
        dgvRegisters.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 45, 58);
        dgvRegisters.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvRegisters.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dgvRegisters.ColumnHeadersHeight = 32;
        dgvRegisters.DefaultCellStyle.BackColor = bgDark;
        dgvRegisters.DefaultCellStyle.ForeColor = fgMain;
        dgvRegisters.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 90, 160);
        dgvRegisters.DefaultCellStyle.SelectionForeColor = fgBright;
        dgvRegisters.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvRegisters.DefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
        dgvRegisters.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(35, 35, 45);
        dgvRegisters.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        dgvRegisters.CellContentClick += dgvRegisters_CellContentClick;

        colAddr.HeaderText = "地址";
        colAddr.Width = 70;
        colAddr.ReadOnly = true;
        colAddr.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        colAddr.Name = "colAddr";

        colHex.HeaderText = "十六进制 (HEX)";
        colHex.Width = 120;
        colHex.MinimumWidth = 80;
        colHex.ReadOnly = true;
        colHex.Name = "colHex";

        colDec.HeaderText = "十进制";
        colDec.Width = 90;
        colDec.MinimumWidth = 70;
        colDec.ReadOnly = true;
        colDec.Name = "colDec";

        colBin.HeaderText = "二进制";
        colBin.Width = 180;
        colBin.MinimumWidth = 140;
        colBin.ReadOnly = true;
        colBin.Name = "colBin";

        dgvRegisters.Columns.AddRange(colAddr, colHex, colDec, colBin);

        // ============================================================
        // rtbLog — 日志区（固定底部 180px）
        // ============================================================
        rtbLog.Dock = DockStyle.Bottom;
        rtbLog.Height = 180;
        rtbLog.BackColor = Color.FromArgb(22, 22, 30);
        rtbLog.ForeColor = Color.FromArgb(120, 220, 140);
        rtbLog.Font = new Font("Consolas", 9F);
        rtbLog.BorderStyle = BorderStyle.None;
        rtbLog.ReadOnly = true;
        rtbLog.DetectUrls = false;

        // ============================================================
        // pnlToolbar — 日志工具栏（紧贴日志上方，用 Anchor 靠右）
        // ============================================================
        pnlToolbar.Dock = DockStyle.Bottom;
        pnlToolbar.Height = 36;
        pnlToolbar.BackColor = bgGroup;

        StyleChk(chkHexLog, "HEX显示");
        chkHexLog.Location = new Point(12, 7);
        chkHexLog.CheckedChanged += ChkHexLog_CheckedChanged;

        StyleChk(chkPauseLog, "暂停");
        chkPauseLog.Location = new Point(112, 7);
        chkPauseLog.CheckedChanged += ChkPauseLog_CheckedChanged;

        btnClearLog.Size = new Size(72, 26);
        StyleBtn(btnClearLog, "清空", Color.FromArgb(70, 70, 90));
        btnClearLog.Anchor = AnchorStyles.Right;
        btnClearLog.Click += BtnClearLog_Click;

        btnSaveLog.Size = new Size(72, 26);
        StyleBtn(btnSaveLog, "保存", accent);
        btnSaveLog.Anchor = AnchorStyles.Right;
        btnSaveLog.Click += BtnSaveLog_Click;

        pnlToolbar.Controls.Add(chkHexLog);
        pnlToolbar.Controls.Add(chkPauseLog);
        pnlToolbar.Controls.Add(btnClearLog);
        pnlToolbar.Controls.Add(btnSaveLog);

        // 用 Layout 事件动态定位右侧按钮
        pnlToolbar.Layout += (s, e) =>
        {
            btnSaveLog.Location = new Point(pnlToolbar.ClientSize.Width - btnSaveLog.Width - 12, 5);
            btnClearLog.Location = new Point(pnlToolbar.ClientSize.Width - btnSaveLog.Width - btnClearLog.Width - 20, 5);
        };

        // ---- 组装右侧 ----
        pnlRight.Controls.Add(dgvRegisters);
        pnlRight.Controls.Add(rtbLog);
        pnlRight.Controls.Add(pnlToolbar);

        // ============================================================
        // statusStrip
        // ============================================================
        statusStrip.BackColor = Color.FromArgb(25, 25, 33);
        statusStrip.Font = new Font("Microsoft YaHei UI", 8.5F);
        statusStrip.SizingGrip = false;

        tslStatus.ForeColor = fgDim;
        tslStatus.Margin = new Padding(8, 0, 8, 0);
        tslStatus.Text = "● 就绪";

        tslConnection.ForeColor = accent;
        tslConnection.Margin = new Padding(8, 0, 8, 0);
        tslConnection.Spring = true;
        tslConnection.Text = "未连接";

        tslCount.ForeColor = fgDim;
        tslCount.Margin = new Padding(8, 0, 8, 0);
        tslCount.Text = "寄存器: 0";

        statusStrip.Items.AddRange(new ToolStripItem[] { tslStatus, tslConnection, tslCount });

        // ============================================================
        // MainForm
        // ============================================================
        ClientSize = new Size(1024, 640);
        Controls.Add(pnlRight);
        Controls.Add(pnlLeft);
        Controls.Add(statusStrip);
        BackColor = bgDark;
        ForeColor = fgMain;
        Font = new Font("Microsoft YaHei UI", 9F);
        MinimumSize = new Size(800, 500);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Modbus 调试工具 v2.0";

        // TabControl OwnerDraw
        tabConnection.DrawMode = TabDrawMode.OwnerDrawFixed;
        tabConnection.DrawItem += TabConnection_DrawItem;
    }

    private void TabConnection_DrawItem(object sender, DrawItemEventArgs e)
    {
        if (sender is not TabControl tc) return;
        var rect = tc.GetTabRect(e.Index);
        var selected = e.Index == tc.SelectedIndex;
        var bgBrush = selected ? new SolidBrush(Color.FromArgb(48, 48, 60)) : new SolidBrush(Color.FromArgb(40, 40, 50));
        var fgBrush = selected ? new SolidBrush(Color.FromArgb(0, 150, 255)) : new SolidBrush(Color.FromArgb(160, 160, 175));
        using (bgBrush) using (fgBrush)
        {
            e.Graphics.FillRectangle(bgBrush, rect);
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            e.Graphics.DrawString(tc.TabPages[e.Index].Text, tc.Font, fgBrush, rect, sf);
        }
    }
}
