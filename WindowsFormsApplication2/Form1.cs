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

using TcpRelay.Properties;

namespace TcpRelay
{
    public partial class FormMain : Form
    {
        /*
        Codepages list https://msdn.microsoft.com/en-us/library/system.text.encoding(v=vs.110).aspx
        const int inputCodePage = TcpRelay.Properties.Settings.Default.CodePage;
        */
        private static TcpClient ServerSocket = new TcpClient();
        private static NetworkStream ServerNetworkStream;
        private static int ServerPort;

        private static TcpClient ClientSocket = new TcpClient();
        private static TcpListener ClientSocketListener;
        private static NetworkStream ClientNetworkStream;
        private static int ClientPort;

        private readonly DataTable CSVdataTable = new DataTable("Logs");
        private string _serverName, _clientName;

        private const byte ServerDataIn = 11;
        private const byte ServerDataOut = 12;
        private const byte ServerStatus = 13;
        private const byte ServerError = 15;

        private const byte ClientDataIn = 21;
        private const byte ClientDataOut = 22;
        private const byte ClientStatus = 23;
        private const byte ClientError = 25;

        private delegate void SetTextCallback1(string text);

        private void SetText(string text)
        {
            text = Accessory.FilterZeroChar(text);
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            //if (this.textBox_terminal1.InvokeRequired)
            if (textBox_terminal1.InvokeRequired)
            {
                var d = new SetTextCallback1(SetText);
                BeginInvoke(d, text);
            }
            else
            {
                textBox_terminal1.SelectionStart = textBox_terminal1.TextLength;
                textBox_terminal1.SelectedText = text;
            }
        }

        private class GridColumns
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

        private readonly object threadLock = new object();

        private void CollectBuffer(string tmpBuffer, int state, string time)
        {
            if (tmpBuffer != "")
                lock (threadLock)
                {
                    //if (!(txtOutState == state && state != ServerDataOut && state != ClientDataOut))
                    //{
                    if (state == ServerDataIn)
                    {
                        if (checkBox_insDir.Checked) tmpBuffer = _serverName + "<< " + tmpBuffer;
                    }
                    else if (state == ServerDataOut)
                    {
                        if (checkBox_insDir.Checked) tmpBuffer = _serverName + ">> " + tmpBuffer;
                    }
                    else if (state == ServerStatus)
                    {
                        if (checkBox_insDir.Checked) tmpBuffer = _serverName + "==" + tmpBuffer;
                    }
                    else if (state == ServerError)
                    {
                        if (checkBox_insDir.Checked) tmpBuffer = _serverName + "!!" + tmpBuffer;
                    }
                    else if (state == ClientDataIn)
                    {
                        if (checkBox_insDir.Checked) tmpBuffer = _clientName + "<< " + tmpBuffer;
                    }
                    else if (state == ClientDataOut)
                    {
                        if (checkBox_insDir.Checked) tmpBuffer = _clientName + ">> " + tmpBuffer;
                    }
                    else if (state == ClientStatus)
                    {
                        if (checkBox_insDir.Checked) tmpBuffer = _clientName + "==" + tmpBuffer;
                    }
                    else if (state == ClientError)
                    {
                        if (checkBox_insDir.Checked) tmpBuffer = _clientName + "!!" + tmpBuffer;
                    }

                    if (checkBox_insTime.Checked) tmpBuffer = time + " " + tmpBuffer;
                    tmpBuffer = "\r\n" + tmpBuffer;
                    //txtOutState = state;
                    //}
                    if (autosaveTXTToolStripMenuItem1.Checked)
                        try
                        {
                            File.AppendAllText(terminaltxtToolStripMenuItem1.Text, tmpBuffer,
                                Encoding.GetEncoding(Settings.Default.CodePage));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("\r\nError opening file " + terminaltxtToolStripMenuItem1.Text + ": " +
                                            ex.Message);
                        }

                    if (logToTextToolStripMenuItem.Checked) SetText(tmpBuffer);
                }
        }

