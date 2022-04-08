using RadarControl;
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

namespace MainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : BaseWindow
    {
        private Random random = new Random(DateTime.Now.Millisecond);

        public MainWindow()
        {
            InitializeComponent();

           

        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            //WindowState = WindowState.Minimized;
            //ShowInTaskbar = false;
        }
        /// <summary>
        /// 最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_max_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
            ShowOrhide();
        }
        /// <summary>
        /// 正常大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_normal_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
            ShowOrhide();
        }
        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_min_Click(object sender, RoutedEventArgs e)
        {
            //WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 双击最大化或者是还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                ShowOrhide();
            }
        }
        /// <summary>
        /// 显示或者隐藏按钮
        /// </summary>
        private void ShowOrhide()
        {
            switch (WindowState)
            {
                case WindowState.Normal:

                    break;
                case WindowState.Minimized:
                    break;
                case WindowState.Maximized:

                    break;
                default:
                    break;
            }
        }





    }
}
