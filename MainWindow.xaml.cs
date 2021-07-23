using DailyNote.Model;
using DailyNote.Util;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace DailyNote
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DateTime startTime;
        //每日开始时间
        DateTime StartTime
        {
            get => startTime;
            set
            {
                startTime = value;
                SyncTitle();
            }
        }
        DateTime lastTime;
        //开始时间
        DateTime LastTime
        {
            get => lastTime;
            set
            {
                lastTime = value;
                SyncTitle();
            }
        }


        LogUtil log = new LogUtil();

        public void SyncTitle()
        {
            this.Title = $"日报工具-上次更新时间{LastTime.ToShortTimeString()}-上班时间{startTime.ToShortTimeString()}";
        }
        public MainWindow()
        {
            InitializeComponent();
            InjectData();
            StartTime = LastTime = DateTime.Now;
        }
        private void InjectData()
        {
            for (int i = 0; i < 24; i++)
            {
                cobHour.Items.Add(i);
            }
            for (int i = 0; i < 60; i++)
            {
                cobMinute.Items.Add(i);
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var appendText = txtWork.Text.Trim();
                var logStr = log.AddLog(appendText);
                LastTime = DateTime.Now;
                txtAllNote.AppendText($"{logStr}\n");
                //清空数据
                txtWork.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtAllNote.Text = log.GetDailyNotes();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("是否确认重置时间", "提示", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            StartTime = DateTime.Now;


            txtAllNote.Text = log.GetDailyNotes();
        }
        private void btnResetFromSelectTime_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("是否确认重置时间", "提示", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            var data = DateTime.Now;
            int hour;
            int minute;
            int.TryParse(cobHour.SelectedItem.ToString(), out hour);
            int.TryParse(cobHour.SelectedItem.ToString(), out minute);

            //重置时间
            StartTime = new DateTime(data.Year, data.Month, data.Day, hour, minute, 0);

            //filePath = $"dailynote_{DateTime.Now.ToString("yyyyMMdd")}.txt";

            txtAllNote.Text = log.GetDailyNotes();
        }

        private void clipCopy(object sender, MouseButtonEventArgs e)
        {
            //有日志文件,读取复制到剪贴板
            txtAllNote.Text = log.GetDailyNotes();

            try
            {
                log.Copy2Clipboard();
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开剪贴板失败" + ex.Message);
                return;
            }
        }
    }
}
