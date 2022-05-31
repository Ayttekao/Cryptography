using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.SignalRClient;
using Client.Stuff;
using CourseWork.LOKI97.AlgorithmService.Modes;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client
{
    public partial class Form1 : Form
    {
        private const String ServerUrl = "https://localhost:5001/chat";
        private const String DownloadFolderName = "Downloads";
        private static readonly string CurrentPath = AppDomain.CurrentDomain.BaseDirectory + DownloadFolderName;
        private SignalRClientImpl _signalRClient;

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
                await _signalRClient.RegistersHandlers();
                await _signalRClient.Start();
                await _signalRClient.ScanFilesDir();
                await RefreshListBox(_signalRClient.GetServerStore());

                SendButton.Enabled = true;
                RefreshButton.Enabled = true;
                DownloadButton.Enabled = true;
                ServerCheckedListBox.Enabled = true;
            }
            catch (Exception exception)
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
                    var mode = (EncryptionMode)Enum.Parse(typeof(EncryptionMode), ModesComboBox.SelectedItem.ToString()!);
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
            foreach (var item in selectedItems)
            {
                await Task.Run(async () =>
                {
                    await _signalRClient.SendFile(item.ToString());
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

        /*private async Task SetPublicKey(PublicKeys publicKeys)
        {
            _benaloh.SetPublicKey(publicKeys);
        }*/

        /*private static List<BigInteger> GenerateKey()
        {
            var rnd = new Random();
            var ringModulo = BigInteger.Pow(2, 16) + BigInteger.One;
            var values = new List<BigInteger>(ValuesCount);
            for (var i = 0; i < ValuesCount; i++)
            {
                values.Add(Utils.RandomBigInteger(BigInteger.Zero, ringModulo));
            }

            var keyByteArray = new Byte[ValuesCount][];
            for (var i = 0; i < ValuesCount; i++)
            {
                keyByteArray[i] = values[i].ToByteArray();
            }
            var temp = new Byte[8];
            var count = 0;
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    temp[count] = keyByteArray[i][j];
                    count++;
                }
            }
            return values;
        }

        private Byte[] GetByteArray(int size)
        {
            var rnd = new Random();
            var b = new Byte[size];
            rnd.NextBytes(b);
            return b;
        }*/
    }
}