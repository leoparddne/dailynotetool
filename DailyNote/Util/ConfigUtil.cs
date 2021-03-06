using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyNote.Util
{
    public class ConfigUtil
    {
        string dir = "runningConfig";
        public string configFile
        {
            get
            {
                string targetDir = GetTargetPath();
                return targetDir + "/dailynote.cfg";
            }
        }

        private string GetTargetPath()
        {
            string targetDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dir);
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            return targetDir;
        }

        private List<Config> configs = new();

        public ConfigUtil()
        {
            string dataStr = string.Empty;
            if (File.Exists(configFile))
            {
                dataStr = File.ReadAllText(configFile);
            }

            if (dataStr != null && dataStr.Trim() != string.Empty)
            {
                configs = JsonConvert.DeserializeObject<List<Config>>(dataStr);
            }
        }

        public Config Add(DateTime day)
        {
            Backup();
            var result = new Config(day);
            configs.Add(result);

            File.WriteAllText(configFile, JsonConvert.SerializeObject(configs));
            return result;
        }
        public void Modify(Config config, DateTime day)
        {
            Backup();
            config.Set(day);

            File.WriteAllText(configFile, JsonConvert.SerializeObject(configs));
        }

        public void Backup()
        {
            if (configs.Count < 100)
            {
                return;
            }
            File.Move(configFile, GetTargetPath() + $"\\{configs.First().Key}_{configs.Last().Key}.cfg");
            configs = new();
        }

        public Config GetConfigOfDay(DateTime day)
        {
            var key = day.ToString("yyyyMMdd");
            var result = configs?.FirstOrDefault(f => f.Key.Trim() == key.Trim());

            return result;
        }
        //文件过大备份
    }
}
