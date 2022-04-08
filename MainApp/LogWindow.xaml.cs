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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MainApp
{
    /// <summary>
    /// LogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LogWindow : Window
    {
        public LogWindow()
        {
            InitializeComponent();

            // 鼠标左键拖动
            MouseLeftButtonDown += (sender, e) =>
            {
                this.DragMove();
                int minNum = 20;

                //当前窗体位置
                //this.Left  his.Top 

                //左
                if (this.Left < minNum)
                    DoMove((int)Left, minNum, "Left");
                //上
                if (this.Top < minNum)
                    DoMove((int)Top, minNum, "Top");
                //右                
                if (SystemParameters.PrimaryScreenWidth - Width - minNum < this.Left)
                    //当前屏幕分辨率  
                    DoMove((int)Left, (int)(SystemParameters.PrimaryScreenWidth - Width - minNum), "Left");
                //下
                if (SystemParameters.PrimaryScreenHeight - Height - minNum < this.Top)
                    //当前屏幕分辨率  
                    DoMove((int)Top, (int)(SystemParameters.PrimaryScreenHeight - Height - minNum), "Top");

                /// <summary>
                /// 移动动画
                /// </summary>
                /// <param name="from"></param>
                /// <param name="to"></param>
                /// <param name="path"></param>
                void DoMove(int from, int to, string path)
                {
                    var sb = new Storyboard();
                    var da = new DoubleAnimation()
                    {
                        From = from,
                        To = to,
                        Duration = TimeSpan.FromSeconds(0.2)
                    };
                    Storyboard.SetTarget(da, this);
                    Storyboard.SetTargetProperty(da, new PropertyPath($"({path})"));
                    sb.Children.Add(da);
                    sb.Completed += (comO, ComE) => { sb.Remove(this); };
                    sb.Begin(this, true);
                }
            };
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
