using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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

namespace SpeedUp.Frames
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class SpeedUpPage : Page
    {
        private string currentItem = "";
        private Button currentButton = null;
        public SpeedUpPage()
        {
            InitializeComponent();
        }
        private void Page_DragEnter(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }
        public bool AddApplication(string file)
        {
            //TODO: 过滤浏览器程序 过滤自己
            var icon = GetIconFromExe(file);
            if (icon != null)
            {
                var image = new System.Windows.Controls.Image
                {
                    Source = icon,
                    Width = 48,
                    Height = 48,
                    Stretch = Stretch.Fill,
                    Margin = new Thickness(5)
                };
                var textBlock = new TextBlock
                {
                    Text = System.IO.Path.GetFileNameWithoutExtension(file),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextTrimming = TextTrimming.CharacterEllipsis,
                    Margin = new Thickness(5)
                };
                var stackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                stackPanel.Children.Add(image);
                stackPanel.Children.Add(textBlock);
                var button = new Button
                {
                    Name = "Button" + Utils.ComputeMd5Hash(file),
                    Content = stackPanel,
                    Width = 80,
                    Height = 75,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(2),
                    Background = System.Windows.Media.Brushes.Transparent,
                    Style = (Style)FindResource("HoverNormal"),
                    Tag = file,
                };

                button.Click += SelectApplication;
                if (IconWrapPanel.Children.Count >= 12)
                {
                    if (Application.Current.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowToast("已达上限");
                    }
                    return false;
                }

                // Check if a button with the same Tag already exists
                if (!IconWrapPanel.Children.OfType<Button>().Any(b => b.Tag.ToString() == file))
                {
                    IconWrapPanel.Children.Add(button);
                    GlobalData.FilePaths.Add(file);
                    return true;
                }
                else
                {
                    if (Application.Current.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowToast("应用程序已存在");
                        return false;
                    }
                }
            }
            return false;
        }
        public void RemoveSelectedApplication()
        {
            if (currentItem != "")
            {
                RemoveApplication(currentItem);
            }
        }
        private void RemoveApplication(string file)
        {
            var button = IconWrapPanel.Children.OfType<Button>().FirstOrDefault(b => b.Tag.ToString() == file);
            if (button != null)
            {
                IconWrapPanel.Children.Remove(button);
                GlobalData.FilePaths.Remove(file);
            }
        }
        private void Page_Drop(object sender, DragEventArgs e)
        {
            if (GlobalData.IsRunning)
            {
                e.Effects = DragDropEffects.None;
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.ShowToast("请先停止加速");
                }
                return;
            }
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    //FilePathTextBox.Text = files[0]; // 显示第一个文件的路径
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (files[i].EndsWith("exe") || files[i].EndsWith("lnk"))
                        {
                            AddApplication(files[i]);
                        }
                        Console.WriteLine(files[i]);
                        //FilePathTextBox.Text += files[i] + "\n";
                    }
                }
            }
        }
        public void RunSelectedApplication()
        {

           if (currentItem != "")
            {
                RunApplication(currentItem);
            }
        }
        private void RunApplication(string exePath)
        {
            try
            {
                Process.Start(exePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void SelectApplication(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string exePath)
            {
                currentItem = exePath;
                if (currentButton != null)
                {
                    currentButton.Background = System.Windows.Media.Brushes.Transparent;
                }
                currentButton = button;
                Console.WriteLine(currentItem);
                button.Background = System.Windows.Media.Brushes.LightGray;
            }
        }
        private BitmapSource GetIconFromExe(string exePath)
        {
            if (exePath.EndsWith("lnk"))
            {
                exePath = Utils.GetShortcutTarget(exePath);
            }
            Icon icon = Icon.ExtractAssociatedIcon(exePath);
            if (icon != null)
            {
                using (Bitmap bitmap = icon.ToBitmap())
                {
                    bitmap.MakeTransparent(); // Make the background transparent
                    IntPtr hBitmap = bitmap.GetHbitmap();
                    try
                    {
                        return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                            hBitmap,
                            IntPtr.Zero,
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());
                    }
                    finally
                    {
                        DeleteObject(hBitmap); // Clean up the HBitmap to avoid memory leaks
                    }
                }
            }
            return null;
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

    }
}
