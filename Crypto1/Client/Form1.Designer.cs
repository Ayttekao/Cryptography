
namespace Client
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ConnectionButton = new System.Windows.Forms.Button();
            this.DownloadButton = new System.Windows.Forms.Button();
            this.SendButton = new System.Windows.Forms.Button();
            this.DownloadProgressBar = new System.Windows.Forms.ProgressBar();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.SendProgressBar = new System.Windows.Forms.ProgressBar();
            this.ServerCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.ModesComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ConnectionButton
            // 
            this.ConnectionButton.Location = new System.Drawing.Point(12, 17);
            this.ConnectionButton.Name = "ConnectionButton";
            this.ConnectionButton.Size = new System.Drawing.Size(150, 46);
            this.ConnectionButton.TabIndex = 1;
            this.ConnectionButton.Text = "Connect";
            this.ConnectionButton.UseVisualStyleBackColor = true;
            this.ConnectionButton.Click += new System.EventHandler(this.ConnectionButton_Click);
            // 
            // DownloadButton
            // 
            this.DownloadButton.Location = new System.Drawing.Point(641, 335);
            this.DownloadButton.Name = "DownloadButton";
            this.DownloadButton.Size = new System.Drawing.Size(115, 61);
            this.DownloadButton.TabIndex = 2;
            this.DownloadButton.Text = "Download files";
            this.DownloadButton.UseVisualStyleBackColor = true;
            this.DownloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(628, 12);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(128, 66);
            this.SendButton.TabIndex = 3;
            this.SendButton.Text = "Select files to send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // DownloadProgressBar
            // 
            this.DownloadProgressBar.Location = new System.Drawing.Point(593, 402);
            this.DownloadProgressBar.Name = "DownloadProgressBar";
            this.DownloadProgressBar.Size = new System.Drawing.Size(204, 36);
            this.DownloadProgressBar.TabIndex = 4;
            // 
            // RefreshButton
            // 
            this.RefreshButton.Location = new System.Drawing.Point(471, 17);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(116, 58);
            this.RefreshButton.TabIndex = 5;
            this.RefreshButton.Text = "Refresh";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // SendProgressBar
            // 
            this.SendProgressBar.Location = new System.Drawing.Point(593, 84);
            this.SendProgressBar.Name = "SendProgressBar";
            this.SendProgressBar.Size = new System.Drawing.Size(204, 33);
            this.SendProgressBar.Step = 1;
            this.SendProgressBar.TabIndex = 7;
            // 
            // ServerCheckedListBox
            // 
            this.ServerCheckedListBox.FormattingEnabled = true;
            this.ServerCheckedListBox.Location = new System.Drawing.Point(12, 82);
            this.ServerCheckedListBox.Name = "ServerCheckedListBox";
            this.ServerCheckedListBox.Size = new System.Drawing.Size(563, 356);
            this.ServerCheckedListBox.TabIndex = 8;
            // 
            // ModesComboBox
            // 
            this.ModesComboBox.DisplayMember = "CBC";
            this.ModesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ModesComboBox.FormattingEnabled = true;
            this.ModesComboBox.Items.AddRange(new object[] {
            "ECB",
            "CBC",
            "CFB",
            "OFB",
            "CTR",
            "RD",
            "RDH"});
            this.ModesComboBox.Location = new System.Drawing.Point(605, 206);
            this.ModesComboBox.Name = "ModesComboBox";
            this.ModesComboBox.Size = new System.Drawing.Size(151, 28);
            this.ModesComboBox.TabIndex = 9;
            this.ModesComboBox.ValueMember = "CBC";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(593, 183);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 20);
            this.label1.TabIndex = 10;
            this.label1.Text = "Choose mode for encrypt";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ModesComboBox);
            this.Controls.Add(this.ServerCheckedListBox);
            this.Controls.Add(this.SendProgressBar);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.DownloadProgressBar);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.DownloadButton);
            this.Controls.Add(this.ConnectionButton);
            this.Name = "Form1";
            this.Text = "The best file sharing service";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ConnectionButton;
        private System.Windows.Forms.Button DownloadButton;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.ProgressBar DownloadProgressBar;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.ProgressBar SendProgressBar;
        private System.Windows.Forms.CheckedListBox ServerCheckedListBox;
        private System.Windows.Forms.ComboBox ModesComboBox;
        private System.Windows.Forms.Label label1;
    }
}

