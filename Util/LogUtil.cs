using DailyNote.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
                return "dailynote_" + StartTimeOfDay.ToString("yyyyMMdd") + ".txt";
            }
        }

        /// <summary>
        /// 日志记录集合
        /// </summary>
        public List<LogInfoModel> LogInfos { get; set; } = new List<LogInfoModel>();

        private ConfigUtil configUtile = new ConfigUtil();

        public LogUtil()
        {
            //初始化加载旧数据
            Load();
        }
        public string AddLog(string log)
        {
            if (LogInfos.Count == 0)
            {
                lastTime = StartTimeOfDay;
            }

            var logInfo = new LogInfoModel
            {
                StartTime = lastTime,
                EndTime = DateTime.Now,
                Log = log
            };
            LogInfos.Add(logInfo);

            FileInfo file = new FileInfo(logFile);
            var writer = file.AppendText();
            string lineStr = GetLogLine(logInfo);
            writer.WriteLine(lineStr);

            writer.Close();
            lastTime = logInfo.EndTime;

            return lineStr;
        }

        public void ReSet(DateTime firstTimeOfDay)
        {
            //TODO
            StartTimeOfDay = firstTimeOfDay;
            if (LogInfos.Count != 0)
            {
                //更新首条记录时间
                LogInfos.First().StartTime = StartTimeOfDay;
            }

            Save();
        }

        public void Load()
        {
            LogInfos.Clear();

            //获取当日上班时间
            Config config = GetConfigOfDay();

            LoadLog(config);
        }

        /// <summary>
        /// 加载之前的日志数据(当日)
        /// </summary>
        /// <param name="config"></param>
        private void LoadLog(Config config)
        {
            DateTime tmpTime = config.StartTime;


            FileInfo file = new FileInfo(logFile);

            if (!file.Exists)
            {
                return;
            }
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
                });
                lastTime = endTime;
            }
        }

        private Config GetConfigOfDay()
        {
            var config = configUtile.GetConfigOfDay(StartTimeOfDay);
            if (config == null)
            {
                config = configUtile.Add(StartTimeOfDay);
            }

            return config;
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

        private string GetLogLine(LogInfoModel logInfo)
        {
            var result = $"{logInfo.Log}{blandStr}{logInfo.Hours}{EndStr}";

            return result;
        }

        public void Copy2Clipboard()
        {
            try
            {
                Clipboard.SetText(GetDailyNotes());
            }
            catch (Exception e)
            {
                throw new Exception($"获取剪贴板失败,{e.Message},可直接打开文件:{logFile}");
            }
        }

        /// <summary>
        /// 获取日志信息
        /// </summary>
        /// <returns></returns>
        public string GetDailyNotes()
        {
            FileInfo file = new FileInfo(logFile);

            if (!file.Exists)
            {
                return "";
            }

            var info = File.ReadAllText(logFile);
            return info;
        }
    }
}
