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
using System.Windows.Threading;

namespace SpeedUp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public void ShowToast(string message)
        {
            var duration = TimeSpan.FromSeconds(1.5);
            ToastText.Text = message;
            ToastPopup.IsOpen = true;

            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = duration
            };
            timer.Tick += (s, args) =>
            {
                ToastPopup.IsOpen = false;
                timer.Stop();
            };
            timer.Start();
        }

        private NotifyIcon notifyIcon;
        public MainWindow()
        {
            InitializeComponent();
            // 初始化NotifyIcon
            notifyIcon = new NotifyIcon
            {

                Icon = LoadIconFromResource("SpeedUp.icons.icon.ico"), // 设置托盘图标的路径
                Visible = false, // 初始时不显示
                Text = "双击打开界面" // 鼠标悬停时的文本
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
            Console.WriteLine("Minimize");
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

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if(GlobalData.FilePaths.Count == 0)
            {
                ShowToast("请添加应用程序");
                return;
            }
            GlobalData.IsRunning = !GlobalData.IsRunning;
            Console.WriteLine("Start");
            SwitchButton.Content= GlobalData.IsRunning ? "停止加速" : "开始加速";
        }

        private void AddApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalData.IsRunning)
            {
                ShowToast("请先停止加速");
                return;
            }
            //打开文件选择器
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "可执行文件|*.exe|快捷方式|*.lnk",
                Multiselect = true
            };
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    if(MainFrame.Content is Frames.SpeedUpPage speedUpPage)
                    {
                        if (!speedUpPage.AddApplication(file))
                        {
                            return;
                        }
                    }
                }
            }
        }

        private void RemoveApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            if(GlobalData.IsRunning)
            {
                ShowToast("请先停止加速");
                return;
            }
            if (MainFrame.Content is Frames.SpeedUpPage speedUpPage)
            {
                speedUpPage.RemoveSelectedApplication();
            }
        }

        private void RunApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content is Frames.SpeedUpPage speedUpPage)
            {
                speedUpPage.RunSelectedApplication();
            }
        }
    }
}
