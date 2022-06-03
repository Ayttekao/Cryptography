using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.SignalRClient;

namespace Client
{
    public partial class Form1 : Form
    {
        private const String ServerUrl = "https://localhost:5001/chat";
        private SignalRClientImpl _signalRClient;
        private Boolean _dragable;
        private Point _startPosition;

        public Form1()
        {
            InitializeComponent();
            ModesComboBox.SelectedItem = "CBC";
            SendButton.Enabled = false;
            RefreshButton.Enabled = false;
            DownloadButton.Enabled = false;
            ServerCheckedListBox.Enabled = false;
        }

        private async void ConnectionButton_Click(object sender, EventArgs e)
        {
            _signalRClient = new SignalRClientImpl(ServerUrl);
            
            try
            {
                DoLog("\nTrying connect to server...\n");
                ConnectionButton.Enabled = false;
                await _signalRClient.RegistersHandlers();
                await _signalRClient.Start();
                await _signalRClient.ScanFilesDir();
                await _signalRClient.Handshake();
                await RefreshListBox(_signalRClient.GetServerStore());

                SendButton.Enabled = true;
                RefreshButton.Enabled = true;
                DownloadButton.Enabled = true;
                ServerCheckedListBox.Enabled = true;
            }
            catch (Exception)
            {
                MessageBox.Show(@"Сan't connect to the server");
            }
            DoLog("Connected successfully\n");
        }

        private async void SendButton_Click(object sender, EventArgs e)
        {
            var opf = new OpenFileDialog();
            opf.Multiselect = true;
            if (opf.ShowDialog() == DialogResult.OK)
            {
                foreach (var fileName in opf.FileNames)
                {
                    var modeAsString = ModesComboBox.SelectedItem.ToString();
                    DoLog("Encrypt file " + Path.GetFileName(fileName) + "\n");
                    await Task.Run(async () =>
                    {
                        await _signalRClient.BroadcastFile(fileName, modeAsString);
                    });
                    DoLog("Send file " + Path.GetFileName(fileName) + "\n");
                    await RefreshListBox(_signalRClient.GetServerStore());
                }
            }
        }

        private async void DownloadButton_Click(object sender, EventArgs e)
        {
            var selectedItems = ServerCheckedListBox.CheckedItems;
            var modeAsString = ModesComboBox.SelectedItem.ToString();
            foreach (var item in selectedItems)
            {
                DoLog("Decrypt file " + item + "\n");
                await Task.Run(async () =>
                {
                    await _signalRClient.SendFile(item.ToString(), modeAsString);
                });
                DoLog("Saving a file " + item + "\n");
            }
        }

        private async void RefreshButton_Click(object sender, EventArgs e)
        {
            await _signalRClient.ScanFilesDir();
            await RefreshListBox(_signalRClient.GetServerStore());
            DoLog("Successfully updated\n");
        }

        public Task RefreshListBox(ICollection<String> fileNames)
        {
            ServerCheckedListBox.Items.Clear();
            foreach (var fileName in fileNames)
            {
                ServerCheckedListBox.Items.Add(fileName);
            }

            return Task.CompletedTask;
        }

        private void DoLog(String message)
        {
            logTextBox.Text += message;
        }

        private void HideForm_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void CloseForm_Click(object sender, EventArgs e)
        {
            Close();
            Application.ExitThread();
            Application.Exit();
        }

        private void MakeDraggable(object sender, MouseEventArgs e)
        {
            _dragable = true;
            _startPosition = e.Location;
        }

        private void DragForm(object sender, MouseEventArgs e)
        {
            if (_dragable)
            {
                Location = new Point(Cursor.Position.X - _startPosition.X, Cursor.Position.Y - _startPosition.Y);
            }
        }

        private void DisableDrag(object sender, MouseEventArgs e)
        {
            _dragable = false;
        }
    }
}