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
        //每日开始时间
        DateTime startTime;
        DateTime lastTime;
        //开始时间
        DateTime LastTime
        {
            get => lastTime;
            set
            {
                lastTime = value;
                this.Title = $"日报工具-上次更新时间{value.ToShortTimeString()}";
                filePath = $"dailynote_{DateTime.Now.ToString("yyyyMMdd")}.txt";
            }
        }
        string filePath = $"dailynote_{DateTime.Now.ToString("yyyyMMdd")}.txt";

        //保存程序配置
        Config config = new Config();


        LogUtil log = new LogUtil();


        public MainWindow()
        {
            InitializeComponent();
            InjectData();
            startTime = LastTime = DateTime.Now;
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
            var appendText = txtWork.Text.Trim();

            //添加新日志数据
            if (appendText != null && appendText != string.Empty)
            {
                FileInfo resultFile = new FileInfo(filePath);

                var text = resultFile.AppendText();
                var timeSpan = DateTime.Now - LastTime;
                var time = Math.Round(timeSpan.TotalHours, 1);
                if (time == 0)
                {
                    time = 0.1;
                }
                text.WriteLine($"{appendText} {time}H");

                text.Close();

                LastTime = DateTime.Now;
            }

            if (!File.Exists(filePath))
            {
                return;
            }
            //有日志文件,读取复制到剪贴板
            txtAllNote.Text = File.ReadAllText(filePath);

            try
            {
                Clipboard.SetText(txtAllNote.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开剪贴板失败" + ex.Message);
                return;
            }

            //清空数据
            txtWork.Text = "";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(filePath))
            {
                txtAllNote.Text = File.ReadAllText(filePath);

            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("是否确认重置时间", "提示", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            startTime = LastTime = DateTime.Now;

            //filePath = $"dailynote_{DateTime.Now.ToString("yyyyMMdd")}.txt";

            if (File.Exists(filePath))
            {
                txtAllNote.Text = File.ReadAllText(filePath);
            }
            else
            {
                txtAllNote.Text = "";
                return;
            }
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
            startTime = LastTime = new DateTime(data.Year, data.Month, data.Day, hour, minute, 0);

            //filePath = $"dailynote_{DateTime.Now.ToString("yyyyMMdd")}.txt";

            if (File.Exists(filePath))
            {
                txtAllNote.Text = File.ReadAllText(filePath);
            }
            else
            {
                txtAllNote.Text = "";
                return;
            }
        }

        private void txtAllNote_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //有日志文件,读取复制到剪贴板
            txtAllNote.Text = File.ReadAllText(filePath);

            try
            {
                Clipboard.SetText(txtAllNote.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开剪贴板失败" + ex.Message);
                return;
            }
        }
    }
}
