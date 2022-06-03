
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ConnectionButton = new System.Windows.Forms.Button();
            this.DownloadButton = new System.Windows.Forms.Button();
            this.SendButton = new System.Windows.Forms.Button();
            this.DownloadProgressBar = new System.Windows.Forms.ProgressBar();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.SendProgressBar = new System.Windows.Forms.ProgressBar();
            this.ServerCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.ModesComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.topPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.closeForm = new System.Windows.Forms.Button();
            this.hideForm = new System.Windows.Forms.Button();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ConnectionButton
            // 
            this.ConnectionButton.Location = new System.Drawing.Point(12, 55);
            this.ConnectionButton.Name = "ConnectionButton";
            this.ConnectionButton.Size = new System.Drawing.Size(150, 50);
            this.ConnectionButton.TabIndex = 1;
            this.ConnectionButton.Text = "Connect";
            this.ConnectionButton.UseVisualStyleBackColor = true;
            this.ConnectionButton.Click += new System.EventHandler(this.ConnectionButton_Click);
            // 
            // DownloadButton
            // 
            this.DownloadButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DownloadButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.DownloadButton.Image = ((System.Drawing.Image)(resources.GetObject("DownloadButton.Image")));
            this.DownloadButton.Location = new System.Drawing.Point(260, 55);
            this.DownloadButton.Name = "DownloadButton";
            this.DownloadButton.Size = new System.Drawing.Size(50, 50);
            this.DownloadButton.TabIndex = 2;
            this.DownloadButton.UseVisualStyleBackColor = true;
            this.DownloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // SendButton
            // 
            this.SendButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SendButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.SendButton.Image = ((System.Drawing.Image)(resources.GetObject("SendButton.Image")));
            this.SendButton.Location = new System.Drawing.Point(186, 55);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(50, 50);
            this.SendButton.TabIndex = 3;
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // DownloadProgressBar
            // 
            this.DownloadProgressBar.Location = new System.Drawing.Point(593, 442);
            this.DownloadProgressBar.Name = "DownloadProgressBar";
            this.DownloadProgressBar.Size = new System.Drawing.Size(204, 36);
            this.DownloadProgressBar.TabIndex = 4;
            // 
            // RefreshButton
            // 
            this.RefreshButton.Location = new System.Drawing.Point(593, 58);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(116, 58);
            this.RefreshButton.TabIndex = 5;
            this.RefreshButton.Text = "Refresh";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // SendProgressBar
            // 
            this.SendProgressBar.Location = new System.Drawing.Point(593, 122);
            this.SendProgressBar.Name = "SendProgressBar";
            this.SendProgressBar.Size = new System.Drawing.Size(204, 33);
            this.SendProgressBar.Step = 1;
            this.SendProgressBar.TabIndex = 7;
            // 
            // ServerCheckedListBox
            // 
            this.ServerCheckedListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.ServerCheckedListBox.ForeColor = System.Drawing.SystemColors.Window;
            this.ServerCheckedListBox.FormattingEnabled = true;
            this.ServerCheckedListBox.Location = new System.Drawing.Point(12, 122);
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
            this.ModesComboBox.Location = new System.Drawing.Point(326, 78);
            this.ModesComboBox.Name = "ModesComboBox";
            this.ModesComboBox.Size = new System.Drawing.Size(151, 28);
            this.ModesComboBox.TabIndex = 9;
            this.ModesComboBox.ValueMember = "CBC";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(326, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 20);
            this.label1.TabIndex = 10;
            this.label1.Text = "Mode";
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.topPanel.Controls.Add(this.label2);
            this.topPanel.Controls.Add(this.closeForm);
            this.topPanel.Controls.Add(this.hideForm);
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(811, 40);
            this.topPanel.TabIndex = 11;
            this.topPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MakeDraggable);
            this.topPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DragForm);
            this.topPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DisableDrag);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.SystemColors.Window;
            this.label2.Location = new System.Drawing.Point(12, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 38);
            this.label2.TabIndex = 2;
            this.label2.Text = "CryptoDump";
            // 
            // closeForm
            // 
            this.closeForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeForm.ForeColor = System.Drawing.Color.MidnightBlue;
            this.closeForm.Image = ((System.Drawing.Image)(resources.GetObject("closeForm.Image")));
            this.closeForm.Location = new System.Drawing.Point(760, 6);
            this.closeForm.Name = "closeForm";
            this.closeForm.Size = new System.Drawing.Size(33, 29);
            this.closeForm.TabIndex = 1;
            this.closeForm.UseVisualStyleBackColor = true;
            this.closeForm.Click += new System.EventHandler(this.CloseForm_Click);
            // 
            // hideForm
            // 
            this.hideForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.hideForm.ForeColor = System.Drawing.Color.MidnightBlue;
            this.hideForm.Image = ((System.Drawing.Image)(resources.GetObject("hideForm.Image")));
            this.hideForm.Location = new System.Drawing.Point(711, 6);
            this.hideForm.Name = "hideForm";
            this.hideForm.Size = new System.Drawing.Size(33, 29);
            this.hideForm.TabIndex = 0;
            this.hideForm.UseVisualStyleBackColor = true;
            this.hideForm.Click += new System.EventHandler(this.HideForm_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.ClientSize = new System.Drawing.Size(809, 500);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ModesComboBox);
            this.Controls.Add(this.ServerCheckedListBox);
            this.Controls.Add(this.SendProgressBar);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.DownloadProgressBar);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.DownloadButton);
            this.Controls.Add(this.ConnectionButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
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
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Button closeForm;
        private System.Windows.Forms.Button hideForm;
        private System.Windows.Forms.Label label2;
    }
}

