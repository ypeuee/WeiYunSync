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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random random = new Random(DateTime.Now.Millisecond);
        private string[] keys = new string[] { "忍", "体", "幻", "贤", "力", "速", "精", "印" };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //只需设置一次，除非顶点数有变化
            chart.AttributeCount = keys.Length;
            for (int i = 0; i < chart.AttributeCount; i++)
            {
                chart[i].Name = keys[i];
            }
            chart_MouseDown(this, null);
        }

        private void chart_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //切换随机属性
            for (int i = 0; i < chart.AttributeCount; i++)
            {
                chart[i].Percent = random.Next(1, 11) / (double)10;
            }
        }

        private void meter_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (meter.IsStarted)
            {
                meter.Stop();
            }
            else
            {
                meter.Start();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            //添加信号源
            RadarSignal rs = new RadarSignal(30, new SolidColorBrush(Colors.Red),
                random.Next((int)RadarMeter.MinDistance, (int)RadarMeter.MaxDistance + 1), random.Next(0, 360));
            meter.SignalCollection.Add(rs);
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            //删除最后一个信号源
            if (meter.SignalCollection.Count > 0)
            {
                meter.SignalCollection.RemoveAt(meter.SignalCollection.Count - 1);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            meter.SignalCollection.Clear();
        }

        private void Come_Click(object sender, RoutedEventArgs e)
        {
            //使所有信号源靠近圆心
            foreach (RadarSignal signal in meter.SignalCollection)
            {
                signal.Distance -= 10;
            }
        }

        private void Leave_Click(object sender, RoutedEventArgs e)
        {
            //使所有信号源远离圆心
            foreach (RadarSignal signal in meter.SignalCollection)
            {
                signal.Distance += 10;
            }
        }

        private void Clockwise_Click(object sender, RoutedEventArgs e)
        {
            //调整信号源相对中轴线的偏移角度（顺时针）
            foreach (RadarSignal signal in meter.SignalCollection)
            {
                signal.Angle += 10;
            }
        }

        private void AntiClock_Click(object sender, RoutedEventArgs e)
        {
            //调整信号源相对中轴线的偏移角度（逆时针）
            foreach (RadarSignal signal in meter.SignalCollection)
            {
                signal.Angle -= 10;
            }
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            //放大信号源
            foreach (RadarSignal signal in meter.SignalCollection)
            {
                signal.Radius += 3;
            }
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            //缩小信号源
            foreach (RadarSignal signal in meter.SignalCollection)
            {
                signal.Radius -= 3;
            }
        }

        private void Red_Click(object sender, RoutedEventArgs e)
        {
            //设置红色信号源
            foreach (RadarSignal signal in meter.SignalCollection)
            {
                signal.Fill = new SolidColorBrush(Colors.Red);
            }
        }

        private void Yellow_Click(object sender, RoutedEventArgs e)
        {
            //设置黄色信号源
            foreach (RadarSignal signal in meter.SignalCollection)
            {
                signal.Fill = new SolidColorBrush(Colors.Yellow);
            }
        }
    }
}
