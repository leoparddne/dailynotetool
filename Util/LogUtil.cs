using DailyNote.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DailyNote.Util
{
    public class LogUtil
    {
        string EndStr = "H";//单条日志记录后缀字符串
        string blandStr = " ";//日志信息与时间分隔符
        DateTime StartTimeOfDay = DateTime.Now;

        DateTime lastTime;

        string logFile
        {
            get
            {
                return "dailynote_" + StartTimeOfDay.ToString("yyyyMMdd");
            }
        }

        /// <summary>
        /// 日志记录集合
        /// </summary>
        public List<LogInfoModel> LogInfos { get; set; } = new List<LogInfoModel>();


        public void AddLog()
        {
            if (LogInfos.Count == 0)
            {
                lastTime = StartTimeOfDay;
            }

            LogInfos.Add(new LogInfoModel
            {
                StartTime = lastTime,
                EndTime = DateTime.Now
            });
        }

        public void ReSet(DateTime firstTimeOfDay)
        {
            //TODO
            StartTimeOfDay = firstTimeOfDay;
        }

        public void Load()
        {
            LogInfos.Clear();
            FileInfo file = new FileInfo(logFile);

            if (!file.Exists)
            {
                return;
            }

            DateTime tmpTime = StartTimeOfDay;

            foreach (var line in File.ReadAllLines(logFile))
            {
                var data = line.Split(" ");
                var log = data[0];
                var timeDurStr = data[1].Replace(EndStr, "");
                double timeDur;
                double.TryParse(timeDurStr, out timeDur);
                var endTime = tmpTime.AddHours(timeDur);
                LogInfos.Add(new LogInfoModel
                {
                    Log = log,
                    StartTime = tmpTime,
                    EndTime = endTime
                }); ;
            }
        }

        public void Save()
        {
            StringBuilder builder = new StringBuilder();

            foreach (var item in LogInfos)
            {
                var showHour = Math.Round(item.Hours, 1);
                builder.Append($"{item.Log}{blandStr}{showHour}\n");
            }

            File.WriteAllText(logFile, builder.ToString());
        }

        public void Copy()
        {
            FileInfo file = new FileInfo(logFile);

            if (!file.Exists)
            {
                return;
            }

            var info = File.ReadAllText(logFile);
            try
            {
                Clipboard.SetText(info);
            }
            catch (Exception e)
            {
                MessageBox.Show($"获取剪贴板失败,可直接打开文件:{logFile}");
            }
        }
    }
}