        private void CSVcollectBuffer(string tmpBuffer)
        {
            lock (threadLock)
            {
                if (tmpBuffer != "")
                    try
                    {
                        File.AppendAllText(terminalcsvToolStripMenuItem1.Text, tmpBuffer,
                            Encoding.GetEncoding(Settings.Default.CodePage));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("\r\nError opening file " + terminalcsvToolStripMenuItem1.Text + ": " +
                                        ex.Message);
                    }
            }
        }

        private void CSVcollectGrid(DataRow tmpDataRow)
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
            colDate = new DataColumn(GridColumns.Date, typeof(string));
            DataColumn colTime;
            colTime = new DataColumn(GridColumns.Time, typeof(string));
            DataColumn colMilis;
            colMilis = new DataColumn(GridColumns.Milis, typeof(string));
            DataColumn colPort;
            colPort = new DataColumn(GridColumns.Port, typeof(string));
            DataColumn colDir;
            colDir = new DataColumn(GridColumns.Dir, typeof(string));
            DataColumn colData;
            colData = new DataColumn(GridColumns.Data, typeof(string));
            DataColumn colSig;
            colSig = new DataColumn(GridColumns.Signal, typeof(string));
            DataColumn colMark;
            colMark = new DataColumn(GridColumns.Mark, typeof(bool));
            //add columns to the table
            CSVdataTable.Columns.AddRange(new[]
                {colDate, colTime, colMilis, colPort, colDir, colData, colSig, colMark});

            var column = dataGridView.Columns[GridColumns.Date];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.Resizable = DataGridViewTriState.True;
            column.MinimumWidth = 70;
            column.Width = 70;

            column = dataGridView.Columns[GridColumns.Time];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.Resizable = DataGridViewTriState.True;
            column.MinimumWidth = 55;
            column.Width = 55;

            column = dataGridView.Columns[GridColumns.Milis];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.Resizable = DataGridViewTriState.True;
            column.MinimumWidth = 30;
            column.Width = 30;

            column = dataGridView.Columns[GridColumns.Port];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.Resizable = DataGridViewTriState.True;
            column.MinimumWidth = 30;
            column.Width = 40;

            column = dataGridView.Columns[GridColumns.Dir];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.Resizable = DataGridViewTriState.True;
            column.MinimumWidth = 30;
            column.Width = 30;

            column = dataGridView.Columns[GridColumns.Data];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.Resizable = DataGridViewTriState.True;
            column.MinimumWidth = 200;
            column.Width = 250;

            column = dataGridView.Columns[GridColumns.Signal];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.Resizable = DataGridViewTriState.True;
            column.MinimumWidth = 60;
            column.Width = 60;

            column = dataGridView.Columns[GridColumns.Mark];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            column.Resizable = DataGridViewTriState.True;
            column.MinimumWidth = 30;
            column.Width = 30;

            //load settings
            checkBox_insTime.Checked = Settings.Default.LogTime;
            checkBox_insDir.Checked = Settings.Default.LogDir;
            checkBox_ServerHex.Checked = Settings.Default.HexPort1;
            checkBox_ClientHex.Checked = Settings.Default.HexPort2;
            textBox_clientName.Text = Settings.Default.ClientName;
            textBox_serverName.Text = Settings.Default.ServerName;
            logToGridToolStripMenuItem.Checked = Settings.Default.LogGrid;
            autoscrollToolStripMenuItem.Checked = Settings.Default.AutoScroll;
            lineWrapToolStripMenuItem.Checked = Settings.Default.LineWrap;
            autosaveTXTToolStripMenuItem1.Checked = Settings.Default.AutoLogTXT;
            terminaltxtToolStripMenuItem1.Text = Settings.Default.TXTlogFile;
            autosaveCSVToolStripMenuItem1.Checked = Settings.Default.AutoLogCSV;
            terminalcsvToolStripMenuItem1.Text = Settings.Default.CSVlogFile;
            textBox_serverIP.Text = Settings.Default.DefaultServerIP;
            textBox_clientIP.Text = Settings.Default.DefaultClientIP;
            textBox_serverPort.Text = Settings.Default.DefaultServerPort;
            textBox_clientPort.Text = Settings.Default.DefaultClientPort;

            _serverName = textBox_serverName.Text;
            checkBox_ServerHex.Text = textBox_serverName.Text;

            _clientName = textBox_clientName.Text;
            checkBox_ClientHex.Text = textBox_clientName.Text;

