using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SpeakerKeeper
{
    class AudioStream:IDisposable
    {
        const int MY_CAPTURED = 10;
        const int MY_NO_DATA = 20;

        [DllImport("SKDLLpp.dll")]
        static extern int InitAudioStream();

        [DllImport("SKDLLpp.dll")]
        static extern int CaptureAudioStream();

        [DllImport("SKDLLpp.dll")]
        static extern void DisposeAudioStream();

        [DllImport("SKDLLpp.dll")]
        static extern int GetSleepMSec();

        public int SleepMSec { get { return GetSleepMSec(); } }

        public AudioStream()
        {
            var ret = InitAudioStream();
            if(ret < 0) {
                throw new Exception("AudioStreamの初期化に失敗しました。");
            }
        }

        public bool Capture()
        {
            var ret = CaptureAudioStream();
            if(ret < 0) {
                throw new Exception("AudioStreamのキャプチャに失敗しました。");
            }

            return (ret == MY_CAPTURED);
        }


        public void Dispose()
        {
            DisposeAudioStream();
        }
    }
}
