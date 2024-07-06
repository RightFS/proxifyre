using System;
using System.Collections.Generic;
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

        private void Page_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    //FilePathTextBox.Text = files[0]; // 显示第一个文件的路径
                    for(int i = 0; i < files.Length; i++)
                    {
                        Console.WriteLine(files[i]);
                        //FilePathTextBox.Text += files[i] + "\n";
                    }
                }
            }
        }
    }
}
