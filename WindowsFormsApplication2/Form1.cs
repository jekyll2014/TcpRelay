using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace TcpRelay
{
    public partial class FormMain : Form
    {
        /*
        Codepages list https://msdn.microsoft.com/en-us/library/system.text.encoding(v=vs.110).aspx
        const int inputCodePage = TcpRelay.Properties.Settings.Default.CodePage;
        */
        IPAddress localAddr = IPAddress.Any;

        public static TcpClient ServerSocket = new TcpClient();
        public static NetworkStream ServerNetworkStream;
        public static TcpListener serverSocketListener;
        public static int serverPort = 0;

        public static TcpClient ClientSocket = new TcpClient();
        public static TcpListener clientSocketListener;
        public static NetworkStream ClientNetworkStream;
        public static int clientPort = 0;

        public DataTable CSVdataTable = new DataTable("Logs");
        string serverName, clientName;

        public const byte ServerDataIn = 11;
        public const byte ServerDataOut = 12;
        public const byte ServerSignal = 13;
        public const byte ServerError = 15;
        public const byte ClientDataIn = 21;
        public const byte ClientDataOut = 22;
        public const byte ClientSignal = 23;
        public const byte ClientError = 25;

        delegate void SetTextCallback1(string text);
        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            //if (this.textBox_terminal1.InvokeRequired)
            if (this.textBox_terminal1.InvokeRequired)
            {
                SetTextCallback1 d = new SetTextCallback1(SetText);
                this.BeginInvoke(d, new object[] { text });
            }
            else
            {
                this.textBox_terminal1.SelectionStart = this.textBox_terminal1.TextLength;
                this.textBox_terminal1.SelectedText = text;
            }
        }

        public class gridColumns
        {
            public static string Date { get; } = "Date";
            public static string Time { get; } = "Time";
            public static string Milis { get; } = "Milis";
            public static string Port { get; } = "Port";
            public static string Dir { get; } = "Dir";
            public static string Data { get; } = "Data";
            public static string Signal { get; } = "Signal";
            public static string Mark { get; } = "Mark";
        }

        private object threadLock = new object();
        public void collectBuffer(string tmpBuffer, int state, string time)
        {
            if (tmpBuffer != "")
            {
                lock (threadLock)
                {
                    //if (!(txtOutState == state && state != ServerDataOut && state != ClientDataOut))
                    //{
                    if (state == ServerDataIn)
                    {
                        if (checkBox_insDir.Checked == true) tmpBuffer = serverName + "<< " + tmpBuffer;
                    }
                    else if (state == ServerDataOut)
                    {
                        if (checkBox_insDir.Checked == true) tmpBuffer = serverName + ">> " + tmpBuffer;
                    }
                    else if (state == ServerSignal)
                    {
                        if (checkBox_insDir.Checked == true) tmpBuffer = serverName + "==" + tmpBuffer;
                    }
                    else if (state == ServerError)
                    {
                        if (checkBox_insDir.Checked == true) tmpBuffer = serverName + "!!" + tmpBuffer;
                    }
                    else if (state == ClientDataIn)
                    {
                        if (checkBox_insDir.Checked == true) tmpBuffer = clientName + "<< " + tmpBuffer;
                    }
                    else if (state == ClientDataOut)
                    {
                        if (checkBox_insDir.Checked == true) tmpBuffer = clientName + ">> " + tmpBuffer;
                    }
                    else if (state == ClientSignal)
                    {
                        if (checkBox_insDir.Checked == true) tmpBuffer = clientName + "==" + tmpBuffer;
                    }
                    else if (state == ClientError)
                    {
                        if (checkBox_insDir.Checked == true) tmpBuffer = clientName + "!!" + tmpBuffer;
                    }
                    if (checkBox_insTime.Checked == true) tmpBuffer = time + " " + tmpBuffer;
                    tmpBuffer = "\r\n" + tmpBuffer;
                    //txtOutState = state;
                    //}
                    if (autosaveTXTToolStripMenuItem1.Checked == true)
                    {
                        try
                        {
                            File.AppendAllText(terminaltxtToolStripMenuItem1.Text, tmpBuffer, Encoding.GetEncoding(TcpRelay.Properties.Settings.Default.CodePage));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("\r\nError opening file " + terminaltxtToolStripMenuItem1.Text + ": " + ex.Message);
                        }
                    }
                    if (logToTextToolStripMenuItem.Checked == true) SetText(tmpBuffer);
                }
            }
        }
        public void CSVcollectBuffer(string tmpBuffer)
        {
            lock (threadLock)
            {
                if (tmpBuffer != "")
                {
                    try
                    {
                        File.AppendAllText(terminalcsvToolStripMenuItem1.Text, tmpBuffer, Encoding.GetEncoding(TcpRelay.Properties.Settings.Default.CodePage));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("\r\nError opening file " + terminalcsvToolStripMenuItem1.Text + ": " + ex.Message);
                    }
                }
            }
        }
        public void CSVcollectGrid(DataRow tmpDataRow)
        {
            lock (threadLock)
            {
                CSVdataTable.Rows.Add(tmpDataRow);
            }
        }

        public FormMain()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView.DataSource = CSVdataTable;
            //create columns
            DataColumn colDate;
            colDate = new DataColumn(gridColumns.Date, typeof(string));
            DataColumn colTime;
            colTime = new DataColumn(gridColumns.Time, typeof(string));
            DataColumn colMilis;
            colMilis = new DataColumn(gridColumns.Milis, typeof(string));
            DataColumn colPort;
            colPort = new DataColumn(gridColumns.Port, typeof(string));
            DataColumn colDir;
            colDir = new DataColumn(gridColumns.Dir, typeof(string));
            DataColumn colData;
            colData = new DataColumn(gridColumns.Data, typeof(string));
            DataColumn colSig;
            colSig = new DataColumn(gridColumns.Signal, typeof(string));
            DataColumn colMark;
            colMark = new DataColumn(gridColumns.Mark, typeof(bool));
            //add columns to the table
            CSVdataTable.Columns.AddRange(new DataColumn[] { colDate, colTime, colMilis, colPort, colDir, colData, colSig, colMark });

            DataGridViewColumn column = dataGridView.Columns[gridColumns.Date];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.Resizable = DataGridViewTriState.True;
            column.MinimumWidth = 70;
            column.Width = 70;

            column = dataGridView.Columns[gridColumns.Time];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.Resizable = DataGridViewTriState.True;
            column.MinimumWidth = 55;
            column.Width = 55;

            column = dataGridView.Columns[gridColumns.Milis];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.Resizable = DataGridViewTriState.True;
            column.MinimumWidth = 30;
            column.Width = 30;

            column = dataGridView.Columns[gridColumns.Port];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.Resizable = DataGridViewTriState.True;
            column.MinimumWidth = 30;
            column.Width = 40;

            column = dataGridView.Columns[gridColumns.Dir];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.Resizable = DataGridViewTriState.True;
            column.MinimumWidth = 30;
            column.Width = 30;

            column = dataGridView.Columns[gridColumns.Data];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.Resizable = DataGridViewTriState.True;
            column.MinimumWidth = 200;
            column.Width = 250;

            column = dataGridView.Columns[gridColumns.Signal];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.Resizable = DataGridViewTriState.True;
            column.MinimumWidth = 60;
            column.Width = 60;

            column = dataGridView.Columns[gridColumns.Mark];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            column.Resizable = DataGridViewTriState.True;
            column.MinimumWidth = 30;
            column.Width = 30;

            //load settings
            checkBox_insTime.Checked = TcpRelay.Properties.Settings.Default.LogTime;
            checkBox_insDir.Checked = TcpRelay.Properties.Settings.Default.LogDir;
            checkBox_ServerHex.Checked = TcpRelay.Properties.Settings.Default.HexPort1;
            checkBox_ClientHex.Checked = TcpRelay.Properties.Settings.Default.HexPort2;
            textBox_clientName.Text = TcpRelay.Properties.Settings.Default.ClientName;
            textBox_serverName.Text = TcpRelay.Properties.Settings.Default.ServerName;
            logToGridToolStripMenuItem.Checked = TcpRelay.Properties.Settings.Default.LogGrid;
            autoscrollToolStripMenuItem.Checked = TcpRelay.Properties.Settings.Default.AutoScroll;
            lineWrapToolStripMenuItem.Checked = TcpRelay.Properties.Settings.Default.LineWrap;
            autosaveTXTToolStripMenuItem1.Checked = TcpRelay.Properties.Settings.Default.AutoLogTXT;
            terminaltxtToolStripMenuItem1.Text = TcpRelay.Properties.Settings.Default.TXTlogFile;
            autosaveCSVToolStripMenuItem1.Checked = TcpRelay.Properties.Settings.Default.AutoLogCSV;
            terminalcsvToolStripMenuItem1.Text = TcpRelay.Properties.Settings.Default.CSVlogFile;
            textBox_serverIP.Text = TcpRelay.Properties.Settings.Default.DefaultServerIP;
            textBox_clientIP.Text = TcpRelay.Properties.Settings.Default.DefaultClientIP;
            textBox_serverPort.Text = TcpRelay.Properties.Settings.Default.DefaultServerPort;
            textBox_clientPort.Text = TcpRelay.Properties.Settings.Default.DefaultClientPort;

            serverName = textBox_serverName.Text;
            checkBox_ServerHex.Text = textBox_serverName.Text;

            clientName = textBox_clientName.Text;
            checkBox_ClientHex.Text = textBox_clientName.Text;

            serverPort = int.Parse(textBox_serverPort.Text);
            clientPort = int.Parse(textBox_clientPort.Text);

            if (autosaveTXTToolStripMenuItem1.Checked == true) terminaltxtToolStripMenuItem1.Enabled = true;
            else terminaltxtToolStripMenuItem1.Enabled = false;
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            if (textBox_serverIP.Text != "" && textBox_serverPort.Text != "" && textBox_clientPort.Text != "")
            {
                try
                {
                    IPAddress localAddr = IPAddress.Any;
                    clientSocketListener = new TcpListener(localAddr, clientPort);
                    ClientSocket = new TcpClient();
                    clientSocketListener.Start();
                }
                catch (Exception ex)
                {
                    ClientErrorReceived("Open client port: " + ex.Message);
                    return;
                }
                //check if server available
                try
                {
                    ServerSocket = new TcpClient();
                    ServerSocket.Connect(textBox_serverIP.Text, serverPort);
                    ServerSocket.ReceiveTimeout = TcpRelay.Properties.Settings.Default.ReceiveTimeOut;
                    ServerSocket.SendTimeout = TcpRelay.Properties.Settings.Default.SendTimeOut;
                    ServerSocket.Client.ReceiveTimeout = TcpRelay.Properties.Settings.Default.ReceiveTimeOut;
                    ServerSocket.Client.SendTimeout = TcpRelay.Properties.Settings.Default.SendTimeOut;
                    ServerNetworkStream = ServerSocket.GetStream();
                }
                catch (Exception ex)
                {
                    ServerErrorReceived("Connect to server: " + ex.ToString());
                    return;
                }
                try
                {
                    if (ServerSocket.Connected)
                    {
                        ServerNetworkStream.Close();
                        ServerSocket.Close();
                        ServerSocket = new TcpClient();
                    }
                }
                catch (Exception ex)
                {
                    ServerErrorReceived("Disconnect server: " + ex.ToString());
                    return;
                }
                textBox_serverIP.Enabled = false;
                textBox_serverPort.Enabled = false;
                textBox_clientIP.Enabled = false;
                textBox_clientPort.Enabled = false;
                button_stop.Enabled = true;
                button_start.Enabled = false;
                ClientStatusChanged("Started listening to " + textBox_clientIP.Text + ":" + textBox_clientPort.Text);
                timer1.Enabled = true;
            }
            //button_send.Enabled = true;
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            try
            {
                clientSocketListener.Stop();
                if (ClientConnected())
                {
                    if (ClientSocket.Connected)
                    {
                        ClientNetworkStream.Close();
                        ClientSocket.Client.Disconnect(false);
                        ClientSocket.Close();
                        ClientSocket = new TcpClient();
                    }
                }
            }
            catch (Exception ex)
            {
                ClientErrorReceived("Close client port: " + ex.Message);
            }
            try
            {
                if (ServerConnected())
                {
                    if (ClientSocket.Connected)
                    {
                        ServerNetworkStream.Close();
                        ServerSocket.Client.Disconnect(false);
                        ServerSocket.Close();
                        ServerSocket = new TcpClient();
                    }
                }
            }
            catch (Exception ex)
            {
                ServerErrorReceived("Close server: " + ex.Message);
            }
            textBox_serverIP.Enabled = true;
            textBox_serverPort.Enabled = true;
            textBox_clientIP.Enabled = true;
            textBox_clientPort.Enabled = true;
            button_start.Enabled = true;
            button_stop.Enabled = false;
            ClientStatusChanged("Stopped listening to " + textBox_clientIP.Text + ":" + textBox_clientPort.Text);
        }

        private void client_DataReceived(byte[] rx1)
        {
            DataRow dataRowRX1 = null;
            dataRowRX1 = CSVdataTable.NewRow();
            dataRowRX1[gridColumns.Date] = DateTime.Today.ToShortDateString();
            dataRowRX1[gridColumns.Time] = DateTime.Now.ToLongTimeString();
            dataRowRX1[gridColumns.Milis] = DateTime.Now.Millisecond.ToString("D3");
            dataRowRX1[gridColumns.Port] = serverName;
            dataRowRX1[gridColumns.Dir] = "RX";
            dataRowRX1[gridColumns.Mark] = checkBox_Mark.Checked;
            dataRowRX1[gridColumns.Data] = Accessory.ConvertByteArrayToHex(rx1);
            if (logToGridToolStripMenuItem.Checked == true) CSVcollectGrid(dataRowRX1);
            string outStr1 = "";
            if (checkBox_ServerHex.Checked == true) outStr1 += dataRowRX1[gridColumns.Data];
            else outStr1 += Encoding.GetEncoding(TcpRelay.Properties.Settings.Default.CodePage).GetString(rx1, 0, rx1.Length);
            collectBuffer(outStr1, ServerDataIn, dataRowRX1[gridColumns.Date] + " " + dataRowRX1[gridColumns.Time] + "." + dataRowRX1[gridColumns.Milis]);
            if (autosaveCSVToolStripMenuItem1.Checked == true) CSVcollectBuffer(dataRowRX1[gridColumns.Date] + "," + dataRowRX1[gridColumns.Time] + "," + dataRowRX1[gridColumns.Milis] + "," + dataRowRX1[gridColumns.Port] + "," + dataRowRX1[gridColumns.Dir] + "," + dataRowRX1[gridColumns.Data] + "," + "," + dataRowRX1[gridColumns.Mark] + "\r\n");
        }

        private void server_DataReceived(byte[] rx2)
        {
            DataRow dataRowRX2 = null;
            dataRowRX2 = CSVdataTable.NewRow();
            dataRowRX2[gridColumns.Date] = DateTime.Today.ToShortDateString();
            dataRowRX2[gridColumns.Time] = DateTime.Now.ToLongTimeString();
            dataRowRX2[gridColumns.Milis] = DateTime.Now.Millisecond.ToString("D3");
            dataRowRX2[gridColumns.Port] = clientName;
            dataRowRX2[gridColumns.Dir] = "RX";
            dataRowRX2[gridColumns.Data] = Accessory.ConvertByteArrayToHex(rx2);
            dataRowRX2[gridColumns.Mark] = checkBox_Mark.Checked;
            if (logToGridToolStripMenuItem.Checked == true) CSVcollectGrid(dataRowRX2);
            string outStr2 = "";
            if (checkBox_ClientHex.Checked == true) outStr2 += dataRowRX2[gridColumns.Data];
            else outStr2 += System.Text.Encoding.GetEncoding(TcpRelay.Properties.Settings.Default.CodePage).GetString(rx2, 0, rx2.Length);
            collectBuffer(outStr2, ClientDataIn, dataRowRX2[gridColumns.Date] + " " + dataRowRX2[gridColumns.Time] + "." + dataRowRX2[gridColumns.Milis]);
            if (autosaveCSVToolStripMenuItem1.Checked == true) CSVcollectBuffer(dataRowRX2[gridColumns.Date] + "," + dataRowRX2[gridColumns.Time] + "," + dataRowRX2[gridColumns.Milis] + "," + dataRowRX2[gridColumns.Port] + "," + dataRowRX2[gridColumns.Dir] + "," + dataRowRX2[gridColumns.Data] + "," + "," + dataRowRX2[gridColumns.Mark] + "\r\n");
        }

        private void ServerStatusChanged(string outStr)
        {
            DataRow dataRowPIN1 = null;
            dataRowPIN1 = CSVdataTable.NewRow();
            dataRowPIN1[gridColumns.Date] = DateTime.Today.ToShortDateString();
            dataRowPIN1[gridColumns.Time] = DateTime.Now.ToLongTimeString();
            dataRowPIN1[gridColumns.Milis] = DateTime.Now.Millisecond.ToString("D3");
            dataRowPIN1[gridColumns.Port] = serverName;
            dataRowPIN1[gridColumns.Dir] = "-";
            dataRowPIN1[gridColumns.Mark] = checkBox_Mark.Checked;
            if (outStr != "")
            {
                if (checkBox_insPin.Checked == true) collectBuffer(outStr, ServerSignal, dataRowPIN1[gridColumns.Date] + " " + dataRowPIN1[gridColumns.Time] + "." + dataRowPIN1[gridColumns.Milis]);
                dataRowPIN1["Signal"] = outStr;
                if (logToGridToolStripMenuItem.Checked == true) CSVcollectGrid(dataRowPIN1);
                if (autosaveCSVToolStripMenuItem1.Checked == true) CSVcollectBuffer(dataRowPIN1[gridColumns.Date] + "," + dataRowPIN1[gridColumns.Time] + "," + dataRowPIN1[gridColumns.Milis] + "," + dataRowPIN1[gridColumns.Port] + "," + dataRowPIN1[gridColumns.Dir] + "," + dataRowPIN1[gridColumns.Data] + "," + dataRowPIN1["Signal"] + "," + dataRowPIN1[gridColumns.Mark] + "\r\n");
            }
        }

        private void ClientStatusChanged(string outStr)
        {
            DataRow dataRowPIN1 = null;
            dataRowPIN1 = CSVdataTable.NewRow();
            dataRowPIN1[gridColumns.Date] = DateTime.Today.ToShortDateString();
            dataRowPIN1[gridColumns.Time] = DateTime.Now.ToLongTimeString();
            dataRowPIN1[gridColumns.Milis] = DateTime.Now.Millisecond.ToString("D3");
            dataRowPIN1[gridColumns.Port] = clientName;
            dataRowPIN1[gridColumns.Dir] = "-";
            dataRowPIN1[gridColumns.Mark] = checkBox_Mark.Checked;
            if (outStr != "")
            {
                if (checkBox_insPin.Checked == true) collectBuffer(outStr, ClientSignal, dataRowPIN1[gridColumns.Date] + " " + dataRowPIN1[gridColumns.Time] + "." + dataRowPIN1[gridColumns.Milis]);
                dataRowPIN1["Signal"] = outStr;
                if (logToGridToolStripMenuItem.Checked == true) CSVcollectGrid(dataRowPIN1);
                if (autosaveCSVToolStripMenuItem1.Checked == true) CSVcollectBuffer(dataRowPIN1[gridColumns.Date] + "," + dataRowPIN1[gridColumns.Time] + "," + dataRowPIN1[gridColumns.Milis] + "," + dataRowPIN1[gridColumns.Port] + "," + dataRowPIN1[gridColumns.Dir] + "," + dataRowPIN1[gridColumns.Data] + "," + dataRowPIN1["Signal"] + "," + dataRowPIN1[gridColumns.Mark] + "\r\n");
            }
        }

        private void ServerErrorReceived(string outStr)
        {
            DataRow dataRowPIN1 = null;
            dataRowPIN1 = CSVdataTable.NewRow();
            dataRowPIN1[gridColumns.Date] = DateTime.Today.ToShortDateString();
            dataRowPIN1[gridColumns.Time] = DateTime.Now.ToLongTimeString();
            dataRowPIN1[gridColumns.Milis] = DateTime.Now.Millisecond.ToString("D3");
            dataRowPIN1[gridColumns.Port] = serverName;
            dataRowPIN1[gridColumns.Dir] = "-";
            dataRowPIN1[gridColumns.Mark] = checkBox_Mark.Checked;
            outStr = "<! Error: " + outStr + "!>";
            if (checkBox_insPin.Checked == true) collectBuffer(outStr, ServerError, dataRowPIN1[gridColumns.Date] + " " + dataRowPIN1[gridColumns.Time] + "." + dataRowPIN1[gridColumns.Milis]);
            dataRowPIN1["Signal"] = outStr;
            if (logToGridToolStripMenuItem.Checked == true) CSVcollectGrid(dataRowPIN1);
            if (autosaveCSVToolStripMenuItem1.Checked == true) CSVcollectBuffer(dataRowPIN1[gridColumns.Date] + "," + dataRowPIN1[gridColumns.Time] + "," + dataRowPIN1[gridColumns.Milis] + "," + dataRowPIN1[gridColumns.Port] + "," + dataRowPIN1[gridColumns.Dir] + "," + dataRowPIN1[gridColumns.Data] + "," + dataRowPIN1["Signal"] + "," + dataRowPIN1[gridColumns.Mark] + "\r\n");
        }

        private void ClientErrorReceived(string outStr)
        {
            DataRow dataRowPIN1 = null;
            dataRowPIN1 = CSVdataTable.NewRow();
            dataRowPIN1[gridColumns.Date] = DateTime.Today.ToShortDateString();
            dataRowPIN1[gridColumns.Time] = DateTime.Now.ToLongTimeString();
            dataRowPIN1[gridColumns.Milis] = DateTime.Now.Millisecond.ToString("D3");
            dataRowPIN1[gridColumns.Port] = clientName;
            dataRowPIN1[gridColumns.Dir] = "-";
            dataRowPIN1[gridColumns.Mark] = checkBox_Mark.Checked;
            outStr = "<! Error: " + outStr + "!>";
            if (checkBox_insPin.Checked == true) collectBuffer(outStr, ClientError, dataRowPIN1[gridColumns.Date] + " " + dataRowPIN1[gridColumns.Time] + "." + dataRowPIN1[gridColumns.Milis]);
            dataRowPIN1["Signal"] = outStr;
            if (logToGridToolStripMenuItem.Checked == true) CSVcollectGrid(dataRowPIN1);
            if (autosaveCSVToolStripMenuItem1.Checked == true) CSVcollectBuffer(dataRowPIN1[gridColumns.Date] + "," + dataRowPIN1[gridColumns.Time] + "," + dataRowPIN1[gridColumns.Milis] + "," + dataRowPIN1[gridColumns.Port] + "," + dataRowPIN1[gridColumns.Dir] + "," + dataRowPIN1[gridColumns.Data] + "," + dataRowPIN1["Signal"] + "," + dataRowPIN1[gridColumns.Mark] + "\r\n");
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (saveFileDialog.Title == "Save .TXT log as...")
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, textBox_terminal1.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing to file " + saveFileDialog.FileName + ": " + ex.Message);
                }
            }
            if (saveFileDialog.Title == "Save .CSV log as...")
            {
                int columnCount = dataGridView.ColumnCount;
                string output = "";
                for (int i = 0; i < columnCount; i++)
                {
                    output += dataGridView.Columns[i].Name.ToString() + ",";
                }
                output += "\r\n";
                for (int i = 1; (i - 1) < dataGridView.RowCount; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        output += dataGridView.Rows[i - 1].Cells[j].Value.ToString() + ",";
                    }
                    output += "\r\n";
                }
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, output, Encoding.GetEncoding(TcpRelay.Properties.Settings.Default.CodePage));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing to file " + saveFileDialog.FileName + ": " + ex.Message);
                }
            }
        }

        private void saveTXTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Title = "Save .TXT log as...";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.Filter = "Text files|*.txt|All files|*.*";
            saveFileDialog.FileName = "terminal_" + DateTime.Today.ToShortDateString().Replace("/", "_") + ".txt";
            saveFileDialog.ShowDialog();
        }

        private void saveCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Title = "Save .CSV log as...";
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.Filter = "CSV files|*.csv|All files|*.*";
            saveFileDialog.FileName = "terminal_" + DateTime.Today.ToShortDateString().Replace("/", "_") + ".csv";
            saveFileDialog.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TcpRelay\r\n(c) Kalugin Andrey\r\nContact: jekyll@mail.ru");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveParametersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TcpRelay.Properties.Settings.Default.LogTime = checkBox_insTime.Checked;
            TcpRelay.Properties.Settings.Default.LogDir = checkBox_insDir.Checked;
            TcpRelay.Properties.Settings.Default.HexPort1 = checkBox_ServerHex.Checked;
            TcpRelay.Properties.Settings.Default.HexPort2 = checkBox_ClientHex.Checked;
            TcpRelay.Properties.Settings.Default.ServerName = textBox_clientName.Text;
            TcpRelay.Properties.Settings.Default.ClientName = textBox_serverName.Text;
            TcpRelay.Properties.Settings.Default.LogText = logToTextToolStripMenuItem.Checked;

            TcpRelay.Properties.Settings.Default.AutoScroll = autoscrollToolStripMenuItem.Checked;
            TcpRelay.Properties.Settings.Default.LineWrap = lineWrapToolStripMenuItem.Checked;
            TcpRelay.Properties.Settings.Default.AutoLogTXT = autosaveTXTToolStripMenuItem1.Checked;
            TcpRelay.Properties.Settings.Default.TXTlogFile = terminaltxtToolStripMenuItem1.Text;
            TcpRelay.Properties.Settings.Default.AutoLogCSV = autosaveCSVToolStripMenuItem1.Checked;
            TcpRelay.Properties.Settings.Default.CSVlogFile = terminalcsvToolStripMenuItem1.Text;
            TcpRelay.Properties.Settings.Default.DefaultServerIP = textBox_serverIP.Text;
            TcpRelay.Properties.Settings.Default.DefaultClientIP = textBox_clientIP.Text;
            TcpRelay.Properties.Settings.Default.DefaultServerPort = textBox_serverPort.Text;
            TcpRelay.Properties.Settings.Default.DefaultClientPort = textBox_clientPort.Text;


            TcpRelay.Properties.Settings.Default.Save();
        }

        private void autosaveTXTToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (autosaveTXTToolStripMenuItem1.Checked == true)
            {
                autosaveTXTToolStripMenuItem1.Checked = false;
                terminaltxtToolStripMenuItem1.Enabled = true;
            }
            else
            {
                autosaveTXTToolStripMenuItem1.Checked = true;
                terminaltxtToolStripMenuItem1.Enabled = false;
            }
        }

        private void autosaveCSVToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (autosaveCSVToolStripMenuItem1.Checked == true)
            {
                autosaveCSVToolStripMenuItem1.Checked = false;
                terminalcsvToolStripMenuItem1.Enabled = true;
            }
            else
            {
                autosaveCSVToolStripMenuItem1.Checked = true;
                terminalcsvToolStripMenuItem1.Enabled = false;
            }
        }

        private void lineWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lineWrapToolStripMenuItem.Checked == true) lineWrapToolStripMenuItem.Checked = false;
            else lineWrapToolStripMenuItem.Checked = true;
            textBox_terminal1.WordWrap = lineWrapToolStripMenuItem.Checked;
        }

        private void autoscrollToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (autoscrollToolStripMenuItem.Checked == true) autoscrollToolStripMenuItem.Checked = false;
            else autoscrollToolStripMenuItem.Checked = true;
        }

        private void logToGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (logToGridToolStripMenuItem.Checked == true)
            {
                logToGridToolStripMenuItem.Checked = false;
                ((Control)this.tabPage1).Enabled = false;
                if (logToTextToolStripMenuItem.Checked == false)
                {
                    tabControl1.Enabled = false;
                    tabControl1.Visible = false;
                }
            }
            else
            {
                logToGridToolStripMenuItem.Checked = true;
                ((Control)this.tabPage1).Enabled = true;
                tabControl1.Enabled = true;
                tabControl1.Visible = true;
            }
        }

        private void checkBox_Mark_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Mark.Checked == true) checkBox_Mark.Font = new Font(checkBox_Mark.Font, FontStyle.Bold);
            else checkBox_Mark.Font = new Font(checkBox_Mark.Font, FontStyle.Regular);
        }

        private void textBox_serverPort_Leave(object sender, EventArgs e)
        {
            int.TryParse(textBox_serverPort.Text, out serverPort);
            textBox_serverPort.Text = serverPort.ToString();
        }

        private void textBox_clientPort_Leave(object sender, EventArgs e)
        {
            int.TryParse(textBox_clientPort.Text, out clientPort);
            textBox_clientPort.Text = clientPort.ToString();
        }

        private void logToTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (logToTextToolStripMenuItem.Checked == true)
            {
                logToTextToolStripMenuItem.Checked = false;
                ((Control)this.tabPage2).Enabled = false;
                if (logToGridToolStripMenuItem.Checked == false)
                {
                    tabControl1.Enabled = false;
                    tabControl1.Visible = false;
                }
            }
            else
            {
                logToTextToolStripMenuItem.Checked = true;
                ((Control)this.tabPage2).Enabled = true;
                tabControl1.Enabled = true;
                tabControl1.Visible = true;
            }
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_terminal1.Clear();
            CSVdataTable.Rows.Clear();
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tabPage1 && logToTextToolStripMenuItem.Checked == false)
                e.Cancel = true;
            if (e.TabPage == tabPage2 && logToGridToolStripMenuItem.Checked == false)
                e.Cancel = true;
        }

        public static string ConvertByteArrayToHex(byte[] byteArr, int Length)
        {
            if (Length > byteArr.Length) Length = byteArr.Length;
            string hexStr = "";
            int i = 0;
            for (i = 0; i < Length; i++)
            {
                hexStr += byteArr[i].ToString("X2") + " ";
            }
            return hexStr;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            List<byte> bytesFromClient = new List<byte>();
            List<byte> bytesFromServer = new List<byte>();

            //accept client if any pending
            if (clientSocketListener.Pending())
            {
                try
                {
                    ClientSocket = clientSocketListener.AcceptTcpClient();
                    IPAddress clientAddr = ((IPEndPoint)ClientSocket.Client.RemoteEndPoint).Address;
                    if (textBox_clientIP.Text != "")
                    {
                        IPAddress setClientAddr;
                        IPAddress.TryParse(textBox_clientIP.Text, out setClientAddr);
                        if (clientAddr != setClientAddr)
                        {
                            ClientSocket.Client.Disconnect(false);
                            ClientSocket.Close();
                            ClientSocket = new TcpClient();
                            return;
                        }
                    }
                    ClientNetworkStream = ClientSocket.GetStream();
                    ClientStatusChanged("Client connected " + clientAddr.ToString() + ":" + textBox_clientPort.Text);
                }
                catch (Exception ex)
                {
                    ClientErrorReceived("Accept client: " + ex.ToString());
                }
                //connect to server
                try
                {
                    ServerSocket = new TcpClient();
                    ServerSocket.Connect(textBox_serverIP.Text, serverPort);
                    ServerSocket.ReceiveTimeout = TcpRelay.Properties.Settings.Default.ReceiveTimeOut;
                    ServerSocket.SendTimeout = TcpRelay.Properties.Settings.Default.SendTimeOut;
                    ServerSocket.Client.ReceiveTimeout = TcpRelay.Properties.Settings.Default.ReceiveTimeOut;
                    ServerSocket.Client.SendTimeout = TcpRelay.Properties.Settings.Default.SendTimeOut;
                    ServerNetworkStream = ServerSocket.GetStream();
                    timer1.Enabled = true;
                    ServerStatusChanged("Connected to server " + textBox_serverIP.Text + ":" + textBox_serverPort.Text);
                }
                catch (Exception ex)
                {
                    ServerErrorReceived("Connect to server: " + ex.ToString());
                }
            }

            //read data from client if any
            if (ClientConnected() && ClientSocket.Connected)
            {
                try
                {
                    while (ClientSocket.Available > 0)
                    {
                        bytesFromClient.Add((byte)ClientNetworkStream.ReadByte());
                    }
                    client_DataReceived(bytesFromClient.ToArray());
                }
                catch (Exception ex)
                {
                    ClientErrorReceived("Read client: " + ex.ToString());
                }
            }

            //disconnect from server if client disconnected
            if (!ClientConnected() && ServerConnected())
            {
                try
                {
                    if (ClientSocket.Connected)
                    {
                        ClientNetworkStream.Close();
                        ClientSocket.Close();
                        ClientSocket = new TcpClient();
                        ClientStatusChanged("Client disconnected");
                    }
                }
                catch (Exception ex)
                {
                    ClientErrorReceived("Disconnect client: " + ex.ToString());
                }
                try
                {
                    if (ServerSocket.Connected)
                    {
                        ServerNetworkStream.Close();
                        ServerSocket.Close();
                        ServerSocket = new TcpClient();
                        ServerStatusChanged("Server disconnected");
                    }
                }
                catch (Exception ex)
                {
                    ServerErrorReceived("Disconnect server: " + ex.ToString());
                }
            }

            //reconnect to server if disconnected
            if (ClientConnected() && !ServerConnected())
            {
                try
                {
                    ServerSocket = new TcpClient();
                    ServerSocket.Connect(textBox_serverIP.Text, serverPort);
                    ServerSocket.ReceiveTimeout = TcpRelay.Properties.Settings.Default.ReceiveTimeOut;
                    ServerSocket.SendTimeout = TcpRelay.Properties.Settings.Default.SendTimeOut;
                    ServerSocket.Client.ReceiveTimeout = TcpRelay.Properties.Settings.Default.ReceiveTimeOut;
                    ServerSocket.Client.SendTimeout = TcpRelay.Properties.Settings.Default.SendTimeOut;
                    ServerNetworkStream = ServerSocket.GetStream();
                    timer1.Enabled = true;
                    ServerStatusChanged("Reconnected to server " + textBox_serverIP.Text + ":" + textBox_serverPort.Text);
                }
                catch (Exception ex)
                {
                    ServerErrorReceived("Reconnect server: " + ex.ToString());
                }
            }

            //send from client to server
            if (bytesFromClient.Count > 0)
            {
                try
                {
                    ServerNetworkStream.Write(bytesFromClient.ToArray(), 0, bytesFromClient.Count);
                }
                catch (Exception ex)
                {
                    ServerErrorReceived("Send to server: " + ex.ToString());
                }
            }

            //read data from server if any
            if (ServerConnected() && ServerSocket.Connected)
            {
                try
                {
                    while (ServerSocket.Available > 0)
                    {
                        bytesFromServer.Add((byte)ServerNetworkStream.ReadByte());
                    }
                    server_DataReceived(bytesFromServer.ToArray());
                }
                catch (Exception ex)
                {
                    ServerErrorReceived("Read server: " + ex.ToString());
                }
            }

            //send from server to client
            if (bytesFromServer.Count > 0)
            {
                try
                {
                    ClientNetworkStream.Write(bytesFromServer.ToArray(), 0, bytesFromServer.Count);
                }
                catch (Exception ex)
                {
                    ClientErrorReceived("Send to client: " + ex.ToString());
                }
            }
        }

        private void textBox_serverName_Leave(object sender, EventArgs e)
        {
            serverName = textBox_serverName.Text;
            checkBox_ServerHex.Text = textBox_serverName.Text;
        }

        private void textBox_clientName_Leave(object sender, EventArgs e)
        {
            clientName = textBox_clientName.Text;
            checkBox_ClientHex.Text = textBox_clientName.Text;
        }

        public bool ClientConnected()
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnections = ipProperties.GetActiveTcpConnections();
            foreach (TcpConnectionInformation c in tcpConnections)
            {
                TcpState stateOfConnection = c.State;
                try
                {
                    if (c.LocalEndPoint.Equals(ClientSocket.Client.LocalEndPoint) && c.RemoteEndPoint.Equals(ClientSocket.Client.RemoteEndPoint))
                    {
                        if (stateOfConnection == TcpState.Established)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ClientErrorReceived("Socket closed: " + ex.ToString());
                    return false;
                }
            }
            return false;
        }

        public bool ServerConnected()
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnections = ipProperties.GetActiveTcpConnections();
            foreach (TcpConnectionInformation c in tcpConnections)
            {
                TcpState stateOfConnection = c.State;
                try
                {
                    if (c.LocalEndPoint.Equals(ServerSocket.Client.LocalEndPoint) && c.RemoteEndPoint.Equals(ServerSocket.Client.RemoteEndPoint))
                    {
                        if (stateOfConnection == TcpState.Established)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ServerErrorReceived("Socket closed: " + ex.ToString());
                    return false;
                }
            }
            return false;
        }

    }
}