            ServerPort = int.Parse(textBox_serverPort.Text);
            ClientPort = int.Parse(textBox_clientPort.Text);

            if (autosaveTXTToolStripMenuItem1.Checked) terminaltxtToolStripMenuItem1.Enabled = true;
            else terminaltxtToolStripMenuItem1.Enabled = false;
        }

        private void Button_start_Click(object sender, EventArgs e)
        {
            if (textBox_serverIP.Text != "" && textBox_serverPort.Text != "" && textBox_clientPort.Text != "")
            {
                try
                {
                    var localAddr = IPAddress.Any;
                    ClientSocketListener = new TcpListener(localAddr, ClientPort);
                    ClientSocket = new TcpClient();
                    ClientSocketListener.Start();
                }
                catch (Exception ex)
                {
                    ClientErrorReceived("Error opening client port: " + ex.Message);
                    return;
                }

                //check if server available
                try
                {
                    ServerSocket = new TcpClient();
                    ServerSocket.Connect(textBox_serverIP.Text, ServerPort);
                    ServerSocket.ReceiveTimeout = Settings.Default.ReceiveTimeOut;
                    ServerSocket.SendTimeout = Settings.Default.SendTimeOut;
                    ServerSocket.Client.ReceiveTimeout = Settings.Default.ReceiveTimeOut;
                    ServerSocket.Client.SendTimeout = Settings.Default.SendTimeOut;
                    ServerNetworkStream = ServerSocket.GetStream();
                }
                catch (Exception ex)
                {
                    ServerErrorReceived("Error connecting to server: " + ex);
                    ServerNetworkStream?.Close();
                    ServerSocket?.Close();
                    ServerSocket = new TcpClient();
                    ClientSocketListener.Stop();
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
                    else
                    {
                        ServerErrorReceived("Error connecting to server");
                        ServerNetworkStream?.Close();
                        ServerSocket?.Close();
                        ServerSocket = new TcpClient();
                        ClientSocketListener.Stop();
                    }
                }
                catch (Exception ex)
                {
                    ServerErrorReceived("Disconnect server: " + ex);
                    ServerErrorReceived("Error connecting to server: " + ex);
                    ServerNetworkStream?.Close();
                    ServerSocket?.Close();
                    ServerSocket = new TcpClient();
                    ClientSocketListener.Stop();
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
        }

        private void Button_stop_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            try
            {
                ClientSocketListener.Stop();
                if (ClientConnected())
                    if (ClientSocket.Connected)
                    {
                        ClientNetworkStream.Close();
                        ClientSocket.Client.Disconnect(false);
                        ClientSocket.Close();
                        ClientSocket = new TcpClient();
                    }
            }
            catch (Exception ex)
            {
                ClientErrorReceived("Close client port: " + ex.Message);
            }

            try
            {
                if (ServerConnected())
                    if (ServerSocket.Connected)
                    {
                        ServerNetworkStream.Close();
                        ServerSocket.Client.Disconnect(false);
                        ServerSocket.Close();
                        ServerSocket = new TcpClient();
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

        private void Client_DataReceived(byte[] rx1)
        {
            var dataRowRx1 = CSVdataTable.NewRow();
            dataRowRx1[GridColumns.Date] = DateTime.Today.ToShortDateString();
            dataRowRx1[GridColumns.Time] = DateTime.Now.ToLongTimeString();
            dataRowRx1[GridColumns.Milis] = DateTime.Now.Millisecond.ToString("D3");
            dataRowRx1[GridColumns.Port] = _serverName;
            dataRowRx1[GridColumns.Dir] = "RX";
            dataRowRx1[GridColumns.Mark] = checkBox_Mark.Checked;
            dataRowRx1[GridColumns.Data] = Accessory.ConvertByteArrayToHex(rx1);
            if (logToGridToolStripMenuItem.Checked) CSVcollectGrid(dataRowRx1);
            var outStr1 = "";
            if (checkBox_ServerHex.Checked) outStr1 += dataRowRx1[GridColumns.Data];
            else outStr1 += Encoding.GetEncoding(Settings.Default.CodePage).GetString(rx1, 0, rx1.Length);
            CollectBuffer(outStr1, ServerDataIn,
                dataRowRx1[GridColumns.Date] + " " + dataRowRx1[GridColumns.Time] + "." +
                dataRowRx1[GridColumns.Milis]);
            if (autosaveCSVToolStripMenuItem1.Checked)
                CSVcollectBuffer(dataRowRx1[GridColumns.Date] + "," + dataRowRx1[GridColumns.Time] + "," +
                                 dataRowRx1[GridColumns.Milis] + "," + dataRowRx1[GridColumns.Port] + "," +
                                 dataRowRx1[GridColumns.Dir] + "," + dataRowRx1[GridColumns.Data] + "," + "," +
                                 dataRowRx1[GridColumns.Mark] + "\r\n");
        }

        private void Server_DataReceived(byte[] rx2)
        {
            var dataRowRx2 = CSVdataTable.NewRow();
            dataRowRx2[GridColumns.Date] = DateTime.Today.ToShortDateString();
            dataRowRx2[GridColumns.Time] = DateTime.Now.ToLongTimeString();
            dataRowRx2[GridColumns.Milis] = DateTime.Now.Millisecond.ToString("D3");
            dataRowRx2[GridColumns.Port] = _clientName;
            dataRowRx2[GridColumns.Dir] = "RX";
            dataRowRx2[GridColumns.Data] = Accessory.ConvertByteArrayToHex(rx2);
            dataRowRx2[GridColumns.Mark] = checkBox_Mark.Checked;
            if (logToGridToolStripMenuItem.Checked) CSVcollectGrid(dataRowRx2);
            var outStr2 = "";
            if (checkBox_ClientHex.Checked) outStr2 += dataRowRx2[GridColumns.Data];
            else outStr2 += Encoding.GetEncoding(Settings.Default.CodePage).GetString(rx2, 0, rx2.Length);
            CollectBuffer(outStr2, ClientDataIn,
                dataRowRx2[GridColumns.Date] + " " + dataRowRx2[GridColumns.Time] + "." +
                dataRowRx2[GridColumns.Milis]);
            if (autosaveCSVToolStripMenuItem1.Checked)
                CSVcollectBuffer(dataRowRx2[GridColumns.Date] + "," + dataRowRx2[GridColumns.Time] + "," +
                                 dataRowRx2[GridColumns.Milis] + "," + dataRowRx2[GridColumns.Port] + "," +
                                 dataRowRx2[GridColumns.Dir] + "," + dataRowRx2[GridColumns.Data] + "," + "," +
                                 dataRowRx2[GridColumns.Mark] + "\r\n");
        }

        private void ServerStatusChanged(string outStr)
        {
            var dataRowPin1 = CSVdataTable.NewRow();
            dataRowPin1[GridColumns.Date] = DateTime.Today.ToShortDateString();
            dataRowPin1[GridColumns.Time] = DateTime.Now.ToLongTimeString();
            dataRowPin1[GridColumns.Milis] = DateTime.Now.Millisecond.ToString("D3");
            dataRowPin1[GridColumns.Port] = _serverName;
            dataRowPin1[GridColumns.Dir] = "-";
            dataRowPin1[GridColumns.Mark] = checkBox_Mark.Checked;
            if (outStr != "")
            {
                if (checkBox_insPin.Checked)
                    CollectBuffer(outStr, ServerStatus,
                        dataRowPin1[GridColumns.Date] + " " + dataRowPin1[GridColumns.Time] + "." +
                        dataRowPin1[GridColumns.Milis]);
                dataRowPin1["Signal"] = outStr;
                if (logToGridToolStripMenuItem.Checked) CSVcollectGrid(dataRowPin1);
                if (autosaveCSVToolStripMenuItem1.Checked)
                    CSVcollectBuffer(dataRowPin1[GridColumns.Date] + "," + dataRowPin1[GridColumns.Time] + "," +
                                     dataRowPin1[GridColumns.Milis] + "," + dataRowPin1[GridColumns.Port] + "," +
                                     dataRowPin1[GridColumns.Dir] + "," + dataRowPin1[GridColumns.Data] + "," +
                                     dataRowPin1["Signal"] + "," + dataRowPin1[GridColumns.Mark] + "\r\n");
            }
        }

        private void ClientStatusChanged(string outStr)
        {
            var dataRowPin1 = CSVdataTable.NewRow();
            dataRowPin1[GridColumns.Date] = DateTime.Today.ToShortDateString();
            dataRowPin1[GridColumns.Time] = DateTime.Now.ToLongTimeString();
            dataRowPin1[GridColumns.Milis] = DateTime.Now.Millisecond.ToString("D3");
            dataRowPin1[GridColumns.Port] = _clientName;
            dataRowPin1[GridColumns.Dir] = "-";
            dataRowPin1[GridColumns.Mark] = checkBox_Mark.Checked;
            if (outStr != "")
            {
                if (checkBox_insPin.Checked)
                    CollectBuffer(outStr, ClientStatus,
                        dataRowPin1[GridColumns.Date] + " " + dataRowPin1[GridColumns.Time] + "." +
                        dataRowPin1[GridColumns.Milis]);
                dataRowPin1["Signal"] = outStr;
                if (logToGridToolStripMenuItem.Checked) CSVcollectGrid(dataRowPin1);
                if (autosaveCSVToolStripMenuItem1.Checked)
                    CSVcollectBuffer(dataRowPin1[GridColumns.Date] + "," + dataRowPin1[GridColumns.Time] + "," +
                                     dataRowPin1[GridColumns.Milis] + "," + dataRowPin1[GridColumns.Port] + "," +
                                     dataRowPin1[GridColumns.Dir] + "," + dataRowPin1[GridColumns.Data] + "," +
                                     dataRowPin1["Signal"] + "," + dataRowPin1[GridColumns.Mark] + "\r\n");
            }
        }

        private void ServerErrorReceived(string outStr)
        {
            var dataRowPin1 = CSVdataTable.NewRow();
            dataRowPin1[GridColumns.Date] = DateTime.Today.ToShortDateString();
            dataRowPin1[GridColumns.Time] = DateTime.Now.ToLongTimeString();
            dataRowPin1[GridColumns.Milis] = DateTime.Now.Millisecond.ToString("D3");
            dataRowPin1[GridColumns.Port] = _serverName;
            dataRowPin1[GridColumns.Dir] = "-";
            dataRowPin1[GridColumns.Mark] = checkBox_Mark.Checked;
            outStr = "<! Error: " + outStr + "!>";
            if (checkBox_insPin.Checked)
                CollectBuffer(outStr, ServerError,
                    dataRowPin1[GridColumns.Date] + " " + dataRowPin1[GridColumns.Time] + "." +
                    dataRowPin1[GridColumns.Milis]);
            dataRowPin1["Signal"] = outStr;
            if (logToGridToolStripMenuItem.Checked) CSVcollectGrid(dataRowPin1);
            if (autosaveCSVToolStripMenuItem1.Checked)
                CSVcollectBuffer(dataRowPin1[GridColumns.Date] + "," + dataRowPin1[GridColumns.Time] + "," +
                                 dataRowPin1[GridColumns.Milis] + "," + dataRowPin1[GridColumns.Port] + "," +
                                 dataRowPin1[GridColumns.Dir] + "," + dataRowPin1[GridColumns.Data] + "," +
                                 dataRowPin1["Signal"] + "," + dataRowPin1[GridColumns.Mark] + "\r\n");
        }

        private void ClientErrorReceived(string outStr)
        {
            var dataRowPin1 = CSVdataTable.NewRow();
            dataRowPin1[GridColumns.Date] = DateTime.Today.ToShortDateString();
            dataRowPin1[GridColumns.Time] = DateTime.Now.ToLongTimeString();
            dataRowPin1[GridColumns.Milis] = DateTime.Now.Millisecond.ToString("D3");
            dataRowPin1[GridColumns.Port] = _clientName;
            dataRowPin1[GridColumns.Dir] = "-";
            dataRowPin1[GridColumns.Mark] = checkBox_Mark.Checked;
            outStr = "<! Error: " + outStr + "!>";
            if (checkBox_insPin.Checked)
                CollectBuffer(outStr, ClientError,
                    dataRowPin1[GridColumns.Date] + " " + dataRowPin1[GridColumns.Time] + "." +
                    dataRowPin1[GridColumns.Milis]);
            dataRowPin1["Signal"] = outStr;
            if (logToGridToolStripMenuItem.Checked) CSVcollectGrid(dataRowPin1);
            if (autosaveCSVToolStripMenuItem1.Checked)
                CSVcollectBuffer(dataRowPin1[GridColumns.Date] + "," + dataRowPin1[GridColumns.Time] + "," +
                                 dataRowPin1[GridColumns.Milis] + "," + dataRowPin1[GridColumns.Port] + "," +
                                 dataRowPin1[GridColumns.Dir] + "," + dataRowPin1[GridColumns.Data] + "," +
                                 dataRowPin1["Signal"] + "," + dataRowPin1[GridColumns.Mark] + "\r\n");
        }

        private void SaveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (saveFileDialog.Title == "Save .TXT log as...")
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, textBox_terminal1.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing to file " + saveFileDialog.FileName + ": " + ex.Message);
                }

            if (saveFileDialog.Title == "Save .CSV log as...")
            {
                var columnCount = dataGridView.ColumnCount;
                var output = "";
                for (var i = 0; i < columnCount; i++) output += dataGridView.Columns[i].Name + ",";
                output += "\r\n";
                for (var i = 1; i - 1 < dataGridView.RowCount; i++)
                {
                    for (var j = 0; j < columnCount; j++) output += dataGridView.Rows[i - 1].Cells[j].Value + ",";
                    output += "\r\n";
                }

                try
                {
                    File.WriteAllText(saveFileDialog.FileName, output, Encoding.GetEncoding(Settings.Default.CodePage));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing to file " + saveFileDialog.FileName + ": " + ex.Message);
                }
            }
        }

        private void SaveTXTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Title = "Save .TXT log as...";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.Filter = "Text files|*.txt|All files|*.*";
            saveFileDialog.FileName = "terminal_" + DateTime.Today.ToShortDateString().Replace("/", "_") + ".txt";
            saveFileDialog.ShowDialog();
        }

        private void SaveCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Title = "Save .CSV log as...";
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.Filter = "CSV files|*.csv|All files|*.*";
            saveFileDialog.FileName = "terminal_" + DateTime.Today.ToShortDateString().Replace("/", "_") + ".csv";
            saveFileDialog.ShowDialog();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TcpRelay\r\n(c) Kalugin Andrey\r\nContact: jekyll@mail.ru");
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SaveParametersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Settings.Default.LogTime = checkBox_insTime.Checked;
            Settings.Default.LogDir = checkBox_insDir.Checked;
            Settings.Default.HexPort1 = checkBox_ServerHex.Checked;
            Settings.Default.HexPort2 = checkBox_ClientHex.Checked;
            Settings.Default.ServerName = textBox_clientName.Text;
            Settings.Default.ClientName = textBox_serverName.Text;
            Settings.Default.LogText = logToTextToolStripMenuItem.Checked;

            Settings.Default.AutoScroll = autoscrollToolStripMenuItem.Checked;
            Settings.Default.LineWrap = lineWrapToolStripMenuItem.Checked;
            Settings.Default.AutoLogTXT = autosaveTXTToolStripMenuItem1.Checked;
            Settings.Default.TXTlogFile = terminaltxtToolStripMenuItem1.Text;
            Settings.Default.AutoLogCSV = autosaveCSVToolStripMenuItem1.Checked;
            Settings.Default.CSVlogFile = terminalcsvToolStripMenuItem1.Text;
            Settings.Default.DefaultServerIP = textBox_serverIP.Text;
            Settings.Default.DefaultClientIP = textBox_clientIP.Text;
            Settings.Default.DefaultServerPort = textBox_serverPort.Text;
            Settings.Default.DefaultClientPort = textBox_clientPort.Text;


            Settings.Default.Save();
        }

