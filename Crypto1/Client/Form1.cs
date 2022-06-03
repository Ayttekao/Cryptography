﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.SignalRClient;

namespace Client
{
    public partial class Form1 : Form
    {
        private const String ServerUrl = "https://localhost:5001/chat";
        private SignalRClientImpl _signalRClient;
        private Boolean dragable = false;
        private Point startPosition;

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
        }

        private async void SendButton_Click(object sender, EventArgs e)
        {
            var opf = new OpenFileDialog();
            opf.Multiselect = true;
            if (opf.ShowDialog() == DialogResult.OK)
            {
                foreach (var nameFile in opf.FileNames)
                {
                    var modeAsString = ModesComboBox.SelectedItem.ToString();
                    await Task.Run(async () =>
                    {
                        await _signalRClient.BroadcastFile(nameFile, modeAsString);
                    });
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
                await Task.Run(async () =>
                {
                    await _signalRClient.SendFile(item.ToString(), modeAsString);
                });
            }
        }

        private async void RefreshButton_Click(object sender, EventArgs e)
        {

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

        private void MakeDragable(object sender, MouseEventArgs e)
        {
            dragable = true;
            startPosition = e.Location;
        }

        private void DragForm(object sender, MouseEventArgs e)
        {
            if (dragable)
            {
                Location = new Point(Cursor.Position.X - startPosition.X, Cursor.Position.Y - startPosition.Y);
            }
        }

        private void DisableDrag(object sender, MouseEventArgs e)
        {
            dragable = false;
        }
    }
}