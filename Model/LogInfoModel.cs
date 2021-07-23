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
                return (EndTime - StartTime).TotalHours;
            }
        }

        public string Log { get; set; }
    }
}
