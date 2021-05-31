using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DailyNote
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

        public MainWindow()
        {
            InitializeComponent();
            LastTime = DateTime.Now;
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

            Clipboard.SetText(txtAllNote.Text);

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
            if (result == MessageBoxResult.Yes)
            {
                if (LastTime.Date == DateTime.Now.Date)
                {
                    return;
                }
                LastTime = DateTime.Now;

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
        }
    }
}
