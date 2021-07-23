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
        public string configFile
        {
            get
            {
                return "dailynote.cfg";
            }
        }

        private List<Config> configs= new();

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
            var result = new Config(day);
            configs.Add(result);

            File.WriteAllText(configFile, JsonConvert.SerializeObject(configs));
            return result;
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
