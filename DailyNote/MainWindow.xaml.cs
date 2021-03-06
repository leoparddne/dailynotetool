using DailyNote.Util;
using DailyNote.Windows;
using System;
using System.Windows;
using System.Windows.Input;

namespace DailyNote
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LogUtil log = new LogUtil();

        public void SyncTitle()
        {
            this.Title = log.Title;
        }
        public MainWindow()
        {
            InitializeComponent();

            SyncTitle();
        }
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var appendText = txtWork.Text.Trim();
                var logStr = log.AddLog(appendText);
                txtAllNote.AppendText($"{logStr}\n");
                //清空数据
                txtWork.Text = "";
                this.Title = log.Title;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timePicker.SelectedTime = log.GetStartTime();
            lastTime.SelectedTime = timePicker.SelectedTime;//结束时间初始化

            txtAllNote.Text = log.GetDailyNotes();
            SyncTitle();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("是否确认重置时间", "提示", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            log.ReSet(DateTime.Now);
            SyncTitle();

            txtAllNote.Text = log.GetDailyNotes();
        }
        private void btnResetFromSelectTime_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("是否确认重置时间? p.s. 重置操作仅修改第一条记录的开始时间", "提示", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            var data = DateTime.Now;

            //重置时间
            var StartTime = new DateTime(data.Year, data.Month, data.Day,
                timePicker.SelectedTime.Value.Hour, timePicker.SelectedTime.Value.Minute, timePicker.SelectedTime.Value.Second);
            log.ReSet(StartTime);
            SyncTitle();

            //filePath = $"dailynote_{DateTime.Now.ToString("yyyyMMdd")}.txt";

            txtAllNote.Text = log.GetDailyNotes();
        }

        private void clipCopy(object sender, MouseButtonEventArgs e)
        {
            Copy();
        }

        private void Copy()
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

        private void txtCopy_Click(object sender, RoutedEventArgs e)
        {
            Copy();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }

        private void btnRevert_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("是否确认撤销最后一条记录?", "提示", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            log.RevertLast();
            Copy();

            SyncTitle();
        }

        private void btnResetLastEndTime_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("是否确认重置时间? p.s. 重置操作仅修改第后一条记录的结束时间", "提示", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            var data = DateTime.Now;

            //重置时间
            var endTime = new DateTime(data.Year, data.Month, data.Day,
                lastTime.SelectedTime.Value.Hour, lastTime.SelectedTime.Value.Minute, lastTime.SelectedTime.Value.Second);
            log.ReSetEndTime(endTime);
            SyncTitle();

            //txtAllNote.Text = log.GetDailyNotes();
        }
    }
}
