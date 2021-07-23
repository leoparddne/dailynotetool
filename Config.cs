using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyNote
{
    public class Config
    {
        public DateTime StartTime { get; set; } = DateTime.Now;

        string configFile
        {
            get
            {
                return "dailynote_" + StartTime.ToString("yyyyMMdd");
            }
        }

        public Config()
        {
            //初始化配置
        }
    }
}
