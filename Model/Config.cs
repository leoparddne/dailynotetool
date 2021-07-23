using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyNote
{
    public class Config
    {
        public string Key { get; set; }

        /// <summary>
        /// 当日上班时间
        /// </summary>
        public DateTime StartTime { get; set; }

        public Config(DateTime StartTime)
        {
            this.StartTime = StartTime;
            GenKey();
        }

        public void GenKey()
        {
            Key = StartTime.ToString("yyyyMMdd");
        }
    }
}
