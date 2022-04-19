using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// UcScan.xaml 的交互逻辑
    /// </summary>
    public partial class UcScan : UserControl
    {
        public UcScan()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 扫描结束事件
        /// </summary>
        [Category("Behavior")]
        public event RoutedEventHandler StopClick;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (meter.IsStarted)
            {
                meter.Stop();
                this.Visibility = Visibility.Hidden;

                if (StopClick != null)
                    StopClick(sender, e);
            }
            else
            {
                meter.Start();
            }
        }
        private void meter_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (meter.IsStarted)
            {
                meter.Stop();
                this.Visibility = Visibility.Hidden;

                if (StopClick != null)
                    StopClick(sender, e);

            }
            else
            {
                meter.Start();
            }
        }
    }
}
