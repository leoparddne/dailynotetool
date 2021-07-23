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

        public string configFile
        {
            get
            {
                return "dailynote_" + StartTime.ToString("yyyyMM");
            }
        }

        public Config()
        {
            //初始化配置
        }
    }
}
