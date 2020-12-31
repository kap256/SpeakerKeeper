namespace SpeakerKeeper
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.Notify = new System.Windows.Forms.NotifyIcon(this.components);
            this.button_Exit = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label_build = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBoxSoundFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Notify
            // 
            this.Notify.Icon = ((System.Drawing.Icon)(resources.GetObject("Notify.Icon")));
            this.Notify.Text = "SpeakerKeeper";
            this.Notify.Visible = true;
            this.Notify.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Notify_Click);
            // 
            // button_Exit
            // 
            this.button_Exit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Exit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Exit.Location = new System.Drawing.Point(354, 344);
            this.button_Exit.Name = "button_Exit";
            this.button_Exit.Size = new System.Drawing.Size(75, 23);
            this.button_Exit.TabIndex = 0;
            this.button_Exit.Text = "終了";
            this.button_Exit.UseVisualStyleBackColor = true;
            this.button_Exit.Click += new System.EventHandler(this.button_Exit_Click);
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(273, 344);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 1;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.WaitOnLoad = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(58, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "SpeakerKeeper";
            // 
            // label_build
            // 
            this.label_build.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_build.AutoSize = true;
            this.label_build.Location = new System.Drawing.Point(11, 355);
            this.label_build.Name = "label_build";
            this.label_build.Size = new System.Drawing.Size(135, 12);
            this.label_build.TabIndex = 9;
            this.label_build.Text = "Build:yyyy/mm/dd hh:mm";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 75);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(417, 263);
            this.textBox1.TabIndex = 10;
            // 
            // textBoxSoundFile
            // 
            this.textBoxSoundFile.Location = new System.Drawing.Point(85, 50);
            this.textBoxSoundFile.Name = "textBoxSoundFile";
            this.textBoxSoundFile.Size = new System.Drawing.Size(344, 19);
            this.textBoxSoundFile.TabIndex = 11;
            this.textBoxSoundFile.TextChanged += new System.EventHandler(this.textBoxSoundFile_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "音声ファイル";
            // 
            // MainForm
            // 
            this.AcceptButton = this.button_ok;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Exit;
            this.ClientSize = new System.Drawing.Size(441, 379);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxSoundFile);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label_build);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.button_Exit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "SpeakerKeeper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon Notify;
        private System.Windows.Forms.Button button_Exit;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_build;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBoxSoundFile;
        private System.Windows.Forms.Label label2;
    }
}

