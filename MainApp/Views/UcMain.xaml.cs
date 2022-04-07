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

namespace MainApp.Views
{
    /// <summary>
    /// UcMain.xaml 的交互逻辑
    /// </summary>
    public partial class UcMain : UserControl
    {
        public UcMain()
        {
            InitializeComponent();
        }

        #region 开始按钮改变格式
        private void btnStart_MouseEnter(object sender, MouseEventArgs e)
        {
            pathStop.Visibility = Visibility.Hidden;
            pathStart.Visibility = Visibility.Visible;
        }
        private void btnStart_MouseLeave(object sender, MouseEventArgs e)
        {
            pathStop.Visibility = Visibility.Visible;
            pathStart.Visibility = Visibility.Hidden;
        }
        #endregion

        /// <summary>
        /// 开始按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
