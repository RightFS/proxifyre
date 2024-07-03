using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpeedUp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotifyIcon notifyIcon;
        public MainWindow()
        {
            InitializeComponent();
            // 初始化NotifyIcon
            notifyIcon = new NotifyIcon
            {

                Icon = LoadIconFromResource("SpeedUp.icons.icon.ico"), // 设置托盘图标的路径
                Visible = false, // 初始时不显示
                Text = "我的应用" // 鼠标悬停时的文本
            };

            notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
        }
        private Icon LoadIconFromResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException("Could not find embedded resource", resourceName);
                }
                return new Icon(stream);
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 检查是否为左键点击
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // 拖动窗口
                DragMove();
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            // 当窗口最小化时隐藏任务栏图标并显示托盘图标
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                notifyIcon.Visible = true;
            }
        }

        private void NotifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // 双击托盘图标时恢复窗口
            Show();
            WindowState = WindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 清理，避免托盘图标残留
            notifyIcon.Dispose();
        }
    }
}