        private void AutosaveTXTToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (autosaveTXTToolStripMenuItem1.Checked)
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

        private void AutosaveCSVToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (autosaveCSVToolStripMenuItem1.Checked)
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

        private void LineWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lineWrapToolStripMenuItem.Checked) lineWrapToolStripMenuItem.Checked = false;
            else lineWrapToolStripMenuItem.Checked = true;
            textBox_terminal1.WordWrap = lineWrapToolStripMenuItem.Checked;
        }

        private void AutoscrollToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (autoscrollToolStripMenuItem.Checked) autoscrollToolStripMenuItem.Checked = false;
            else autoscrollToolStripMenuItem.Checked = true;
        }

        private void LogToGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (logToGridToolStripMenuItem.Checked)
            {
                logToGridToolStripMenuItem.Checked = false;
                ((Control) tabPage1).Enabled = false;
                if (logToTextToolStripMenuItem.Checked == false)
                {
                    tabControl1.Enabled = false;
                    tabControl1.Visible = false;
                }
            }
            else
            {
                logToGridToolStripMenuItem.Checked = true;
                ((Control) tabPage1).Enabled = true;
                tabControl1.Enabled = true;
                tabControl1.Visible = true;
            }
        }

        private void CheckBox_Mark_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Mark.Checked) checkBox_Mark.Font = new Font(checkBox_Mark.Font, FontStyle.Bold);
            else checkBox_Mark.Font = new Font(checkBox_Mark.Font, FontStyle.Regular);
        }

        private void TextBox_serverPort_Leave(object sender, EventArgs e)
        {
            int.TryParse(textBox_serverPort.Text, out ServerPort);
            textBox_serverPort.Text = ServerPort.ToString();
        }

        private void TextBox_clientPort_Leave(object sender, EventArgs e)
        {
            int.TryParse(textBox_clientPort.Text, out ClientPort);
            textBox_clientPort.Text = ClientPort.ToString();
        }

        private void LogToTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (logToTextToolStripMenuItem.Checked)
            {
                logToTextToolStripMenuItem.Checked = false;
                ((Control) tabPage2).Enabled = false;
                if (logToGridToolStripMenuItem.Checked == false)
                {
                    tabControl1.Enabled = false;
                    tabControl1.Visible = false;
                }
            }
            else
            {
                logToTextToolStripMenuItem.Checked = true;
                ((Control) tabPage2).Enabled = true;
                tabControl1.Enabled = true;
                tabControl1.Visible = true;
            }
        }

        private void Button_clear_Click(object sender, EventArgs e)
        {
            textBox_terminal1.Clear();
            CSVdataTable.Rows.Clear();
        }

        private void TabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tabPage1 && logToTextToolStripMenuItem.Checked == false)
                e.Cancel = true;
            if (e.TabPage == tabPage2 && logToGridToolStripMenuItem.Checked == false)
                e.Cancel = true;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            var bytesFromClient = new List<byte>();
            var bytesFromServer = new List<byte>();

            //accept client if any pending
            if (ClientSocketListener.Pending())
            {
                try
                {
                    ClientSocket = ClientSocketListener.AcceptTcpClient();
                    var clientAddr = ((IPEndPoint) ClientSocket.Client.RemoteEndPoint).Address;
                    if (textBox_clientIP.Text != "")
                    {
                        IPAddress.TryParse(textBox_clientIP.Text, out var setClientAddr);
                        if (!clientAddr.Equals(setClientAddr))
                        {
                            ClientSocket.Client.Disconnect(false);
                            ClientSocket.Close();
                            ClientSocket = new TcpClient();
                            return;
                        }
                    }

                    ClientNetworkStream = ClientSocket.GetStream();
                    ClientStatusChanged("Client connected " + clientAddr + ":" + textBox_clientPort.Text);
                }
                catch (Exception ex)
                {
                    ClientErrorReceived("Accept client: " + ex);
                }

                //connect to server
                try
                {
                    ServerSocket = new TcpClient();
                    ServerSocket.Connect(textBox_serverIP.Text, ServerPort);
                    ServerSocket.ReceiveTimeout = Settings.Default.ReceiveTimeOut;
                    ServerSocket.SendTimeout = Settings.Default.SendTimeOut;
                    ServerSocket.Client.ReceiveTimeout = Settings.Default.ReceiveTimeOut;
                    ServerSocket.Client.SendTimeout = Settings.Default.SendTimeOut;
                    ServerNetworkStream = ServerSocket.GetStream();
                    timer1.Enabled = true;
                    ServerStatusChanged("Connected to server " + textBox_serverIP.Text + ":" + textBox_serverPort.Text);
                }
                catch (Exception ex)
                {
                    ServerErrorReceived("Connect to server: " + ex);
                }
            }

            //read data from client if any
            if (ClientConnected() && ClientSocket.Connected)
                try
                {
                    while (ClientSocket.Available > 0) bytesFromClient.Add((byte) ClientNetworkStream.ReadByte());
                    Client_DataReceived(bytesFromClient.ToArray());
                }
                catch (Exception ex)
                {
                    ClientErrorReceived("Read client: " + ex);
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
                    ClientErrorReceived("Disconnect client: " + ex);
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
                    ServerErrorReceived("Disconnect server: " + ex);
                }
            }

            //reconnect to server if disconnected
            if (ClientConnected() && !ServerConnected())
                try
                {
                    ServerSocket = new TcpClient();
                    ServerSocket.Connect(textBox_serverIP.Text, ServerPort);
                    ServerSocket.ReceiveTimeout = Settings.Default.ReceiveTimeOut;
                    ServerSocket.SendTimeout = Settings.Default.SendTimeOut;
                    ServerSocket.Client.ReceiveTimeout = Settings.Default.ReceiveTimeOut;
                    ServerSocket.Client.SendTimeout = Settings.Default.SendTimeOut;
                    ServerNetworkStream = ServerSocket.GetStream();
                    timer1.Enabled = true;
                    ServerStatusChanged(
                        "Reconnected to server " + textBox_serverIP.Text + ":" + textBox_serverPort.Text);
                }
                catch (Exception ex)
                {
                    ServerErrorReceived("Reconnect server: " + ex);
                }

            //send from client to server
            if (bytesFromClient.Count > 0)
                try
                {
                    ServerNetworkStream.Write(bytesFromClient.ToArray(), 0, bytesFromClient.Count);
                }
                catch (Exception ex)
                {
                    ServerErrorReceived("Send to server: " + ex);
                }

            //read data from server if any
            if (ServerConnected() && ServerSocket.Connected)
                try
                {
                    while (ServerSocket.Available > 0) bytesFromServer.Add((byte) ServerNetworkStream.ReadByte());
                    Server_DataReceived(bytesFromServer.ToArray());
                }
                catch (Exception ex)
                {
                    ServerErrorReceived("Read server: " + ex);
                }

            //send from server to client
            if (bytesFromServer.Count > 0)
                try
                {
                    ClientNetworkStream.Write(bytesFromServer.ToArray(), 0, bytesFromServer.Count);
                }
                catch (Exception ex)
                {
                    ClientErrorReceived("Send to client: " + ex);
                }
        }

        private void TextBox_serverName_Leave(object sender, EventArgs e)
        {
            _serverName = textBox_serverName.Text;
            checkBox_ServerHex.Text = textBox_serverName.Text;
        }

        private void TextBox_clientName_Leave(object sender, EventArgs e)
        {
            _clientName = textBox_clientName.Text;
            checkBox_ClientHex.Text = textBox_clientName.Text;
        }

        private bool ClientConnected()
        {
            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            var tcpConnections = ipProperties.GetActiveTcpConnections();
            foreach (var c in tcpConnections)
            {
                var stateOfConnection = c.State;
                try
                {
                    if (c.LocalEndPoint.Equals(ClientSocket.Client.LocalEndPoint) &&
                        c.RemoteEndPoint.Equals(ClientSocket.Client.RemoteEndPoint))
                    {
                        if (stateOfConnection == TcpState.Established)
                            return true;
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    ClientErrorReceived("Socket closed: " + ex);
                    return false;
                }
            }

            return false;
        }

        private bool ServerConnected()
        {
            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            var tcpConnections = ipProperties.GetActiveTcpConnections();
            foreach (var c in tcpConnections)
            {
                var stateOfConnection = c.State;
                try
                {
                    if (c.LocalEndPoint.Equals(ServerSocket.Client.LocalEndPoint) &&
                        c.RemoteEndPoint.Equals(ServerSocket.Client.RemoteEndPoint))
                    {
                        if (stateOfConnection == TcpState.Established)
                            return true;
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    ServerErrorReceived("Socket closed: " + ex);
                    return false;
                }
            }

            return false;
        }
    }
}
