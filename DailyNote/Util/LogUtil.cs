using DailyNote.Model;
using LYXUI.Control.Toast;
using Newtonsoft.Json;
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

        string dir = "logs";

        string logFile
        {
            get
            {
                var targetPath = GetTargetPath();
                return targetPath + "\\dailynote_" + StartTimeOfDay.ToString("yyyyMMdd") + ".txt";
            }
        }
        //原始数据
        string logDataFile
        {
            get
            {
                var targetPath = GetTargetPath();
                return targetPath + "\\dailynote_" + StartTimeOfDay.ToString("yyyyMMdd") + ".dat";
            }
        }

        public string Title
        {
            get
            {
                var title = $"日报工具-上次更新时间{lastTime.ToShortTimeString()}-上班时间{StartTimeOfDay.ToShortTimeString()}";

                return title;
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
        /// <summary>
        /// 日志记录集合
        /// </summary>
        public List<LogInfoModel> LogInfos { get; set; } = new List<LogInfoModel>();

        private ConfigUtil configUtil = new ConfigUtil();

        public LogUtil()
        {
            //初始化加载旧数据
            Load();
        }

        public DateTime GetStartTime()
        {
            return StartTimeOfDay;
        }
        public string AddLog(string log)
        {
            if (LogInfos.Count == 0)
            {
                lastTime = StartTimeOfDay;
                Window mainwin = Application.Current.MainWindow;
                Toast.MakeToast("当日第一次添加记录，请注意二次确认是否已打上班卡!", mainwin, LYXUI.Control.Toast.Enum.ToastDuringEnum.LONG);
                //提醒是否打卡
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

            SaveRawData();

            return lineStr;
        }

        public void ReSet(DateTime firstTimeOfDay)
        {
            if (firstTimeOfDay > DateTime.Now)
            {
                MessageBox.Show($"开始时间不得大于当前系统时间");
                return;
            }
            //TODO
            if (LogInfos.Count != 0)
            {
                var firstData = LogInfos.First();
                if (firstTimeOfDay > firstData.EndTime)
                {
                    MessageBox.Show($"开始时间不得大于第一条记录的结束时间,结束时间为{firstData.EndTime.ToShortTimeString()}");
                    return;
                }
                //更新首条记录时间
                LogInfos.First().StartTime = firstTimeOfDay;
            }

            StartTimeOfDay = firstTimeOfDay;
            var config = configUtil.GetConfigOfDay(StartTimeOfDay);
            if (config != null)
            {
                configUtil.Modify(config, firstTimeOfDay);
            }
            Save();

            //LogInfos.Clear();
            //Load();
        }

        /// <summary>
        /// 撤销最后一条
        /// </summary>
        public void RevertLast()
        {
            if ((LogInfos?.Count ?? 0) == 0)
            {
                return;
            }
            LogInfos.Remove(LogInfos.Last());



            if ((LogInfos?.Count ?? 0) > 0)
            {
                lastTime = LogInfos.Last().EndTime;
            }
            else
            {
                lastTime = StartTimeOfDay;
            }

            Save();
        }

        public void Load()
        {
            LogInfos.Clear();

            //获取当日上班时间
            Config config = GetConfigOfDay();

            StartTimeOfDay = config.StartTime;
            LoadLog(config);
        }

        /// <summary>
        /// 加载之前的日志数据(当日)
        /// </summary>
        /// <param name="config"></param>
        private void LoadLog(Config config)
        {
            FileInfo file = new FileInfo(logDataFile);

            if (!file.Exists)
            {
                return;
            }
            var strData = File.ReadAllText(logDataFile);
            if (strData == null || strData.Trim() == string.Empty)
            {
                return;
            }

            LogInfos = JsonConvert.DeserializeObject<List<LogInfoModel>>(strData);
            lastTime = LogInfos.Last().EndTime;
            //foreach (var line in File.ReadAllLines(logFile))
            //{
            //    var data = line.Split(" ");
            //    var log = data[0];
            //    var timeDurStr = data[1].Replace(EndStr, "");
            //    double timeDur;
            //    double.TryParse(timeDurStr, out timeDur);
            //    var endTime = tmpTime.AddHours(timeDur);
            //    LogInfos.Add(new LogInfoModel
            //    {
            //        Log = log,
            //        StartTime = tmpTime,
            //        EndTime = endTime
            //    });
            //    lastTime = endTime;
            //}
        }

        private Config GetConfigOfDay()
        {
            var config = configUtil.GetConfigOfDay(StartTimeOfDay);
            if (config == null)
            {
                config = configUtil.Add(StartTimeOfDay);
            }

            return config;
        }

        public void Save()
        {
            StringBuilder builder = new StringBuilder();

            foreach (var item in LogInfos)
            {
                builder.Append($"{GetLogLine(item)}\n");
            }

            File.WriteAllText(logFile, builder.ToString());
            SaveRawData();
        }

        private void SaveRawData()
        {
            if ((LogInfos?.Count ?? 0) == 0)
            {
                return;
            }
            File.WriteAllText(logDataFile, JsonConvert.SerializeObject(LogInfos));
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
                string hours = $"总时长 {GetTotalHours()}H\n";

                var copyText = $"{hours}{GetDailyNotes()}";
                Clipboard.SetText(copyText);
            }
            catch (Exception e)
            {
                throw new Exception($"获取剪贴板失败,{e.Message},可直接打开文件:{logFile}");
            }
        }

        public double GetTotalHours()
        {
            if ((LogInfos?.Count ?? 0) == 0)
            {
                return 0;
            }
            var hours = (LogInfos.Last().EndTime - LogInfos.First().StartTime).TotalHours;

            var result = Math.Round(hours, 1);
            return result;
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
