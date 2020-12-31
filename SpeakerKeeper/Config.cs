using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SpeakerKeeper
{
    static class Config
    {
        private static readonly Configuration config;
        public static string SoundFile
        {
            get {
                return GetConfig("SoundFile", @"C:\Windows\Media\notify.wav");
            }
            set {
                SetConfig("SoundFile", value);
            }
        }
        public static string Threshold
        {
            get {
                return GetConfig("Threshold", "600");
            }
            set {
                SetConfig("Threshold", value);
            }
        }

        static Config()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        private static string GetConfig(string key, string def_value)
        {
            var conf = config.AppSettings.Settings[key];
            if(conf == null) {
                SetConfig(key, def_value);
                return def_value;
            } else {
                return conf.Value;
            }
        }

        private static void SetConfig(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var current = config.AppSettings.Settings[key];
            if(current != null) {
                current.Value = value;
            } else {
                config.AppSettings.Settings.Add(key, value);
            }
            config.Save();
        }
    }
}
