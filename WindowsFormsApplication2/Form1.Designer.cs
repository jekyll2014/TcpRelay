using System.Windows.Forms;
namespace TcpRelay
{
    partial class FormMain
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBox_terminal1 = new System.Windows.Forms.TextBox();
            this.button_start = new System.Windows.Forms.Button();
            this.button_stop = new System.Windows.Forms.Button();
            this.button_clear1 = new System.Windows.Forms.Button();
            this.checkBox_insTime = new System.Windows.Forms.CheckBox();
            this.checkBox_ServerHex = new System.Windows.Forms.CheckBox();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.checkBox_insDir = new System.Windows.Forms.CheckBox();
            this.checkBox_ClientHex = new System.Windows.Forms.CheckBox();
            this.label_dispHex = new System.Windows.Forms.Label();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.checkBox_Mark = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBox_clientName = new System.Windows.Forms.TextBox();
            this.textBox_serverName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_insPin = new System.Windows.Forms.CheckBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveTXTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logToGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logToTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoscrollToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineWrapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autosaveTXTToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.terminaltxtToolStripMenuItem1 = new System.Windows.Forms.ToolStripTextBox();
            this.autosaveCSVToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.terminalcsvToolStripMenuItem1 = new System.Windows.Forms.ToolStripTextBox();
            this.LineBreakToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.LineBreakToolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.saveParametersToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox_serverIP = new System.Windows.Forms.TextBox();
            this.textBox_clientPort = new System.Windows.Forms.TextBox();
            this.textBox_serverPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_clientIP = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_terminal1
            // 
            this.textBox_terminal1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox_terminal1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_terminal1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_terminal1.HideSelection = false;
            this.textBox_terminal1.Location = new System.Drawing.Point(3, 3);
            this.textBox_terminal1.MaxLength = 20480000;
            this.textBox_terminal1.Multiline = true;
            this.textBox_terminal1.Name = "textBox_terminal1";
            this.textBox_terminal1.ReadOnly = true;
            this.textBox_terminal1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_terminal1.Size = new System.Drawing.Size(666, 212);
            this.textBox_terminal1.TabIndex = 50;
            // 
            // button_start
            // 
            this.button_start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_start.Location = new System.Drawing.Point(259, 294);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(68, 25);
            this.button_start.TabIndex = 14;
            this.button_start.Text = "Start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // button_stop
            // 
            this.button_stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_stop.Enabled = false;
            this.button_stop.Location = new System.Drawing.Point(259, 325);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(67, 25);
            this.button_stop.TabIndex = 34;
            this.button_stop.Text = "Stop";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // button_clear1
            // 
            this.button_clear1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_clear1.Location = new System.Drawing.Point(333, 294);
            this.button_clear1.Name = "button_clear1";
            this.button_clear1.Size = new System.Drawing.Size(67, 25);
            this.button_clear1.TabIndex = 30;
            this.button_clear1.Text = "Clear";
            this.button_clear1.UseVisualStyleBackColor = true;
            this.button_clear1.Click += new System.EventHandler(this.button_clear1_Click);
            // 
            // checkBox_insTime
            // 
            this.checkBox_insTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_insTime.AutoSize = true;
            this.checkBox_insTime.Checked = true;
            this.checkBox_insTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_insTime.Location = new System.Drawing.Point(129, 19);
            this.checkBox_insTime.Name = "checkBox_insTime";
            this.checkBox_insTime.Size = new System.Drawing.Size(45, 17);
            this.checkBox_insTime.TabIndex = 2;
            this.checkBox_insTime.Text = "time";
            this.checkBox_insTime.UseVisualStyleBackColor = true;
            // 
            // checkBox_ServerHex
            // 
            this.checkBox_ServerHex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_ServerHex.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox_ServerHex.Location = new System.Drawing.Point(406, 310);
            this.checkBox_ServerHex.Name = "checkBox_ServerHex";
            this.checkBox_ServerHex.Size = new System.Drawing.Size(76, 17);
            this.checkBox_ServerHex.TabIndex = 27;
            this.checkBox_ServerHex.Text = "Server";
            this.checkBox_ServerHex.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBox_ServerHex.UseVisualStyleBackColor = true;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "txt";
            this.saveFileDialog.FileName = "terminal.txt";
            this.saveFileDialog.Filter = "Text files|*.txt|All files|*.*";
            this.saveFileDialog.RestoreDirectory = true;
            this.saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // checkBox_insDir
            // 
            this.checkBox_insDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_insDir.AutoSize = true;
            this.checkBox_insDir.Checked = true;
            this.checkBox_insDir.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_insDir.Location = new System.Drawing.Point(129, 42);
            this.checkBox_insDir.Name = "checkBox_insDir";
            this.checkBox_insDir.Size = new System.Drawing.Size(66, 17);
            this.checkBox_insDir.TabIndex = 3;
            this.checkBox_insDir.Text = "direction";
            this.checkBox_insDir.UseVisualStyleBackColor = true;
            // 
            // checkBox_ClientHex
            // 
            this.checkBox_ClientHex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_ClientHex.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox_ClientHex.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBox_ClientHex.Location = new System.Drawing.Point(406, 333);
            this.checkBox_ClientHex.Name = "checkBox_ClientHex";
            this.checkBox_ClientHex.Size = new System.Drawing.Size(76, 17);
            this.checkBox_ClientHex.TabIndex = 28;
            this.checkBox_ClientHex.Text = "Client";
            this.checkBox_ClientHex.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBox_ClientHex.UseVisualStyleBackColor = true;
            // 
            // label_dispHex
            // 
            this.label_dispHex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_dispHex.AutoSize = true;
            this.label_dispHex.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_dispHex.Location = new System.Drawing.Point(403, 294);
            this.label_dispHex.Name = "label_dispHex";
            this.label_dispHex.Size = new System.Drawing.Size(76, 13);
            this.label_dispHex.TabIndex = 3;
            this.label_dispHex.Text = "Display hex:";
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.dataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dataGridView.Location = new System.Drawing.Point(3, 3);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowTemplate.ReadOnly = true;
            this.dataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView.Size = new System.Drawing.Size(666, 289);
            this.dataGridView.StandardTab = true;
            this.dataGridView.TabIndex = 51;
            this.dataGridView.VirtualMode = true;
            // 
            // checkBox_Mark
            // 
            this.checkBox_Mark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_Mark.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox_Mark.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkBox_Mark.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox_Mark.Location = new System.Drawing.Point(333, 325);
            this.checkBox_Mark.Name = "checkBox_Mark";
            this.checkBox_Mark.Size = new System.Drawing.Size(67, 25);
            this.checkBox_Mark.TabIndex = 29;
            this.checkBox_Mark.Text = "Mark";
            this.checkBox_Mark.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_Mark.UseVisualStyleBackColor = true;
            this.checkBox_Mark.CheckedChanged += new System.EventHandler(this.checkBox_Mark_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(680, 244);
            this.tabControl1.TabIndex = 32;
            this.tabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl1_Selecting);
            // 
            // tabPage1
            // 
            this.tabPage1.AutoScroll = true;
            this.tabPage1.Controls.Add(this.textBox_terminal1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(672, 218);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "Text";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.AutoScroll = true;
            this.tabPage2.Controls.Add(this.dataGridView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(672, 295);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Table";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBox_clientName
            // 
            this.textBox_clientName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_clientName.Location = new System.Drawing.Point(188, 330);
            this.textBox_clientName.MaxLength = 50;
            this.textBox_clientName.Name = "textBox_clientName";
            this.textBox_clientName.Size = new System.Drawing.Size(65, 20);
            this.textBox_clientName.TabIndex = 6;
            this.textBox_clientName.Text = "FN";
            this.textBox_clientName.Leave += new System.EventHandler(this.textBox_clientName_Leave);
            // 
            // textBox_serverName
            // 
            this.textBox_serverName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_serverName.Location = new System.Drawing.Point(188, 290);
            this.textBox_serverName.MaxLength = 50;
            this.textBox_serverName.Name = "textBox_serverName";
            this.textBox_serverName.Size = new System.Drawing.Size(65, 20);
            this.textBox_serverName.TabIndex = 13;
            this.textBox_serverName.Text = "OFD";
            this.textBox_serverName.Leave += new System.EventHandler(this.textBox_serverName_Leave);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(117, 314);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Client port";
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(185, 314);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(62, 13);
            this.label16.TabIndex = 3;
            this.label16.Text = "Client name";
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(117, 275);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(59, 13);
            this.label17.TabIndex = 3;
            this.label17.Text = "Server port";
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(185, 274);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(67, 13);
            this.label18.TabIndex = 3;
            this.label18.Text = "Server name";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBox_insPin);
            this.groupBox1.Controls.Add(this.checkBox_insTime);
            this.groupBox1.Controls.Add(this.checkBox_insDir);
            this.groupBox1.Location = new System.Drawing.Point(491, 283);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(201, 67);
            this.groupBox1.TabIndex = 60;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log options";
            // 
            // checkBox_insPin
            // 
            this.checkBox_insPin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_insPin.AutoSize = true;
            this.checkBox_insPin.Checked = true;
            this.checkBox_insPin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_insPin.Location = new System.Drawing.Point(23, 19);
            this.checkBox_insPin.Name = "checkBox_insPin";
            this.checkBox_insPin.Size = new System.Drawing.Size(76, 17);
            this.checkBox_insPin.TabIndex = 4;
            this.checkBox_insPin.Text = "Port status";
            this.checkBox_insPin.UseVisualStyleBackColor = true;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(704, 24);
            this.menuStrip.TabIndex = 35;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveTXTToolStripMenuItem,
            this.saveCSVToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveTXTToolStripMenuItem
            // 
            this.saveTXTToolStripMenuItem.Name = "saveTXTToolStripMenuItem";
            this.saveTXTToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveTXTToolStripMenuItem.Text = "Save .TXT";
            this.saveTXTToolStripMenuItem.Click += new System.EventHandler(this.saveTXTToolStripMenuItem_Click);
            // 
            // saveCSVToolStripMenuItem
            // 
            this.saveCSVToolStripMenuItem.Name = "saveCSVToolStripMenuItem";
            this.saveCSVToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveCSVToolStripMenuItem.Text = "Save .CSV";
            this.saveCSVToolStripMenuItem.Click += new System.EventHandler(this.saveCSVToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logToGridToolStripMenuItem,
            this.logToTextToolStripMenuItem,
            this.autoscrollToolStripMenuItem,
            this.lineWrapToolStripMenuItem,
            this.autosaveTXTToolStripMenuItem1,
            this.autosaveCSVToolStripMenuItem1,
            this.LineBreakToolStripMenuItem1,
            this.saveParametersToolStripMenuItem1});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // logToGridToolStripMenuItem
            // 
            this.logToGridToolStripMenuItem.Checked = true;
            this.logToGridToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.logToGridToolStripMenuItem.Name = "logToGridToolStripMenuItem";
            this.logToGridToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.logToGridToolStripMenuItem.Text = "Log to table";
            this.logToGridToolStripMenuItem.Click += new System.EventHandler(this.logToGridToolStripMenuItem_Click);
            // 
            // logToTextToolStripMenuItem
            // 
            this.logToTextToolStripMenuItem.Checked = true;
            this.logToTextToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.logToTextToolStripMenuItem.Name = "logToTextToolStripMenuItem";
            this.logToTextToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.logToTextToolStripMenuItem.Text = "Log to text";
            this.logToTextToolStripMenuItem.Click += new System.EventHandler(this.logToTextToolStripMenuItem_Click);
            // 
            // autoscrollToolStripMenuItem
            // 
            this.autoscrollToolStripMenuItem.Checked = true;
            this.autoscrollToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoscrollToolStripMenuItem.Enabled = false;
            this.autoscrollToolStripMenuItem.Name = "autoscrollToolStripMenuItem";
            this.autoscrollToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.autoscrollToolStripMenuItem.Text = "Autoscroll";
            this.autoscrollToolStripMenuItem.Click += new System.EventHandler(this.autoscrollToolStripMenuItem_Click);
            // 
            // lineWrapToolStripMenuItem
            // 
            this.lineWrapToolStripMenuItem.Checked = true;
            this.lineWrapToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.lineWrapToolStripMenuItem.Name = "lineWrapToolStripMenuItem";
            this.lineWrapToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.lineWrapToolStripMenuItem.Text = "Line wrap";
            this.lineWrapToolStripMenuItem.Click += new System.EventHandler(this.lineWrapToolStripMenuItem_Click);
            // 
            // autosaveTXTToolStripMenuItem1
            // 
            this.autosaveTXTToolStripMenuItem1.Checked = true;
            this.autosaveTXTToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autosaveTXTToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.terminaltxtToolStripMenuItem1});
            this.autosaveTXTToolStripMenuItem1.Name = "autosaveTXTToolStripMenuItem1";
            this.autosaveTXTToolStripMenuItem1.Size = new System.Drawing.Size(200, 22);
            this.autosaveTXTToolStripMenuItem1.Text = "Autosave .TXT";
            this.autosaveTXTToolStripMenuItem1.Click += new System.EventHandler(this.autosaveTXTToolStripMenuItem1_Click);
            // 
            // terminaltxtToolStripMenuItem1
            // 
            this.terminaltxtToolStripMenuItem1.Name = "terminaltxtToolStripMenuItem1";
            this.terminaltxtToolStripMenuItem1.Size = new System.Drawing.Size(152, 23);
            this.terminaltxtToolStripMenuItem1.Text = "terminal.txt";
            // 
            // autosaveCSVToolStripMenuItem1
            // 
            this.autosaveCSVToolStripMenuItem1.Checked = true;
            this.autosaveCSVToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autosaveCSVToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.terminalcsvToolStripMenuItem1});
            this.autosaveCSVToolStripMenuItem1.Name = "autosaveCSVToolStripMenuItem1";
            this.autosaveCSVToolStripMenuItem1.Size = new System.Drawing.Size(200, 22);
            this.autosaveCSVToolStripMenuItem1.Text = "Autosave .CSV";
            this.autosaveCSVToolStripMenuItem1.Click += new System.EventHandler(this.autosaveCSVToolStripMenuItem1_Click);
            // 
            // terminalcsvToolStripMenuItem1
            // 
            this.terminalcsvToolStripMenuItem1.Name = "terminalcsvToolStripMenuItem1";
            this.terminalcsvToolStripMenuItem1.Size = new System.Drawing.Size(152, 23);
            this.terminalcsvToolStripMenuItem1.Text = "terminal.csv";
            // 
            // LineBreakToolStripMenuItem1
            // 
            this.LineBreakToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LineBreakToolStripTextBox1});
            this.LineBreakToolStripMenuItem1.Name = "LineBreakToolStripMenuItem1";
            this.LineBreakToolStripMenuItem1.Size = new System.Drawing.Size(200, 22);
            this.LineBreakToolStripMenuItem1.Text = "Line break timeout [ms]";
            // 
            // LineBreakToolStripTextBox1
            // 
            this.LineBreakToolStripTextBox1.MaxLength = 5;
            this.LineBreakToolStripTextBox1.Name = "LineBreakToolStripTextBox1";
            this.LineBreakToolStripTextBox1.Size = new System.Drawing.Size(100, 23);
            this.LineBreakToolStripTextBox1.Text = "1000";
            this.LineBreakToolStripTextBox1.TextChanged += new System.EventHandler(this.toolStripTextBox1_TextChanged);
            // 
            // saveParametersToolStripMenuItem1
            // 
            this.saveParametersToolStripMenuItem1.Name = "saveParametersToolStripMenuItem1";
            this.saveParametersToolStripMenuItem1.Size = new System.Drawing.Size(200, 22);
            this.saveParametersToolStripMenuItem1.Text = "Save parameters";
            this.saveParametersToolStripMenuItem1.Click += new System.EventHandler(this.saveParametersToolStripMenuItem1_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // textBox_serverIP
            // 
            this.textBox_serverIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_serverIP.Location = new System.Drawing.Point(14, 291);
            this.textBox_serverIP.Name = "textBox_serverIP";
            this.textBox_serverIP.Size = new System.Drawing.Size(100, 20);
            this.textBox_serverIP.TabIndex = 61;
            this.textBox_serverIP.Text = "109.73.43.4";
            // 
            // textBox_clientPort
            // 
            this.textBox_clientPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_clientPort.Location = new System.Drawing.Point(120, 330);
            this.textBox_clientPort.Name = "textBox_clientPort";
            this.textBox_clientPort.Size = new System.Drawing.Size(62, 20);
            this.textBox_clientPort.TabIndex = 61;
            this.textBox_clientPort.Text = "19086";
            this.textBox_clientPort.Leave += new System.EventHandler(this.textBox_clientPort_Leave);
            // 
            // textBox_serverPort
            // 
            this.textBox_serverPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_serverPort.Location = new System.Drawing.Point(120, 291);
            this.textBox_serverPort.Name = "textBox_serverPort";
            this.textBox_serverPort.Size = new System.Drawing.Size(62, 20);
            this.textBox_serverPort.TabIndex = 61;
            this.textBox_serverPort.Text = "19086";
            this.textBox_serverPort.Leave += new System.EventHandler(this.textBox_serverPort_Leave);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 275);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Server IP";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 314);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Client IP";
            // 
            // textBox_clientIP
            // 
            this.textBox_clientIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_clientIP.Location = new System.Drawing.Point(14, 330);
            this.textBox_clientIP.Name = "textBox_clientIP";
            this.textBox_clientIP.Size = new System.Drawing.Size(100, 20);
            this.textBox_clientIP.TabIndex = 61;
            this.textBox_clientIP.Text = "127.0.0.1";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 362);
            this.Controls.Add(this.textBox_clientPort);
            this.Controls.Add(this.textBox_serverPort);
            this.Controls.Add(this.textBox_clientIP);
            this.Controls.Add(this.textBox_serverIP);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox_serverName);
            this.Controls.Add(this.textBox_clientName);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.checkBox_Mark);
            this.Controls.Add(this.checkBox_ClientHex);
            this.Controls.Add(this.checkBox_ServerHex);
            this.Controls.Add(this.button_clear1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label_dispHex);
            this.Controls.Add(this.button_stop);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(720, 400);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TcpRelay";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Button button_stop;
        private System.Windows.Forms.Button button_clear1;
        public System.Windows.Forms.TextBox textBox_terminal1;
        private System.Windows.Forms.CheckBox checkBox_insTime;
        private System.Windows.Forms.CheckBox checkBox_ServerHex;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.CheckBox checkBox_insDir;
        private System.Windows.Forms.CheckBox checkBox_ClientHex;
        private System.Windows.Forms.Label label_dispHex;
        private System.Windows.Forms.CheckBox checkBox_Mark;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.DataGridView dataGridView;
        private TextBox textBox_clientName;
        private TextBox textBox_serverName;
        private Label label9;
        private Label label16;
        private Label label17;
        private Label label18;
        private GroupBox groupBox1;
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveTXTToolStripMenuItem;
        private ToolStripMenuItem saveCSVToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem logToGridToolStripMenuItem;
        private ToolStripMenuItem autoscrollToolStripMenuItem;
        private ToolStripMenuItem lineWrapToolStripMenuItem;
        private ToolStripMenuItem autosaveTXTToolStripMenuItem1;
        private ToolStripMenuItem autosaveCSVToolStripMenuItem1;
        private ToolStripMenuItem saveParametersToolStripMenuItem1;
        private ToolStripTextBox terminaltxtToolStripMenuItem1;
        private ToolStripTextBox terminalcsvToolStripMenuItem1;
        private ToolStripMenuItem LineBreakToolStripMenuItem1;
        private ToolStripTextBox LineBreakToolStripTextBox1;
        private ToolStripMenuItem logToTextToolStripMenuItem;
        private TextBox textBox_serverIP;
        private TextBox textBox_clientPort;
        private TextBox textBox_serverPort;
        private Label label1;
        private Timer timer1;
        private Label label2;
        private TextBox textBox_clientIP;
        private CheckBox checkBox_insPin;
    }
}

