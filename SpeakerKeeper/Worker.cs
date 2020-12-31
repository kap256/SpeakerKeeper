using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpeakerKeeper
{
    class Worker
    {
        const int SLEEP_TIME = 500;
        const int CAPTURE_TIME = 10*1000;
        const int SLEEP_COUNT = CAPTURE_TIME/SLEEP_TIME;

        readonly MainForm Form;
        CancellationToken Token;
        DateTime LastCapturedTime;
        TimeSpan Threshold;

        public Worker(MainForm form,CancellationToken token,int thr_sec)
        {
            Form = form;
            Token = token;
            Threshold = new TimeSpan(0, 0, thr_sec);
        }
        public void Work()
        {
            Form.OutputText($"起動しました。 無音検出時間-{Threshold:hh\\:mm\\:ss}");
            while(true) {
                try {
                    using(var stream = new AudioStream()) {
                        LastCapturedTime = DateTime.Now;
                        //var sleep = stream.SleepMSec; サンプルが落ちようが全く構わないので、規定のスリープ間隔は無視する。
                        while(true) {
                            for(int i = 0; i < SLEEP_COUNT; i++) {
                                //一度にキャプチャー間隔分スリープすると、キャンセルへの反応が遅れるため。
                                Thread.Sleep(SLEEP_TIME);
                                Token.ThrowIfCancellationRequested();
                            }
                            
                            //キャプチャー
                            var captured = stream.Capture();
                            Form.UpdateIcon(captured, LastCapturedTime);
                            if(captured) {
                                LastCapturedTime = DateTime.Now;
                            } else {
                                var silent = DateTime.Now - LastCapturedTime;
                                Form.DebugText($"silent = {silent} ");
                                if(silent> Threshold) {
                                    Form.OutputText("無音時間が規定を超えました。音声を再生します。");
                                    Form.PlaySound();
                                    LastCapturedTime = DateTime.Now;
                                }
                            }
                            Token.ThrowIfCancellationRequested();
                        }
                    }
                } catch(OperationCanceledException) {
                    return;
                } catch(Exception ex) {
                    try {
                        Form.OutputText($"エラー：{ex.Message}");
                    } catch(OperationCanceledException) {
                        return;
                    }
                }
            }
        }
    }
}
