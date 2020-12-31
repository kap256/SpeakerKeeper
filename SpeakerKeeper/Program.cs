using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpeakerKeeper
{
    static class Program
    {

        const string mutexName = "SK32_756e7162-a066-4e77-91c7";

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Mutex mutex = new System.Threading.Mutex(false, mutexName);
            bool hasHandle = false;
            try {
                //多重起動の抑止
                try {
                    hasHandle = mutex.WaitOne(0, false);
                } catch (AbandonedMutexException) {
                    hasHandle = true;
                }

                if (hasHandle == false) {
                    return;
                }

                //メイン処理へ
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            } catch (Exception e) {
                MessageBox.Show(e.Message);
            } finally {
                if (hasHandle) {
                    mutex.ReleaseMutex();
                }
                mutex.Close();
            }
        }
    }
}
