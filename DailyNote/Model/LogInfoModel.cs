using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyNote.Model
{
    public class LogInfoModel
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public double Hours
        {
            get
            {
                var time = Math.Round((EndTime - StartTime).TotalHours, 1);
                if (time == 0)
                {
                    time = 0.1;
                }
                return time;
            }
        }

        public string Log { get; set; }
    }
}
