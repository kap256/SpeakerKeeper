using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpeakerKeeper.Properties;

namespace SpeakerKeeper
{
    public partial class MainForm : Form
    {
        Task MainTask;
        CancellationTokenSource CancelSource;
        CancellationToken Cancel;
        SoundPlayer Sound=null;

        #region 初期化-----------------------------------------
        public MainForm()
        {
            InitializeComponent();
            pictureBox1.Image = Resources.play.ToBitmap();
            label_build.Text = "Build : " + GetBuildDateStr();

            textBoxSoundFile.Text = Config.SoundFile;
            LoadSound(textBoxSoundFile.Text);

            CancelSource = new CancellationTokenSource();
            Cancel = CancelSource.Token;
            MainTask = Task.Run(() => {
                var worker = new Worker(this,Cancel,int.Parse(Config.Threshold));
                worker.Work();
            }, Cancel);



#if !DEBUG
            HideForm();
#endif
        }

        public String GetBuildDateStr()
        {
            var asm = GetType().Assembly;
            var ver = asm.GetName().Version;
            
            var build = ver.Build;
            var revision = ver.Revision;
            var baseDate = new DateTime(2000, 1, 1);

            return baseDate.AddDays(build).AddSeconds(revision * 2).ToString("yyyy/MM/dd HH:mm");
        }

        #endregion


        #region サウンド-----------------------------------------
        private bool LoadSound(string path)
        {
            try {
                var sound = new SoundPlayer(path);
                Sound = sound;
                return true;
            } catch(UriFormatException) {
                OutputText($"サウンドファイルを読み込めませんでした。 - {path}");
                return false;
            }
        }
        private void textBoxSoundFile_TextChanged(object sender, EventArgs e)
        {
            var ret = LoadSound(textBoxSoundFile.Text);
            if(ret) {
                Config.SoundFile = textBoxSoundFile.Text;
            } else {
                textBoxSoundFile.Text =  Config.SoundFile;
            }
        }
        #endregion

        #region 起動・終了-----------------------------------------
        private void Notify_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                ShowForm();
            }
        }
        private void button_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button_ok_Click(object sender, EventArgs e)
        {
            HideForm();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try {
                CancelSource.Cancel();
                MainTask.Wait();
            } catch(OperationCanceledException ex) {
                Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {ex.Message}");
            } finally {
                CancelSource.Dispose();
            }
        }

        private void ShowForm()
        {
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void HideForm()
        {
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
        }
        #endregion

        #region ワーカーからの状態更新-----------------------------------------

        public void OutputText(string str)
        {
            if(this.InvokeRequired) {
                Cancel.ThrowIfCancellationRequested();
                this.Invoke(new Action<string>(OutputText), str);
                return;
            }
            textBox1.AppendText($"{str}{Environment.NewLine}");
        }

        public void PlaySound()
        {
            if(this.InvokeRequired) {
                Cancel.ThrowIfCancellationRequested();
                this.Invoke(new Action(PlaySound));
                return;
            }
            if(Sound != null) {
                Sound.Play();
            } else {
                OutputText("※サウンドファイルが読み込まれていません！");
            }
        }

        public void UpdateIcon(bool captured, DateTime last)
        {
            if(this.InvokeRequired) {
                Cancel.ThrowIfCancellationRequested();
                this.Invoke(new Action<bool,DateTime>(UpdateIcon), captured, last);
                return;
            }
            DebugText($"captured = {captured} ,LastCapturedTime = {last}");
            var cap_text=  $"最終キャプチャ時刻 - {last:HH\\:mm\\:ss}";
            if(captured) {
                Notify.Icon = Resources.play;
                Notify.Text = cap_text;
            } else {
                Notify.Icon = Resources.silent;
                Notify.Text = $"{cap_text}{Environment.NewLine}無音時間 - {DateTime.Now - last:hh\\:mm\\:ss}";
            }
        }

        [Conditional("DEBUG")]
        public void DebugText(string str)
        {
            OutputText($"DEBUG:{str}");
        }
        #endregion

    }
}
