using DailyNote.Configs;
using System.IO;
using System.Windows;

namespace DailyNote.Windows
{
    /// <summary>
    /// AboutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();

            txtVer.Text = VersionConfig.VersionStr;


            LoadTips();
        }

        void LoadTips()
        {
            var readmePath = "ReadMe.md";

            if (!File.Exists(readmePath))
            {
                return;
            }
            File.ReadAllText(readmePath);
            txtTips.Text = File.ReadAllText(readmePath);
        }
    }


}
